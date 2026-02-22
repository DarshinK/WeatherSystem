using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherService.Domain.Entities;

namespace WeatherService.Infrastructure.Data;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
        : base(options) { }

    public DbSet<WeatherRecord> WeatherRecords => Set<WeatherRecord>();
}

