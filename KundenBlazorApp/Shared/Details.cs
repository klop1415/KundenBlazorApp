﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KundenBlazorApp.Shared
{
    public class Details
    {
        public Dictionary<string, string[]> errors { get; set; }
        public string? detail { get; set; }
        public int status { get; set; }
        public string? title { get; set; }
        public string traceId { get; set; }
        public string? type { get; set; }
    }
    public class Error
    {
        public string? message { get; set; }
        public string[] exception { get; set; }
    }
}
