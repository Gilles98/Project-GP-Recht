using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    public abstract class Basisklasse
    {
        [NotMapped]
        public virtual string this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                var property = GetType().GetProperty(columnName);
                if (property != null)
                {
                    var validationContext = new ValidationContext(this)
                    {
                        MemberName = columnName
                    };

                    var isValid = Validator.TryValidateProperty(property.GetValue(this), validationContext, validationResults);
                    if (isValid)
                    {
                        return null;
                    }

                    return validationResults.First().ErrorMessage;
                }
                return "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public string Error
        {
            get
            {
                string foutmeldingen = "";
                foreach (var item in this.GetType().GetProperties()) //reflection 
                {
                    string fout = this[item.Name];
                    if (!string.IsNullOrWhiteSpace(fout))
                    {
                        foutmeldingen += fout + Environment.NewLine;
                    }
                }
                return foutmeldingen;
            }
        }


        #region hulpmethodes
        public bool IsGeldig()
        {
            return string.IsNullOrWhiteSpace(Error);
        }
        #endregion
    }
}
