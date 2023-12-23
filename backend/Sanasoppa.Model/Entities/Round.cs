using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.Entities;

public partial class Round
{
    public Guid Id { get; set; }

    public Guid GameSessionId { get; set; }

    public int RoundNumber { get; set; }

    public Guid LeaderId { get; set; }

    public string? Word { get; set; }

    public virtual GameSession GameSession { get; set; } = null!;

    public virtual Player Leader { get; set; } = null!;

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
