using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Models.Results
{
    public class SetCoordinatorResult
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}