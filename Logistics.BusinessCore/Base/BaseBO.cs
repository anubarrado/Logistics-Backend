using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.BusinessCore.Base
{
    public class BaseBO
    {
        public Dictionary<string, List<string>> Errors;

        public BaseBO()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string propiedad, string errorInfo)
        {
            if (!Errors.ContainsKey(propiedad))
            {
                Errors[propiedad] = new List<string>();
            }
            Errors[propiedad].Add(errorInfo);
        }
        public void RemoveError(string propiedad)
        {
            if (Errors.ContainsKey(propiedad))
            {
                var error = Errors[propiedad].SingleOrDefault();
                if (error != null)
                {
                    Errors[propiedad].Remove(error);
                    if (!Errors[propiedad].Any())
                    {
                        Errors.Remove(propiedad);
                    }
                }
            }
        }
        public bool HasErrors
        {
            get { return Errors.Count > 0; }
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return Errors[propertyName];
        }
        public bool Validation<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            var pasaValidacion = Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
            return pasaValidacion;
        }

    }
}
