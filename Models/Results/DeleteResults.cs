using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Models.Results
{
    public class DeleteResults
    {
        public bool DeleteSucceded { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}