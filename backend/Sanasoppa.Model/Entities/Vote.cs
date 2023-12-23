using System;
using System.Collections.Generic;

namespace Sanasoppa.Model.Entities;

public partial class Vote
{
    public Guid Id { get; set; }

    public Guid RoundId { get; set; }

    public Guid VoterId { get; set; }

    public Guid SubmissionId { get; set; }

    public virtual Round Round { get; set; } = null!;

    public virtual Submission Submission { get; set; } = null!;

    public virtual Player Voter { get; set; } = null!;
}
