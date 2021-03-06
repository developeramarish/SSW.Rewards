﻿using Microsoft.EntityFrameworkCore;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using SSW.Rewards.Application;

namespace SSW.Rewards.Persistence
{
    public class SSWRewardsDbContext : DbContext, ISSWRewardsDbContext
    {
		public interface ISecrets
		{
            string SqlConnectionString { get; }
        }

		private readonly ISecrets _secrets;
		private readonly IDateTimeProvider _dateTimeProvider;

		public SSWRewardsDbContext(ISecrets secrets, IDateTimeProvider dateTimeProvider)
		{
			_secrets = secrets;
			_dateTimeProvider = dateTimeProvider;
		}

        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<StaffMemberSkill> StaffMemberSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserReward> UserRewards { get; set; }
        public DbSet<Reward> Rewards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(_secrets.SqlConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SSWRewardsDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach(var entry in ChangeTracker.Entries<Entity>())
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedUtc = _dateTimeProvider.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
