-- Database: "Ksiegarnia"

-- DROP DATABASE "Ksiegarnia";

CREATE DATABASE "Ksiegarnia"
  WITH OWNER = postgres
       ENCODING = 'UTF8'
       TABLESPACE = pg_default
       LC_COLLATE = 'Polish_Poland.1250'
       LC_CTYPE = 'Polish_Poland.1250'
       CONNECTION LIMIT = -1;



CREATE TABLE Autorzy
(
	ID_Autora serial PRIMARY KEY,
	Imie varchar,
	Nazwisko varchar
);

CREATE TABLE Kategorie
(
	ID_Kategorii serial PRIMARY KEY,
	Nazwa varchar,
	Opis varchar
);

CREATE TABLE Sprzedaze
(
	ID_Sprzedazy serial PRIMARY KEY,
	Data_sprzedazy date
);

CREATE TABLE Ksiazki
(
	ID_Ksiazki serial PRIMARY KEY,
	Tytul varchar,
	Liczba_dostepnych int,
	Cena_dostawa float,
	Cena_sprzedaz float,
	ISBN varchar,
	ID_Autora int REFERENCES Autorzy(ID_Autora) NOT NULL,
	ID_Kategorii int REFERENCES Kategorie(ID_Kategorii) NOT NULL
);

CREATE TABLE Dostawy
(
	ID_Dostawy serial PRIMARY KEY,
	Data_dostawy date,
	Dostawca varchar
);

CREATE TABLE Sprzedaze_Ksiazki
(
	ID_Sprzedazy int REFERENCES Sprzedaze(ID_Sprzedazy),
	ID_Ksiazki int REFERENCES Ksiazki(ID_Ksiazki),
	Liczba int,
	PRIMARY KEY(ID_Sprzedazy,ID_Ksiazki)
);

CREATE TABLE Dostawy_Ksiazki
(
	ID_Dostawy int REFERENCES Dostawy(ID_Dostawy),
	ID_Ksiazki int REFERENCES Ksiazki(ID_Ksiazki),
	Liczba int,
	PRIMARY KEY(ID_Dostawy,ID_Ksiazki)
);

CREATE TABLE Uprawnienia
(
	ID_Uprawnienia serial PRIMARY KEY,
	Instrukcja varchar
	Nazwa_tabeli varchar
);

CREATE TABLE Rolee
(
	ID_Roli serial PRIMARY KEY,
	Nazwa varchar
);

CREATE TABLE Uzytkownicy
(
	ID_Uzytkownika serial PRIMARY KEY,
	Login varchar,
	Nazwa varchar,
	Haslo varchar
);

CREATE TABLE Sesje
(
	ID_Sesji varchar,
	ID_Uzytkownika int REFERENCES Uzytkownicy(ID_Uzytkownika),
	ID_Roli int REFERENCES Rolee(ID_Roli),
	Data_waznosci int,
	PRIMARY KEY(ID_Sesji)
);

CREATE TABLE Uzytkownicy_Role
(
	ID_Uzytkownika int REFERENCES Uzytkownicy(ID_Uzytkownika),
	ID_Roli int REFERENCES Rolee(ID_Roli),
	PRIMARY KEY(ID_Uzytkownika,ID_Roli)
);

CREATE TABLE Uprawnienia_Role
(
	ID_Uprawnienia int REFERENCES Uprawnienia(ID_Uprawnienia),
	ID_Roli int REFERENCES Rolee(ID_Roli),
	PRIMARY KEY(ID_Uprawnienia,ID_Roli)
);