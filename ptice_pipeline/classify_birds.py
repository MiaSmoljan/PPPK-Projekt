import requests
import os
import json
import io
from datetime import datetime, timezone
from pymongo import MongoClient
from minio import Minio


client = MongoClient("mongodb://localhost:27017")
db = client["ptice_db"]
observations_col = db["observations"]
species_col = db["species"]

minio_client = Minio(
    "localhost:9000",
    access_key="minioadmin",
    secret_key="minioadmin",
    secure=False
)

log_bucket = "classify-logs"
if not minio_client.bucket_exists(log_bucket):
    minio_client.make_bucket(log_bucket)
    print(f"Bucket '{log_bucket}' kreiran.")

audio_dir = "audio_files"


neklasificirano = list(observations_col.find({"classification": {"$exists": False}}))
print(f"Pronađeno {len(neklasificirano)} neklasificiranih opažanja.")

for obs in neklasificirano:
    filename = obs["filename"]
    putanja = os.path.join(audio_dir, filename)

    if not os.path.exists(putanja):
        print(f"Preskačem '{filename}' - datoteka nije pronađena lokalno.")
        continue

    print(f"Klasificiram: {filename}")
    with open(putanja, "rb") as f:
        resp = requests.post("https://aves.regoch.net/api/classify", files={"file": f})

    if resp.status_code != 200:
        print(f"  Greška ({resp.status_code}), preskačem.")
        continue

    rezultat = resp.json()

    log_entry = {
        "timestamp": datetime.now(timezone.utc).isoformat(),
        "filename": filename,
        "status_code": resp.status_code,
        "response": rezultat
    }
    log_bytes = json.dumps(log_entry, indent=2).encode("utf-8")
    log_name = f"log_{obs['_id']}.json"
    minio_client.put_object(
        log_bucket, log_name, io.BytesIO(log_bytes),
        length=len(log_bytes), content_type="application/json"
    )

 
    najbolji = None
    if rezultat.get("results"):
        najbolji = max(rezultat["results"], key=lambda r: r["confidence"])


    povezana_vrsta = None
    if najbolji:
        povezana_vrsta = species_col.find_one({"canonicalName": najbolji["scientific_name"]})

    observations_col.update_one(
        {"_id": obs["_id"]},
        {"$set": {
            "classification": rezultat,
            "best_match": najbolji,
            "species_ref": povezana_vrsta["taxonID"] if povezana_vrsta else None
        }}
    )

    if najbolji:
        print(f"  -> {najbolji['common_name']} ({najbolji['scientific_name']}), pouzdanost: {najbolji['confidence']:.1%}")
    else:
        print("  -> Nema prepoznatih rezultata.")

print("Gotovo!")