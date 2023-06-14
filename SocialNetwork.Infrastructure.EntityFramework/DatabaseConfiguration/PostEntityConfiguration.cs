using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Infrastructure.EntityFramework.DatabaseConfiguration
{
    public class PostEntityConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> modelBuilder)
        {
            modelBuilder
            .HasKey(x => x.Id);

            modelBuilder
                .Property(x => x.Id)
            .ValueGeneratedOnAdd();

            modelBuilder
                .Property(x => x.Content)
            .IsRequired();

            modelBuilder
                .HasMany(x => x.Comments)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId);
        }
    }
}

