using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model
{
    public class Haven
    {
		private string _naam;

		public string Naam
		{
			get { return _naam; }
			set 
			{
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomeinException("Haven_naam"); // dit is een custom exception die uitgeschreven staat in 'DomeinExceptions' (using niet vergeten)
                _naam = value;
			}
		}

        public int? Id { get; } // kan nullable (leeg) zijn.

		public Haven(string naam)
		{
			_naam = naam;
		}

		public Haven(string naam, int? id) : this(naam)
		{
			Id = id;
		}
    }
}
