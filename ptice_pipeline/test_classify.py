import requests
import os

audio_dir = "audio_files"
prva_datoteka = os.listdir(audio_dir)[0]
putanja = os.path.join(audio_dir, prva_datoteka)

print(f"Šaljem datoteku: {prva_datoteka}")

with open(putanja, "rb") as f:
    resp = requests.post(
        "https://aves.regoch.net/api/classify",
        files={"file": f}
    )

print(f"Status kod: {resp.status_code}")
print("Odgovor:")
print(resp.text)