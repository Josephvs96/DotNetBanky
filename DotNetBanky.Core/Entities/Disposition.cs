using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.Entities
{
    public partial class Disposition
    {
        public int DispositionId { get; set; }

        [MaxLength(50)]
        public string Type { get; set; } = null!;
        public virtual Account Account { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}
