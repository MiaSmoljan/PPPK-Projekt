using System;
using Microsoft.EntityFrameworkCore;
using MedicinskiSustav.Data;
using MedicinskiSustav.Models;
using MedicinskiSustav.Models.MedicinskiSustav.Models;

using var db = new AppDbContext();


if (!db.Doctors.Any())
{
    db.Doctors.AddRange(
        new Doctor { Ime = "Ana", Prezime = "Anić", Specijalizacija = "Kardiologija" },
        new Doctor { Ime = "Ivo", Prezime = "Ivić", Specijalizacija = "Radiologija" },
        new Doctor { Ime = "Petra", Prezime = "Petrić", Specijalizacija = "Dermatologija" }
    );
    db.SaveChanges();
    Console.WriteLine("Liječnici su dodani (prvo pokretanje).");
}

bool kraj = false;
while (!kraj)
{
    Console.WriteLine("- MEDICINSKI SUSTAV -");
    Console.WriteLine(" Pacijenti ");
    Console.WriteLine("1. Prikaži sve pacijente");
    Console.WriteLine("2. Dodaj pacijenta");
    Console.WriteLine("3. Uredi pacijenta");
    Console.WriteLine("4. Obriši pacijenta");
    Console.WriteLine(" Povijest bolesti ");
    Console.WriteLine("5. Prikaži povijest bolesti (za pacijenta)");
    Console.WriteLine("6. Dodaj povijest bolesti");
    Console.WriteLine("7. Uredi povijest bolesti");
    Console.WriteLine("8. Obriši povijest bolesti");
    Console.WriteLine(" Lijekovi ");
    Console.WriteLine("9. Prikaži lijekove (za pacijenta)");
    Console.WriteLine("10. Dodaj lijek");
    Console.WriteLine("11. Uredi lijek");
    Console.WriteLine("12. Obriši lijek");
    Console.WriteLine(" Liječnici ");
    Console.WriteLine("13. Prikaži liječnike");
    Console.WriteLine(" Pregledi ");
    Console.WriteLine("14. Prikaži preglede (za pacijenta)");
    Console.WriteLine("15. Zakaži pregled");
    Console.WriteLine("16. Uredi pregled");
    Console.WriteLine("17. Obriši pregled");
    Console.WriteLine("0. Izlaz");
    Console.Write("Odabir: ");
    string? odabir = Console.ReadLine();

    switch (odabir)
    {
        case "1":
            PrikaziPacijente();
            break;
        case "2":
            DodajPacijenta();
            break;
        case "3":
            UrediPacijenta();
            break;
        case "4":
            ObrisiPacijenta();
            break;
        case "5":
            PrikaziPovijestBolesti();
            break;
        case "6":
            DodajPovijestBolesti();
            break;
        case "7":
            UrediPovijestBolesti();
            break;
        case "8":
            ObrisiPovijestBolesti();
            break;
        case "9":
            PrikaziLijekove();
            break;
        case "10":
            DodajLijek();
            break;
        case "11":
            UrediLijek();
            break;
        case "12":
            ObrisiLijek();
            break;
        case "13":
            PrikaziLijecnike();
            break;
        case "14":
            PrikaziPreglede();
            break;
        case "15":
            ZakaziPregled();
            break;
        case "16":
            UrediPregled();
            break;
        case "17":
            ObrisiPregled();
            break;
        case "0":
            kraj = true;
            break;
        default:
            Console.WriteLine("Nepoznat odabir, pokušaj ponovno.");
            break;
    }
}


void PrikaziPacijente()
{
    var pacijenti = db.Patients.ToList();

    if (!pacijenti.Any())
    {
        Console.WriteLine("Nema pacijenata u bazi.");
        return;
    }

    foreach (var p in pacijenti)
    {
        Console.WriteLine($"[{p.Id}] {p.Ime} {p.Prezime}, OIB: {p.Oib}, rođen/a: {p.DatumRodjenja}");
    }
}

void DodajPacijenta()
{
    Console.Write("Ime: ");
    string ime = Console.ReadLine() ?? "";

    Console.Write("Prezime: ");
    string prezime = Console.ReadLine() ?? "";

    Console.Write("OIB: ");
    string oib = Console.ReadLine() ?? "";

    Console.Write("Datum rođenja (yyyy-mm-dd): ");
    DateOnly datumRodjenja = DateOnly.Parse(Console.ReadLine() ?? "2000-01-01");

    Console.Write("Spol: ");
    string spol = Console.ReadLine() ?? "";

    Console.Write("Adresa boravišta: ");
    string adresaBoravista = Console.ReadLine() ?? "";

    Console.Write("Adresa prebivališta: ");
    string adresaPrebivalista = Console.ReadLine() ?? "";

    var noviPacijent = new Patient
    {
        Ime = ime,
        Prezime = prezime,
        Oib = oib,
        DatumRodjenja = datumRodjenja,
        Spol = spol,
        AdresaBoravista = adresaBoravista,
        AdresaPrebivalista = adresaPrebivalista
    };

    db.Patients.Add(noviPacijent);
    db.SaveChanges();

    Console.WriteLine("Pacijent dodan.");
}

void UrediPacijenta()
{
    Console.Write("Unesi Id pacijenta kojeg želiš urediti: ");
    string unos = Console.ReadLine() ?? "";

    if (!int.TryParse(unos, out int id))
    {
        Console.WriteLine("Neispravan unos - Id mora biti cijeli broj (vidi ga preko opcije 1).");
        return;
    }

    var pacijent = db.Patients.Find(id);
    if (pacijent == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    bool gotovo = false;
    while (!gotovo)
    {
        Console.WriteLine($"\n--- Uređivanje pacijenta [{pacijent.Id}] ---");
        Console.WriteLine($"1. Ime: {pacijent.Ime}");
        Console.WriteLine($"2. Prezime: {pacijent.Prezime}");
        Console.WriteLine($"3. OIB: {pacijent.Oib}");
        Console.WriteLine($"4. Datum rođenja: {pacijent.DatumRodjenja}");
        Console.WriteLine($"5. Spol: {pacijent.Spol}");
        Console.WriteLine($"6. Adresa boravišta: {pacijent.AdresaBoravista}");
        Console.WriteLine($"7. Adresa prebivališta: {pacijent.AdresaPrebivalista}");
        Console.WriteLine("0. Gotovo (spremi i izađi iz uređivanja)");
        Console.Write("Koje polje želiš promijeniti: ");
        string polje = Console.ReadLine() ?? "";

        switch (polje)
        {
            case "1":
                Console.Write("Novo ime: ");
                pacijent.Ime = Console.ReadLine() ?? pacijent.Ime;
                break;
            case "2":
                Console.Write("Novo prezime: ");
                pacijent.Prezime = Console.ReadLine() ?? pacijent.Prezime;
                break;
            case "3":
                Console.Write("Novi OIB: ");
                pacijent.Oib = Console.ReadLine() ?? pacijent.Oib;
                break;
            case "4":
                Console.Write("Novi datum rođenja (yyyy-mm-dd): ");
                string datumUnos = Console.ReadLine() ?? "";
                if (DateOnly.TryParse(datumUnos, out DateOnly noviDatum))
                    pacijent.DatumRodjenja = noviDatum;
                else
                    Console.WriteLine("Neispravan format datuma, polje nije promijenjeno.");
                break;
            case "5":
                Console.Write("Novi spol: ");
                pacijent.Spol = Console.ReadLine() ?? pacijent.Spol;
                break;
            case "6":
                Console.Write("Nova adresa boravišta: ");
                pacijent.AdresaBoravista = Console.ReadLine() ?? pacijent.AdresaBoravista;
                break;
            case "7":
                Console.Write("Nova adresa prebivališta: ");
                pacijent.AdresaPrebivalista = Console.ReadLine() ?? pacijent.AdresaPrebivalista;
                break;
            case "0":
                gotovo = true;
                break;
            default:
                Console.WriteLine("Nepoznata opcija.");
                break;
        }
    }

    db.SaveChanges();
    Console.WriteLine("Pacijent ažuriran.");
}

void ObrisiPacijenta()
{
    Console.Write("Unesi Id pacijenta kojeg želiš obrisati: ");
    string unos = Console.ReadLine() ?? "";

    if (!int.TryParse(unos, out int id))
    {
        Console.WriteLine("Neispravan unos - Id mora biti cijeli broj (vidi ga preko opcije 1).");
        return;
    }

    var pacijent = db.Patients.Find(id);
    if (pacijent == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    db.Patients.Remove(pacijent);
    db.SaveChanges();
    Console.WriteLine("Pacijent obrisan.");
}

void PrikaziPovijestBolesti()
{
    Console.Write("Unesi Id pacijenta: ");
    if (!int.TryParse(Console.ReadLine(), out int patientId))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var pacijent = db.Patients
        .Include(p => p.PovijestBolesti)
        .FirstOrDefault(p => p.Id == patientId);

    if (pacijent == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    if (!pacijent.PovijestBolesti.Any())
    {
        Console.WriteLine("Pacijent nema zapisa povijesti bolesti.");
        return;
    }

    foreach (var pb in pacijent.PovijestBolesti)
    {
        Console.WriteLine($"[{pb.Id}] {pb.Bolest} — od: {pb.DatumPocetka}, do: {(pb.DatumZavrsetka.HasValue ? pb.DatumZavrsetka.ToString() : "još traje")}");
    }
}

void DodajPovijestBolesti()
{
    Console.Write("Unesi Id pacijenta: ");
    if (!int.TryParse(Console.ReadLine(), out int patientId) || db.Patients.Find(patientId) == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    Console.Write("Naziv bolesti: ");
    string bolest = Console.ReadLine() ?? "";

    Console.Write("Datum početka (yyyy-mm-dd): ");
    if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly datumPocetka))
    {
        Console.WriteLine("Neispravan format datuma.");
        return;
    }

    Console.Write("Datum završetka (yyyy-mm-dd, ili ostavi prazno ako bolest još traje): ");
    string unosZavrsetka = Console.ReadLine() ?? "";
    DateOnly? datumZavrsetka = null;
    if (!string.IsNullOrWhiteSpace(unosZavrsetka) && DateOnly.TryParse(unosZavrsetka, out DateOnly dz))
    {
        datumZavrsetka = dz;
    }

    var novaPovijest = new MedicalHistory
    {
        PatientId = patientId,
        Bolest = bolest,
        DatumPocetka = datumPocetka,
        DatumZavrsetka = datumZavrsetka
    };

    db.MedicalHistories.Add(novaPovijest);
    db.SaveChanges();
    Console.WriteLine("Zapis povijesti bolesti dodan.");
}

void UrediPovijestBolesti()
{
    Console.Write("Unesi Id zapisa povijesti bolesti (vidi ga preko opcije 5): ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var zapis = db.MedicalHistories.Find(id);
    if (zapis == null)
    {
        Console.WriteLine("Zapis s tim Id-em ne postoji.");
        return;
    }

    Console.Write($"Novi naziv bolesti (trenutno: {zapis.Bolest}, ostavi prazno da ne mijenjaš): ");
    string noviBolest = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(noviBolest))
        zapis.Bolest = noviBolest;

    Console.Write($"Novi datum završetka (trenutno: {(zapis.DatumZavrsetka.HasValue ? zapis.DatumZavrsetka.ToString() : "još traje")}, ostavi prazno da ne mijenjaš): ");
    string noviZavrsetak = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(noviZavrsetak) && DateOnly.TryParse(noviZavrsetak, out DateOnly dz))
        zapis.DatumZavrsetka = dz;

    db.SaveChanges();
    Console.WriteLine("Zapis ažuriran.");
}

void ObrisiPovijestBolesti()
{
    Console.Write("Unesi Id zapisa povijesti bolesti kojeg želiš obrisati: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var zapis = db.MedicalHistories.Find(id);
    if (zapis == null)
    {
        Console.WriteLine("Zapis s tim Id-em ne postoji.");
        return;
    }

    db.MedicalHistories.Remove(zapis);
    db.SaveChanges();
    Console.WriteLine("Zapis obrisan.");
}

void PrikaziLijekove()
{
    Console.Write("Unesi Id pacijenta: ");
    if (!int.TryParse(Console.ReadLine(), out int patientId))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var pacijent = db.Patients
        .Include(p => p.Lijekovi)
        .FirstOrDefault(p => p.Id == patientId);

    if (pacijent == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    if (!pacijent.Lijekovi.Any())
    {
        Console.WriteLine("Pacijent nema propisanih lijekova.");
        return;
    }

    foreach (var lijek in pacijent.Lijekovi)
    {
        Console.WriteLine($"[{lijek.Id}] {lijek.NazivLijeka} — doza: {lijek.Doza}, učestalost: {lijek.Ucestalost}");
    }
}

void DodajLijek()
{
    Console.Write("Unesi Id pacijenta: ");
    if (!int.TryParse(Console.ReadLine(), out int patientId) || db.Patients.Find(patientId) == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    Console.Write("Naziv lijeka: ");
    string naziv = Console.ReadLine() ?? "";

    Console.Write("Doza (npr. 500mg, 2 tablete): ");
    string doza = Console.ReadLine() ?? "";

    Console.Write("Učestalost (npr. 3x dnevno, svaki drugi dan): ");
    string ucestalost = Console.ReadLine() ?? "";

    var noviLijek = new Prescription
    {
        PatientId = patientId,
        NazivLijeka = naziv,
        Doza = doza,
        Ucestalost = ucestalost
    };

    db.Prescriptions.Add(noviLijek);
    db.SaveChanges();
    Console.WriteLine("Lijek dodan.");
}

void UrediLijek()
{
    Console.Write("Unesi Id lijeka (vidi ga preko opcije 9): ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var lijek = db.Prescriptions.Find(id);
    if (lijek == null)
    {
        Console.WriteLine("Lijek s tim Id-em ne postoji.");
        return;
    }

    Console.Write($"Novi naziv (trenutno: {lijek.NazivLijeka}, ostavi prazno da ne mijenjaš): ");
    string noviNaziv = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(noviNaziv))
        lijek.NazivLijeka = noviNaziv;

    Console.Write($"Nova doza (trenutno: {lijek.Doza}, ostavi prazno da ne mijenjaš): ");
    string novaDoza = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(novaDoza))
        lijek.Doza = novaDoza;

    Console.Write($"Nova učestalost (trenutno: {lijek.Ucestalost}, ostavi prazno da ne mijenjaš): ");
    string novaUcestalost = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(novaUcestalost))
        lijek.Ucestalost = novaUcestalost;

    db.SaveChanges();
    Console.WriteLine("Lijek ažuriran.");
}

void ObrisiLijek()
{
    Console.Write("Unesi Id lijeka kojeg želiš obrisati: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var lijek = db.Prescriptions.Find(id);
    if (lijek == null)
    {
        Console.WriteLine("Lijek s tim Id-em ne postoji.");
        return;
    }

    db.Prescriptions.Remove(lijek);
    db.SaveChanges();
    Console.WriteLine("Lijek obrisan.");
}

void PrikaziLijecnike()
{
    var lijecnici = db.Doctors.ToList();

    foreach (var d in lijecnici)
    {
        Console.WriteLine($"[{d.Id}] {d.Ime} {d.Prezime} — {d.Specijalizacija}");
    }
}

void PrikaziPreglede()
{
    Console.Write("Unesi Id pacijenta: ");
    if (!int.TryParse(Console.ReadLine(), out int patientId))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var pacijent = db.Patients
        .Include(p => p.Pregledi)
        .ThenInclude(e => e.Doctor)
        .FirstOrDefault(p => p.Id == patientId);

    if (pacijent == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    if (!pacijent.Pregledi.Any())
    {
        Console.WriteLine("Pacijent nema zakazanih pregleda.");
        return;
    }

    foreach (var pregled in pacijent.Pregledi)
    {
        string lijecnikIme = pregled.Doctor != null ? $"{pregled.Doctor.Ime} {pregled.Doctor.Prezime}" : "nepoznat";
        Console.WriteLine($"[{pregled.Id}] {pregled.Tip} — termin: {pregled.Termin}, liječnik: {lijecnikIme}");
    }
}

void ZakaziPregled()
{
    Console.Write("Unesi Id pacijenta: ");
    if (!int.TryParse(Console.ReadLine(), out int patientId) || db.Patients.Find(patientId) == null)
    {
        Console.WriteLine("Pacijent s tim Id-em ne postoji.");
        return;
    }

    Console.WriteLine("Dostupni liječnici:");
    PrikaziLijecnike();
    Console.Write("Unesi Id liječnika: ");
    if (!int.TryParse(Console.ReadLine(), out int doctorId) || db.Doctors.Find(doctorId) == null)
    {
        Console.WriteLine("Liječnik s tim Id-em ne postoji.");
        return;
    }

    Console.WriteLine("Tipovi pregleda: CT, MR, ULTRA, EKG, ECHO, OKO, DERM, DENTA, MAMMO, EEG");
    Console.Write("Unesi tip pregleda: ");
    string tipUnos = Console.ReadLine() ?? "";
    if (!Enum.TryParse<TipPregleda>(tipUnos, true, out TipPregleda tip))
    {
        Console.WriteLine("Nepoznat tip pregleda.");
        return;
    }

    Console.Write("Termin (yyyy-mm-dd HH:mm): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime termin))
    {
        Console.WriteLine("Neispravan format datuma/vremena.");
        return;
    }
    termin = DateTime.SpecifyKind(termin, DateTimeKind.Utc);

    var noviPregled = new Exam
    {
        PatientId = patientId,
        DoctorId = doctorId,
        Tip = tip,
        Termin = termin
    };

    db.Exams.Add(noviPregled);
    db.SaveChanges();
    Console.WriteLine("Pregled zakazan.");
}

void UrediPregled()
{
    Console.Write("Unesi Id pregleda (vidi ga preko opcije 14): ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var pregled = db.Exams.Find(id);
    if (pregled == null)
    {
        Console.WriteLine("Pregled s tim Id-em ne postoji.");
        return;
    }

    Console.Write($"Novi termin (trenutno: {pregled.Termin}, format yyyy-mm-dd HH:mm, ostavi prazno da ne mijenjaš): ");
    string noviTerminUnos = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(noviTerminUnos) && DateTime.TryParse(noviTerminUnos, out DateTime noviTermin))
        pregled.Termin = noviTermin;

    Console.Write("Termin (yyyy-mm-dd HH:mm): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime termin))
    {
        Console.WriteLine("Neispravan format datuma/vremena.");
        return;
    }

    termin = DateTime.SpecifyKind(termin, DateTimeKind.Utc);
    Console.Write($"Novi Id liječnika (trenutno: {pregled.DoctorId}, ostavi prazno da ne mijenjaš): ");
    string noviDoctorUnos = Console.ReadLine() ?? "";
    if (!string.IsNullOrWhiteSpace(noviDoctorUnos) && int.TryParse(noviDoctorUnos, out int noviDoctorId) && db.Doctors.Find(noviDoctorId) != null)
        pregled.DoctorId = noviDoctorId;

    db.SaveChanges();
    Console.WriteLine("Pregled ažuriran.");
}

void ObrisiPregled()
{
    Console.Write("Unesi Id pregleda kojeg želiš obrisati: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Neispravan unos.");
        return;
    }

    var pregled = db.Exams.Find(id);
    if (pregled == null)
    {
        Console.WriteLine("Pregled s tim Id-em ne postoji.");
        return;
    }

    db.Exams.Remove(pregled);
    db.SaveChanges();
    Console.WriteLine("Pregled obrisan.");
}