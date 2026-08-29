import os
from pymongo import MongoClient

client = MongoClient("mongodb://localhost:27017")
db = client["ptice_db"]
observations_col = db["observations"]

audio_dir = "audio_files"
obrisano = 0

for obs in observations_col.find():
    putanja = os.path.join(audio_dir, obs["filename"])
    if not os.path.exists(putanja):
        observations_col.delete_one({"_id": obs["_id"]})
        obrisano += 1
        print(f"Obrisan zapis za: {obs['filename']} (datoteka ne postoji lokalno)")

print(f"Gotovo! Obrisano {obrisano} starih zapisa.")