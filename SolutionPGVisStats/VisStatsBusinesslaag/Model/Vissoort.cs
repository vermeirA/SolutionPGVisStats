using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model
{
    public class Vissoort
    {
        private string _naam;

        public Vissoort(string naam)
        {
            Naam = naam;
        }

        public Vissoort(string naam, int id) : this(naam)
        {
            Id = id;
        }

        public string Naam { // mag niet leeg zijn 
            get { return _naam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomeinException("Vissoort_naam"); // dit is een custom exception die uitgeschreven staat in 'DomeinExceptions' (using niet vergeten)
                _naam = value;
            } 
        }                   
        
        public int? Id { get; } // ? achter type betekent dat het een 'nullable' is. (kan zijn dat het leeg is)

	}
}
