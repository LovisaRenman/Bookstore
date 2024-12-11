using System;
using System.Collections.Generic;
using BookstoreEf.Model;
using Microsoft.EntityFrameworkCore;

namespace BookstoreEf;

public partial class BookstoreContext : DbContext
{
    public BookstoreContext()
    {
    }

    public BookstoreContext(DbContextOptions<BookstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    //public virtual DbSet<BookReview> BookReviews { get; set; }

    public virtual DbSet<BookReview1> BookReviews1 { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TitlesByAuthor> TitlesByAuthors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Initial Catalog=Bookstore;Integrated Security=True;Trust Server Certificate=True;Server SPN=localhost");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Finnish_Swedish_CI_AS");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC276E84DC7B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasDefaultValue("Unknown");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasDefaultValue("Uknown");            

            //entity.HasMany(d => d.BookIsbns).WithMany(p => p.Authors)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "AuthorsBook",
            //        r => r.HasOne<Book>().WithMany()
            //            .HasForeignKey("BookIsbn")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__AuthorsBo__BookI__39E294A9"),
            //        l => l.HasOne<Author>().WithMany()
            //            .HasForeignKey("AuthorId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK__AuthorsBo__Autho__38EE7070"),
            //        j =>
            //        {
            //            j.HasKey("AuthorId", "BookIsbn").HasName("PK__AuthorsB__D33751843AAFE47B");
            //            j.ToTable("AuthorsBooks");
            //            j.IndexerProperty<int>("AuthorId").HasColumnName("AuthorID");
            //            j.IndexerProperty<string>("BookIsbn")
            //                .HasMaxLength(13)
            //                .HasColumnName("BookISBN");
            //        });


        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__Books__447D36EBD4D34FA5");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.BookTitle).HasMaxLength(100);
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Language)
                .HasMaxLength(20)
                .HasDefaultValue("Unknown");
            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Books__GenreID__351DDF8C");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__Publisher__361203C5");
        });

        modelBuilder.Entity<BookReview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Book Reviews");

            entity.Property(e => e.AverageRatingOutOf5).HasColumnName("Average rating (out of 5)");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(100)
                .HasColumnName("Book Title");
            entity.Property(e => e.GenreName)
                .HasMaxLength(20)
                .HasColumnName("Genre Name");
            entity.Property(e => e.StarRatingsOutOf5)
                .HasMaxLength(4000)
                .HasColumnName("Star Ratings (out of 5)");
        });

        modelBuilder.Entity<BookReview1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookRevi__3214EC278AFE0DCD");

            entity.ToTable("BookReviews");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookIsbn)
                .HasMaxLength(13)
                .HasColumnName("BookISBN");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(200)
                .HasDefaultValue("");

            entity.HasOne(d => d.BookIsbnNavigation).WithMany(p => p.BookReview1s)
                .HasForeignKey(d => d.BookIsbn)
                .HasConstraintName("FK__BookRevie__BookI__595B4002");

            entity.HasOne(d => d.Customer).WithMany(p => p.BookReview1s)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookRevie__Custo__5A4F643B");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC2706BE0AC0");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534B504811C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phonenumber).HasMaxLength(13);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC2752A823DF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GenreName).HasMaxLength(20);
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.BookIsbn }).HasName("PK__Inventor__986F5D71F6F4FD49");

            entity.ToTable("Inventory");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.BookIsbn)
                .HasMaxLength(13)
                .HasColumnName("BookISBN");

            entity.HasOne(d => d.BookIsbnNavigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.BookIsbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__BookI__46486B8E");

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK__Inventory__Store__45544755");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC275071CD25");

            entity.HasIndex(e => e.Name, "UQ__Publishe__737584F6A560B7B9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3214EC271C888CB1");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Postcode).HasMaxLength(10);
            entity.Property(e => e.StoreName).HasMaxLength(100);
            entity.Property(e => e.Street).HasMaxLength(100);
            entity.Property(e => e.StreetNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<TitlesByAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Titles By Author");

            entity.Property(e => e.Age)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.Author).HasMaxLength(101);
            entity.Property(e => e.ExemplarsInStore).HasColumnName("Exemplars in Store");
            entity.Property(e => e.InventoryValue)
                .HasMaxLength(44)
                .IsUnicode(false)
                .HasColumnName("Inventory Value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
