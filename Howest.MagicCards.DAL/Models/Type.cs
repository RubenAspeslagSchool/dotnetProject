using System;
using System.Collections.Generic;

namespace Howest.MagicCards.DAL.Models;

public partial class Type
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Type1 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CardType> CardTypes { get; set; } = new List<CardType>();
}
