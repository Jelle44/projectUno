using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Persistence.Data;
internal class VueTS_ASPdotNetContext : DbContext
{
    public DbSet<CardDbDTO> Cards { get; set; }
    public DbSet<DeckDbDTO> Deck { get; set; }
    public DbSet<PlayerDbDTO> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VueTS_ASPdotNet;Integrated Security=True");
    }
}
