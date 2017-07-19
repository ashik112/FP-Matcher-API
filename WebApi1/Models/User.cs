﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi1.Models
{
    public class User
    {
        public string id { get; set; }
        public byte[] profile { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public byte[] finger { get; set; }
        public byte[] template { get; set; }
    }
}