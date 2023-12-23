using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.Entities;

public partial class Submission
{
    public Guid Id { get; set; }

    public Guid RoundId { get; set; }

    public Guid PlayerId { get; set; }

    public string Guess { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;

    public virtual Round Round { get; set; } = null!;

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
