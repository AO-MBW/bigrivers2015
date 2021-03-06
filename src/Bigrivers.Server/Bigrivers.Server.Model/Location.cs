﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bigrivers.Server.Model
{
    public class Location
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public string Stagename { get; set; }

        public string EditedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Edited { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public virtual List<Event> Events { get; set; }
    }
}