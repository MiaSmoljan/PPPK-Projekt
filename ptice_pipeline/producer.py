import json
from kafka import KafkaProducer

producer = KafkaProducer(
    bootstrap_servers="localhost:9092",
    value_serializer=lambda v: json.dumps(v).encode("utf-8")
)

opazanja = [
    {
        "taxonID": "gbif:2473325",
        "latitude": 45.815,
        "longitude": 15.982,
        "izvor": "OrnitoloskoDrustvoZG",
        "velicina_tijela_cm": 58,
        "status_migracije": "stacionarno"
    },
    {
        "taxonID": "gbif:2473325",
        "latitude": 46.301,
        "longitude": 16.336,
        "izvor": "eBird",
        "tjelesna_temperatura_c": 41.2,
        "obrazac_leta": "ravni let"
    },
    {
        "taxonID": "gbif:2473324",
        "latitude": 45.5,
        "longitude": 18.7,
        "izvor": "Xeno-canto zajednica",
        "stanište": "šuma"
    },
]

for opazanje in opazanja:
    producer.send("bird-observations", opazanje)
    print(f"Poslano: {opazanje}")

producer.flush()
print("Sve poruke poslane na topic 'bird-observations'.")