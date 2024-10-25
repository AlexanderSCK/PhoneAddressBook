﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class CreatePersonDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one address is required.")]
        public ICollection<CreateAddressDto> Addresses { get; set; } = new List<CreateAddressDto>();
    }
}