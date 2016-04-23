using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BSK.Models
{
    public class Classes
    {
        //tabele!!!
        #region ORM Classes
        [Table("Autorzy")]
        public class Autor
        {
            [Key]
            [Column("ID_Autora")]
            public int ID_Autora { get; set; }

            [Column("Imie")]
            public string Imie { get; set; }

            [Column("Nazwisko")]
            public string Nazwisko { get; set; }

            public virtual List<Ksiazka> Ksiazki { get; set; }
        }

        [Table("Kategorie")]
        public class Kategoria
        {
            [Key]
            [Column("ID_Kategorii")]
            public int ID_Kategorii { get; set; }

            [Column("Nazwa")]
            public string Nazwa { get; set; }

            [Column("Opis")]
            public string Opis { get; set; }

            public virtual List<Ksiazka> Ksiazki { get; set; }
        }

        [Table("Sprzedaze")]
        public class Sprzedaz
        {
            [Key]
            [Column("ID_Sprzedazy")]
            public int ID_Sprzedazy { get; set; }

            [Column("Data_sprzedazy")]
            public DateTime Data_sprzedazy { get; set; }
        }

        [Table("Ksiazki")]
        public class Ksiazka
        {
            [Key]
            [Column("ID_Ksiazki")]
            public int ID_Ksiazki { get; set; }

            [Column("Tytul")]
            public string Tytul { get; set; }

            [Column("Liczba_dostepnych")]
            public int Liczba_dostepnych { get; set; }

            [Column("Cena_dostawa")]
            public double Cena_dostawa { get; set; }

            [Column("Cena_sprzedaz")]
            public double Cena_sprzedaz { get; set; }

            [Column("ISBN")]
            public string ISBN { get; set; }

            [ForeignKey("Autor")]
            [Column("ID_Autora")]
            public int ID_Autora { get; set; }
            public virtual Autor Autor { get; set; }

            [ForeignKey("Kategoria")]
            [Column("ID_Kategorii")]
            public int ID_Kategorii { get; set; }
            public virtual Kategoria Kategoria { get; set; }
        }

        [Table("Dostawy")]
        public class Dostawa
        {
            [Key]
            [Column("ID_Dostawy")]
            public int ID_Dostawy { get; set; }

            [Column("Data_dostawy")]
            public DateTime Data_dostawy { get; set; }

            [Column("Dostawca")]
            public string Dostawca { get; set; }
            //public virtual List<Ksiazka> Ksiazki { get; set; }
        }

        [Table("Sprzedaze_Ksiazki")]
        public class Sprzedaz_Ksiazka
        {
            [Key, Column("ID_Sprzedazy", Order = 1)]
            public int ID_Sprzedazy { get; set; }

            [Key, Column("ID_Ksiazki", Order = 2)]
            public int ID_Ksiazki { get; set; }

            [Column("Liczba")]
            public int Liczba { get; set; }

            public Sprzedaz Sprzedaz { get; set; }
            public Ksiazka Ksiazka { get; set; }
        }

        [Table("Dostawy_Ksiazki")]
        public class Dostawa_Ksiazka
        {
            [Key, Column("ID_Dostawy", Order = 1)]
            public int ID_Dostawy { get; set; }

            [Key, Column("ID_Ksiazki", Order = 2)]
            public int ID_Ksiazki { get; set; }

            [Column("Liczba")]
            public int Liczba { get; set; }

            public Dostawa Dostawa { get; set; }
            public Ksiazka Ksiazka { get; set; }
        }

        [Table("Uprawnienia")]
        public class Uprawnienie
        {
            [Key]
            [Column("ID_Uprawnienia")]
            public int ID_Uprawnienia { get; set; }

            [Column("Instrukcja")]
            public string Instrukcja { get; set; }

            [Column("Nazwa_tabeli")]
            public string Nazwa_tabeli { get; set; }

            public virtual List<Uprawnienie_Rola> Uprawnienie_Rola { get; set; }
        }

        [Table("Rolee")]
        public class Rola
        {
            [Key]
            [Column("ID_Roli")]
            public int ID_Roli { get; set; }

            [Column("Nazwa")]
            public string Nazwa { get; set; }

            public virtual List<Uprawnienie_Rola> Uprawnienie_Rola { get; set; }
            public virtual List<Uzytkownik_Rola> Uzytkownik_Rola { get; set; }
        }

        [Table("Uzytkownicy")]
        public class Uzytkownik
        {
            [Key]
            [Column("ID_Uzytkownika")]
            public int ID_Uzytkownika { get; set; }

            [Column("Login")]
            public string Login { get; set; }

            [Column("Nazwa")]
            public string Nazwa { get; set; }

            [Column("Haslo")]
            public string Haslo { get; set; }

            public virtual List<Uzytkownik_Rola> Uzytkownik_Rola { get; set; }

            //hashowanie!!!! proponuje wziac jakies inne, np sha3

        }

        [Table("Sesje")]
        public class Sesja
        {
            [Key]
            [Column("ID_Sesji")]
            public string ID_Sesji { get; set; }

            [ForeignKey("Uzytkownik")]
            [Column("ID_Uzytkownika")]
            public int ID_Uzytkownika { get; set; }
            //public virtual Uzytkownik Uzytkownik { get; set; }

            [ForeignKey("Rola")]
            [Column("ID_Roli")]
            public int ID_Roli { get; set; }
            //public virtual Rola Rola { get; set; }

            [Column("Data_waznosci")]
            public int Data_waznosci { get; set; }
        }

        [Table("Uzytkownicy_Role")]
        public class Uzytkownik_Rola
        {
            [Key, Column("ID_Uzytkownika", Order = 1)]
            public int ID_Uzytkownika { get; set; }

            [Key, Column("ID_Roli", Order = 2)]
            public int ID_Roli { get; set; }

            public Uzytkownik Uzytkownik { get; set; }
            public Rola Rola { get; set; }
        }

        [Table("Uprawnienia_Role")]
        public class Uprawnienie_Rola
        {
            [Key, Column("ID_Uprawnienia", Order = 1)]
            public int ID_Uprawnienia { get; set; }

            [Key, Column("ID_Roli", Order = 2)]
            public int ID_Roli { get; set; }

            public Uprawnienie Uprawnienie { get; set; }
            public Rola Rola { get; set; }
        }
        #endregion
    }
}