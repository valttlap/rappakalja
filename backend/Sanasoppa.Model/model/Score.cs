using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.model;

public partial class Score
{
    public Guid Id { get; set; }

    public Guid RoundId { get; set; }

    public Guid PlayerId { get; set; }

    public int Points { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Round Round { get; set; } = null!;
}
