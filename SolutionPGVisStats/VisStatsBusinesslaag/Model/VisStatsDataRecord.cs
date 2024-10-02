﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model
{
    public class VisStatsDataRecord
    {

        // property's

		private int _jaar;
		public int Jaar
		{
			get { return _jaar; }
			set 
			{ 
				if ((value < 2000) || (value > 2100)) throw new DomeinException("Jaar niet correct"); 
				_jaar = value;
			}
		}

        private int _maand;
        public int Maand
        {
            get { return _maand; }
            set
            {
                if ((value < 1) || (value > 12)) throw new DomeinException("Maand niet correct");
                _maand = value;
            }
        }

        private double _gewicht;
		public double Gewicht
		{
			get { return _gewicht; }
			set 
			{ 
				if (value < 0.0) throw new DomeinException("Gewicht < 0"); 
				_gewicht = value;
			}
		}

        private double _waarde;
        public double Waarde
        {
            get { return _waarde; }
            set
            {
                if (value < 0.0) throw new DomeinException("Waarde < 0");
                _waarde = value;
            }
        }

        // property's in OO programeren -> Niet op basis van ID, steeds op basis van het object zelf!
        private Vissoort _soort;
        public Vissoort Soort
        {
            get { return _soort; }
            set 
            {   if (value == null) throw new DomeinException("Soort is null.");

               _soort = value;
            }
        }

        //TODO Haven
        private Haven _haven;
        public Haven Haven
        {
            get { return _haven; }
            set
            {
                if (value == null) throw new DomeinException("Haven is null");

                _haven = value;
            }
        }

    }
}
