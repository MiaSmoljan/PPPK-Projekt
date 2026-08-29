import requests
from pymongo import MongoClient

client = MongoClient("mongodb://localhost:27017")
db = client["ptice_db"]
species_col = db["species"]

print("Dohvaćam podatke s aves.regoch.net...")
resp = requests.get("https://aves.regoch.net/aves.json")
data = resp.json()

print(f"Dohvaćeno {len(data)} zapisa o vrstama.")

broj_dodano = 0
broj_azurirano = 0

for vrsta in data:

    rezultat = species_col.update_one(
        {"taxonID": vrsta.get("taxonID")},
        {"$set": vrsta},
        upsert=True
    )
    if rezultat.upserted_id is not None:
        broj_dodano += 1
    elif rezultat.modified_count > 0:
        broj_azurirano += 1

print(f"Gotovo! Novo dodano: {broj_dodano}, ažurirano: {broj_azurirano}")
print(f"Ukupno zapisa u bazi sada: {species_col.count_documents({})}")