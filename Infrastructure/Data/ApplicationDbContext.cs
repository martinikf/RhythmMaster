using System.Reflection;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    //public DbSet<Album> Albums { get; set; }
    //public DbSet<Block> Blocks { get; set; }
    public DbSet<Dance> Dances { get; set; }
    //public DbSet<Event> Events { get; set; }
    //public DbSet<EventBlock> EventBlocks { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistSong> PlaylistSongs { get; set; }
    public DbSet<Song> Songs { get; set; }
    //public DbSet<RemixSong> Remixes { get; set; }
    
    public DbSet<Person> Persons { get; set; }
    public DbSet<AppUser> DJs { get; set; }
    public DbSet<Interpret> Interprets { get; set; }

    public DbSet<UserSong> UserSongs { get; set; }
    public DbSet<SongDance> SongDances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Song>().UseTptMappingStrategy();
        builder.Entity<Person>().UseTptMappingStrategy();
    }
    
    /*
     *dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API --output-dir Data\Migrations
     *  
     */
    
}