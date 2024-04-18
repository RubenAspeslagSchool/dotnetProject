using System;
using System.Collections.Generic;

namespace Howest.MagicCards.DAL.Models;

public partial class Migration
{
    public int Id { get; set; }

    public string Migration1 { get; set; }

    public int Batch { get; set; }
}
