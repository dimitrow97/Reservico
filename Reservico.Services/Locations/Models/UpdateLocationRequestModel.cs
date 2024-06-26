﻿using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Locations.Models
{
    public class UpdateLocationRequestModel
    {
        public Guid LocationId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        public string Country { get; set; }

        public IList<Guid> Categories { get; set; }
    }
}