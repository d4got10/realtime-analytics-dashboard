﻿using DataAggregation.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAggregation.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Event> Events { get; set; }
}