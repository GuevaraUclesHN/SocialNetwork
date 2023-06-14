using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Infrastructure.EntityFramework.DatabaseConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder
            .HasKey(x => x.Id);

            modelBuilder
                .Property(x => x.Id)
            .ValueGeneratedOnAdd();

            modelBuilder
                .Property(x => x.Username)
            .IsRequired();

            modelBuilder
               .Property(x => x.Name)
            .IsRequired();

            modelBuilder
              .Property(x => x.Email)
              .IsRequired();

            modelBuilder
               .HasMany(x => x.Posts)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            modelBuilder
                .HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}

