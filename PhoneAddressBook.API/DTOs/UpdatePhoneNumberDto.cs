﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class UpdatePhoneNumberDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
    }
}
