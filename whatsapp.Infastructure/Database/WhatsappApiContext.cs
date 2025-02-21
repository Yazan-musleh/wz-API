using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using whatsapp.Domain.Entities;

namespace whatsapp.Infastructure.Database;

public partial class WhatsappApiContext : DbContext
{
    public WhatsappApiContext()
    {
    }

    public WhatsappApiContext(DbContextOptions<WhatsappApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LocationMessage> LocationMessages { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

    public virtual DbSet<TextMessage> TextMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=YZN\\SQLEXPRESS;Initial Catalog=whatsappApi;User ID=sa;Password=yazan.123;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__location__3213E83FE2074920");

            entity.ToTable("location_messages");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("longitude");

            entity.HasOne(d => d.Message).WithOne(p => p.LocationMessage)
                .HasForeignKey<LocationMessage>(d => d.Id)
                .HasConstraintName("FK__location_mes__id__5070F446");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__messages__3213E83FCBF1AEBD");

            entity.ToTable("messages");

            entity.HasIndex(e => e.WaMessageId, "UQ__messages__FD85572C7778A804").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LocalTime)
                .HasColumnType("datetime2")
                .HasColumnName("local_time");
            entity.Property(e => e.MessageType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("message_type");
            entity.Property(e => e.Receiver)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("receiver");
            entity.Property(e => e.Sender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sender");
            entity.Property(e => e.WaMessageId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("wa_message_id");
        });

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.ToTable("phoneNumbers");

            entity.Property(e => e.PhoneNumber1).HasColumnName("PhoneNumber");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<TextMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__text_mes__3213E83F59584CE1");

            entity.ToTable("text_messages");

            entity.Property(e => e.Id)
                .HasColumnType("long")
                .HasColumnName("id");
            entity.Property(e => e.MessageBody)
                .HasColumnType("text")
                .HasColumnName("message_body");

            entity.HasOne(d => d.Message).WithOne(p => p.TextMessage)
                .HasForeignKey<TextMessage>(d => d.Id)
                .HasConstraintName("FK__text_message__id__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
