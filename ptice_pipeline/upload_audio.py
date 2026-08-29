import os
import uuid
from minio import Minio
from pymongo import MongoClient

minio_client = Minio(
    "localhost:9000",
    access_key="minioadmin",
    secret_key="minioadmin",
    secure=False  # secure=False jer koristimo obični http, ne https
)

bucket_name = "bird-audio"

if not minio_client.bucket_exists(bucket_name):
    minio_client.make_bucket(bucket_name)
    print(f"Bucket '{bucket_name}' kreiran.")
else:
    print(f"Bucket '{bucket_name}' već postoji.")


client = MongoClient("mongodb://localhost:27017")
db = client["ptice_db"]
observations_col = db["observations"]


lokacija = {"lat": 45.815, "lon": 15.982}  # Zagreb, kao primjer


audio_dir = "audio_files"
datoteke = os.listdir(audio_dir)

print(f"Pronađeno {len(datoteke)} datoteka za upload.")

for filename in datoteke:
    putanja = os.path.join(audio_dir, filename)

    if observations_col.find_one({"filename": filename}):
        print(f"Preskačem '{filename}' - već je uploadano.")
        continue

    object_name = f"{uuid.uuid4()}_{filename}"

    minio_client.fput_object(bucket_name, object_name, putanja)
    print(f"Uploadano: {filename} -> {object_name}")

    observations_col.insert_one({
        "filename": filename,
        "minio_object": object_name,
        "location": lokacija
    })

print("Gotovo!")