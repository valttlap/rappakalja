using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.model;

public partial class GameSession
{
    public Guid Id { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<Round> Rounds { get; set; } = new List<Round>();
}
