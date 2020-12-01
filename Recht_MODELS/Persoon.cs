using Project_Recht.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recht_MODELS
{
    public class Persoon : Basis
    {
        private string _voornaam;
        private string _achternaam;
        private string _gemeente;
        private string _straat;
        private string _huisnummer;
        private int _postcode;
        public override string this[string columnName] {


            get
            {
                if (columnName == "Postcode" && Postcode == 0)
                {
                    return "* Geef de postcode in";
                }
                if (columnName == "Postcode" && Postcode < 1000 || Postcode > 9999)
                {
                    return "* Een postcode ligt tussen 1000 en 9999";
                }

                var lijst = this.GetType().GetProperties().ToList();

                for (int i = 0; i <= lijst.Count - 1; i++)
                {
                    if (lijst[i].Name == "Item")
                    {
                        lijst.Remove(lijst[i]);
                    }
                    if (lijst[i].Name == columnName && lijst[i].GetValue(this, null) == null || (string)lijst[i].GetValue(this, null) == "")
                    {
                        return "* Verplicht veld";
                    }

                }
                return "";
            }
        }

        public string Voornaam
        {
            get
            {
                return _voornaam;
            }
            set
            {
                _voornaam = value;
                NotifyPropertyChanged();
            }
        }

        public string Achternaam
        {
            get
            {
                return _achternaam;
            }
            set
            {
                _achternaam = value;
                NotifyPropertyChanged();
            }
        }

        public string Gemeente
        {
            get
            {
                return _gemeente;
            }
            set
            {
                _gemeente = value;
                NotifyPropertyChanged();
            }
        }

        public string Straat
        {
            get
            {
                return _straat;
            }
            set
            {
                _straat = value;
                NotifyPropertyChanged();
            }
        }

        public string Huisnummer
        {
            get
            {
                return _huisnummer;
            }
            set
            {
                _huisnummer = value;
                NotifyPropertyChanged();
            }
        }

        public int Postcode
        {
            get
            {
                return _postcode;
            }
            set
            {
                _postcode = value;
                NotifyPropertyChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
