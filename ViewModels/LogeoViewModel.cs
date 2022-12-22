using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using tl2_tp4_2022_loboser.Models;

#nullable disable

namespace tl2_tp4_2022_loboser.ViewModels
{
    public class LogeoViewModel
    {
        [Required]
        [StringLength(100)]
        public string User { get; set; }

        [Required]
        [StringLength(100)]
        public string Pass { get; set; }
    }
}