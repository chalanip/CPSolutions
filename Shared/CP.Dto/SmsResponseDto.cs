﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Dto
{
    public class SmsResponseDto
    {
        public string statusCode { get; set; }
        public string statusDetail { get; set; }
        public string requestId { get; set; }
        public string version { get; set; }
    }
}
