using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.Model.model;

namespace Sanasoppa.Model.context;

public partial class SanasoppaContext : DbContext
{
    public SanasoppaContext(DbContextOptions<SanasoppaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GameSession> GameSessions { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

    public virtual DbSet<Submission> Submissions { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_session_pkey");

            entity.ToTable("game_session", "sanasoppa");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_pkey");

            entity.ToTable("player", "sanasoppa");

            entity.HasIndex(e => new { e.Name, e.GameSessionId }, "player_name_game_session_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.GameSessionId).HasColumnName("game_session_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.GameSession).WithMany(p => p.Players)
                .HasForeignKey(d => d.GameSessionId)
                .HasConstraintName("player_game_session_id_fkey");
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("round_pkey");

            entity.ToTable("round", "sanasoppa");

            entity.HasIndex(e => new { e.GameSessionId, e.RoundNumber }, "round_game_session_id_round_number_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.GameSessionId).HasColumnName("game_session_id");
            entity.Property(e => e.LeaderId).HasColumnName("leader_id");
            entity.Property(e => e.RoundNumber).HasColumnName("round_number");

            entity.HasOne(d => d.GameSession).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.GameSessionId)
                .HasConstraintName("round_game_session_id_fkey");

            entity.HasOne(d => d.Leader).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.LeaderId)
                .HasConstraintName("round_leader_id_fkey");
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("score_pkey");

            entity.ToTable("score", "sanasoppa");

            entity.HasIndex(e => new { e.RoundId, e.PlayerId }, "score_round_id_player_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.RoundId).HasColumnName("round_id");

            entity.HasOne(d => d.Player).WithMany(p => p.Scores)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("score_player_id_fkey");

            entity.HasOne(d => d.Round).WithMany(p => p.Scores)
                .HasForeignKey(d => d.RoundId)
                .HasConstraintName("score_round_id_fkey");
        });

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("submission_pkey");

            entity.ToTable("submission", "sanasoppa");

            entity.HasIndex(e => new { e.RoundId, e.PlayerId }, "submission_round_id_player_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Guess).HasColumnName("guess");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.RoundId).HasColumnName("round_id");

            entity.HasOne(d => d.Player).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("submission_player_id_fkey");

            entity.HasOne(d => d.Round).WithMany(p => p.Submissions)
                .HasForeignKey(d => d.RoundId)
                .HasConstraintName("submission_round_id_fkey");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vote_pkey");

            entity.ToTable("vote", "sanasoppa");

            entity.HasIndex(e => new { e.RoundId, e.VoterId }, "vote_round_id_voter_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.RoundId).HasColumnName("round_id");
            entity.Property(e => e.SubmissionId).HasColumnName("submission_id");
            entity.Property(e => e.VoterId).HasColumnName("voter_id");

            entity.HasOne(d => d.Round).WithMany(p => p.Votes)
                .HasForeignKey(d => d.RoundId)
                .HasConstraintName("vote_round_id_fkey");

            entity.HasOne(d => d.Submission).WithMany(p => p.Votes)
                .HasForeignKey(d => d.SubmissionId)
                .HasConstraintName("vote_submission_id_fkey");

            entity.HasOne(d => d.Voter).WithMany(p => p.Votes)
                .HasForeignKey(d => d.VoterId)
                .HasConstraintName("vote_voter_id_fkey");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("word_pkey");

            entity.ToTable("word", "sanasoppa");

            entity.HasIndex(e => new { e.RoundId, e.Word1 }, "word_round_id_word_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.RoundId).HasColumnName("round_id");
            entity.Property(e => e.Word1).HasColumnName("word");

            entity.HasOne(d => d.Round).WithMany(p => p.Words)
                .HasForeignKey(d => d.RoundId)
                .HasConstraintName("word_round_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
