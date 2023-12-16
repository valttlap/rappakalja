using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.Entities;

public partial class Word
{
    public Guid Id { get; set; }

    public Guid RoundId { get; set; }

    public string Word1 { get; set; } = null!;

    public virtual Round Round { get; set; } = null!;
}
