using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.Entities;

public partial class Player
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid GameSessionId { get; set; }

    public string? ConnectionId { get; set; }

    public string PlayerId { get; set; } = null!;

    public virtual GameSession GameSession { get; set; } = null!;

    public virtual ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
