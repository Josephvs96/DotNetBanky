using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.Entities
{
    public class Card
    {
        public int CardId { get; set; }

        [MaxLength(50)]
        public string Type { get; set; } = null!;
        public DateTime Issued { get; set; }

        [MaxLength(50)]
        public string Cctype { get; set; } = null!;

        [MaxLength(50)]
        public string Ccnumber { get; set; } = null!;

        [MaxLength(10)]
        public string Cvv2 { get; set; } = null!;
        public int ExpM { get; set; }
        public int ExpY { get; set; }

        public Disposition Disposition { get; set; } = null!;
    }
}
