using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SimpleUrlShorten.Models.Context;

namespace SimpleUrlShorten.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SimpleUrlShorten.Models.Url", b =>
                {
                    b.Property<int>("UrlId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("GeneratedDate");

                    b.Property<string>("OriginalUrl")
                        .IsRequired();

                    b.Property<string>("ShortUrl")
                        .IsRequired();

                    b.HasKey("UrlId");

                    b.ToTable("Urls");
                });
        }
    }
}
