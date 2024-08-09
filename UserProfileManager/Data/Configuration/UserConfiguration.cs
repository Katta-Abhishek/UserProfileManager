using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfileManager.Models;

namespace UserProfileManager.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{

  //for sql server

  //public void Configure(EntityTypeBuilder<User> builder)
  //{
  //  builder.HasKey(u => u.Id);

  //  builder.Property(u => u.UUID)
  //      .IsRequired();

  //  builder.Property(u => u.UniqueName)
  //      .IsRequired()
  //      .HasMaxLength(256);

  //  builder.HasIndex(u => u.UniqueName)
  //      .IsUnique();

  //  builder.Property(u => u.Password)
  //      .IsRequired()
  //      .HasMaxLength(256);

  //  builder.Property(u => u.FirstName)
  //      .IsRequired()
  //      .HasMaxLength(64);

  //  builder.Property(u => u.CountryCode)
  //      .IsRequired()
  //      .HasMaxLength(5);

  //  builder.Property(u => u.PhoneNumber)
  //      .IsRequired()
  //      .HasMaxLength(15);

  //  builder.Property(u => u.Gender)
  //      .IsRequired()
  //      .HasMaxLength(1)
  //      .IsFixedLength();

  //  builder.Property(u => u.Email)
  //      .IsRequired()
  //      .HasMaxLength(256);

  //  builder.HasIndex(u => u.Email)
  //      .IsUnique();

  //  builder.Property(u => u.DateOfBirth)
  //      .IsRequired();

  //  builder.Property(u => u.CreatedOn)
  //      .HasDefaultValueSql("getutcdate()");

  //  builder.Property(u => u.UpdatedOn)
  //      .HasDefaultValueSql("getutcdate()");

  //  builder.Property(u => u.MiddleName)
  //      .HasMaxLength(64);

  //  builder.Property(u => u.LastName)
  //      .HasMaxLength(64);

  //  builder.Property(u => u.Education)
  //      .HasMaxLength(128);

  //  builder.Property(u => u.Address)
  //      .HasMaxLength(256);

  //  builder.Property(u => u.Photo)
  //      .HasColumnType("varbinary(max)");

  //  builder.Property(u => u.City)
  //      .HasMaxLength(128);
  //}

  // for the sqlite
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasKey(u => u.Id);

    builder.Property(u => u.UUID)
        .IsRequired();

    builder.Property(u => u.UniqueName)
        .IsRequired()
        .HasMaxLength(256);

    builder.HasIndex(u => u.UniqueName)
        .IsUnique();

    builder.Property(u => u.Password)
        .IsRequired()
        .HasMaxLength(256);

    builder.Property(u => u.FirstName)
        .IsRequired()
        .HasMaxLength(64);

    builder.Property(u => u.CountryCode)
        .IsRequired()
        .HasMaxLength(5);

    builder.Property(u => u.PhoneNumber)
        .IsRequired()
        .HasMaxLength(15);

    builder.Property(u => u.Gender)
        .IsRequired()
        .HasMaxLength(1)
        .IsFixedLength();

    builder.Property(u => u.Email)
        .IsRequired()
        .HasMaxLength(256);

    builder.HasIndex(u => u.Email)
        .IsUnique();

    builder.Property(u => u.DateOfBirth)
        .IsRequired();

    builder.Property(u => u.CreatedOn)
        .HasDefaultValueSql("CURRENT_TIMESTAMP");

    builder.Property(u => u.UpdatedOn)
        .HasDefaultValueSql("CURRENT_TIMESTAMP");

    builder.Property(u => u.MiddleName)
        .HasMaxLength(64);

    builder.Property(u => u.LastName)
        .HasMaxLength(64);

    builder.Property(u => u.Education)
        .HasMaxLength(128);

    builder.Property(u => u.Address)
        .HasMaxLength(256);

    builder.Property(u => u.Photo)
        .HasColumnType("BLOB");

    builder.Property(u => u.City)
        .HasMaxLength(128);
  }
}
