using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Coordenation
{
    public class CreateCoordenationDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "O nome da coordenação não pode ser inferior a 3 caracteres")]
        public string Name { get; set; } = string.Empty;
    }
}