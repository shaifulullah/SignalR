﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.ViewModel
{
    public class ViewModelUserLogin
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string ErrorMessage { get; set; }
    }
}
