﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeBuddyLibrary.Models.MetaFileModels
{
    public class MetaAirportModel
    {
        public string AirportName { get; set; }

        public string Military { get; set; }

        public string AptIdent { get; set; }

        public string Icao { get; set; }

        public string Alnum { get; set; }

        public List<MetaRecordModel> Records { get; set; } = new List<MetaRecordModel>();
    }
}
