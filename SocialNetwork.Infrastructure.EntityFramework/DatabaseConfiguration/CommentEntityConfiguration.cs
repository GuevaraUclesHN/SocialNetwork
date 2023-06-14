using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Infrastructure.EntityFramework.DatabaseConfiguration
{
	public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
	{
        public void Configure(EntityTypeBuilder<Comment> modelBuilder)
        {
            modelBuilder
            .HasKey(x => x.Id);

            modelBuilder
                .Property(x => x.Id)
            .ValueGeneratedOnAdd();

            modelBuilder
                .Property(x => x.Content)
                .IsRequired();
        }
    }
}

