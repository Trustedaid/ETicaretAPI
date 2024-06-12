﻿using ETicaretAPI.Domain;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using File = ETicaretAPI.Domain.Entities.File;

namespace ETicaretAPI.Persistence.Contexts;

public class ETicaretAPIDbContext : DbContext
{
    public ETicaretAPIDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ProductImageFile> ProductImageFiles { get; set; }
    public DbSet<InvoiceFile> InvoiceFiles { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // ChangeTracker : Entityler üzerinden yapılan değişiklikleri takip eder ya da
        // yeni eklenen verinin yakalanmasını sağlayan property. Update operasyonlarında
        // Track edilen verileri yakalayıp elde etmeyi sağlar.
        
        var datas = ChangeTracker.Entries<BaseEntity>();
        foreach (var data in datas)
        {
            _ = data.State switch
            {
                EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                _ => DateTime.UtcNow
            };
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}