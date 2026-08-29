import json
from kafka import KafkaConsumer
from pymongo import MongoClient

client = MongoClient("mongodb://localhost:27017")
db = client["ptice_db"]
kafka_opazanja_col = db["kafka_opazanja"]

consumer = KafkaConsumer(
    "bird-observations",
    bootstrap_servers="localhost:9092",
    auto_offset_reset="earliest",    
    enable_auto_commit=True,
    group_id="ptice-consumer-group",  
    value_deserializer=lambda v: json.loads(v.decode("utf-8")),
    consumer_timeout_ms=20000        
)

print("Čitam poruke prisutne na Kafka brokeru...")
broj = 0

for poruka in consumer:
    opazanje = poruka.value
    kafka_opazanja_col.insert_one(opazanje)
    broj += 1
    print(f"Spremljeno opažanje: {opazanje}")

print(f"Gotovo! Spremljeno {broj} novih opažanja iz Kafke.")
consumer.close()