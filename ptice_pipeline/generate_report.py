import sys
import csv
import difflib
from collections import defaultdict
from pymongo import MongoClient

client = MongoClient("mongodb://localhost:27017")
db = client["ptice_db"]
observations_col = db["observations"]

filter_naziv = sys.argv[1] if len(sys.argv) > 1 else None

stats = defaultdict(lambda: {"broj": 0, "lokacije": [], "pouzdanosti": []})

for obs in observations_col.find({"best_match": {"$ne": None}}):
    najbolji = obs["best_match"]
    naziv = najbolji.get("common_name", "Nepoznato").strip()
    stats[naziv]["broj"] += 1
    stats[naziv]["pouzdanosti"].append(najbolji.get("confidence", 0))
    lok = obs.get("location")
    if lok:
        stats[naziv]["lokacije"].append(f"{lok.get('lat')},{lok.get('lon')}")

nazivi_vrsta = list(stats.keys())

if filter_naziv and nazivi_vrsta:
    podudarni = difflib.get_close_matches(filter_naziv, nazivi_vrsta, n=len(nazivi_vrsta), cutoff=0.4)
    print(f"Fuzzy filter '{filter_naziv}' -> podudarne vrste: {podudarni}")
    nazivi_vrsta = podudarni
elif filter_naziv and not nazivi_vrsta:
    print("Nema vrsta u bazi za filtrirati.")

with open("izvjestaj_ptice.csv", "w", newline="", encoding="utf-8-sig") as f:
    writer = csv.writer(f, delimiter=';')
    writer.writerow(["Naziv vrste", "Broj klasificiranih opažanja", "Prosječna pouzdanost (%)", "Lokacije"])

    for naziv in sorted(nazivi_vrsta):
        podaci = stats[naziv]
        prosjek = sum(podaci["pouzdanosti"]) / len(podaci["pouzdanosti"]) * 100
        prosjek_str = f"{prosjek:.1f}".replace(".", ",")
        lokacije_str = "; ".join(sorted(set(podaci["lokacije"])))
        writer.writerow([naziv, podaci["broj"], prosjek_str, lokacije_str])

print(f"Izvještaj spremljen: izvjestaj_ptice.csv ({len(nazivi_vrsta)} vrsta)")