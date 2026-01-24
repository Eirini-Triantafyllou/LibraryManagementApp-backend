using LibraryManagementApp.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Data
{
    public class LibraryAppDbContext : DbContext
    {
        public LibraryAppDbContext()
        {
        }

        public LibraryAppDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Reader> Readers { get; set; } = null!;
        public DbSet<Librarian> Librarians { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Wishlist> Wishlists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
             {
                 entity.ToTable("Users");

                 entity.HasKey(e => e.Id);
                 entity.Property(e => e.Username).HasMaxLength(50);
                 entity.Property(e => e.Email).HasMaxLength(100);
                 entity.Property(e => e.Password).HasMaxLength(100);
                 entity.Property(e => e.Firstname).HasMaxLength(255);
                 entity.Property(e => e.Lastname).HasMaxLength(255);
                 entity.Property(e=> e.UserRole).HasMaxLength(20).HasConversion<string>();
                 entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                 entity.Property(e => e.Address).HasMaxLength(200);

                 entity.Property(e=> e.InsertedAt)
                     .HasDefaultValueSql("GETUTCDATE()")
                     .ValueGeneratedOnAdd();

                 entity.Property(e => e.ModifiedAt)
                     .HasDefaultValueSql("GETUTCDATE()")
                     .ValueGeneratedOnAddOrUpdate();

                 entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
                 entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
             });

            modelBuilder.Entity<Reader>(entity =>
            {
                entity.ToTable("Readers");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.MembershipDate);
                entity.Property(e => e.IsActive);
                entity.Property(e => e.BooksBorrowedCount);

                entity.Property(e=> e.InsertedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => e.Email, "IX_Readers_Email").IsUnique();
                entity.HasIndex(e => e.UserId, "IX_Readers_UserId").IsUnique();
            });

            modelBuilder.Entity<Librarian>(entity =>
            {
                entity.ToTable("Librarians");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.HireDate);
                entity.Property(e => e.IsActive);

                entity.Property(e=> e.InsertedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => e.Email, "IX_Librarians_Email").IsUnique();
                entity.HasIndex(e => e.UserId, "IX_Librarians_UserId").IsUnique();
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Authors");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.AuthorFullName).HasMaxLength(255);
                entity.Property(e => e.Biography).HasMaxLength(2000)
                   .IsRequired(false);
                entity.Property(e => e.DateOfBirth)
                   .IsRequired(false);

                entity.Property(e=> e.InsertedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.ISBN).HasMaxLength(13);
                entity.Property(e => e.PublishedDate);
                entity.Property(e => e.CopiesAvailable);

                entity.Property(e => e.InsertedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasIndex(e => e.ISBN, "IX_Books_ISBN").IsUnique();

                //entity.HasOne(d => d.Author)
                //      .WithMany(p => p.Books)
                //      .HasForeignKey(d => d.AuthorId)
                //      .HasConstraintName("FK_Books_Authors");

                entity.HasMany(d => d.Readers).WithMany(p => p.Books)
                  .UsingEntity("ReadersBooks");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("Wishlists");

                entity.HasKey(e => e.Id);

                entity.HasOne(w => w.User)
                    .WithMany(u => u.Wishlists)
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(w => w.Book)
                    .WithMany(b => b.Wishlists)
                    .HasForeignKey(w => w.BookId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(w => new { w.UserId, w.BookId })
                    .IsUnique();
                entity.Property(e => e.AddedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.InsertedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.ModifiedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAddOrUpdate();
            });

        }

    }
}
