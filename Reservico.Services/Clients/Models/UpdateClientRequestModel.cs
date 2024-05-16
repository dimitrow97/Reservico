using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reservico.Services.Clients.Models
{
    public class UpdateClientRequestModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }

    }
}