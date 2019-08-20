using Microsoft.EntityFrameworkCore;
using WikiGameBot.Data.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.IO;
using System;
using WikiGameBot.Data.Loaders.Interfaces;
using WikiGameBot.Data.Loaders;

namespace WikiGameBot.Data
{
    public class WikiBotDataDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AWSRDSInfoLoader serverInfoLoader = new AWSRDSInfoLoader();
            optionsBuilder.UseSqlServer(serverInfoLoader.GetConnectionString());
        }

    }

}
