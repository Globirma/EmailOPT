﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTP.Models
{
    public class EmailConfirmModel
    {
        public string Email { get; set; }
        //public bool IsConfirmed { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
