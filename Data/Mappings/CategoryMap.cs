using ApiBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiBlog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //Tabela 
            builder.ToTable("Category");

            // Chave Primaria
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

            // Property
            builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80);

            builder.Property(x => x.Slug)
            .IsRequired()
            .HasColumnName("Slug")
            .HasColumnType("VARCHAR")
            .HasMaxLength(80);

            // INdices
            builder
            .HasIndex(x => x.Slug, "IX_Category_Slug")
            .IsUnique();
        }
    }
}