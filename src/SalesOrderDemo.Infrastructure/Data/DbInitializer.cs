using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesOrderDemo.Domain.Entities;
using SalesOrderDemo.Domain.Enums;
using SalesOrderDemo.Infrastructure.Identity;

namespace SalesOrderDemo.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Apply migrations
        await context.Database.MigrateAsync();

        // Seed roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Seed admin user
        if (!await userManager.Users.AnyAsync())
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@salesorder.com",
                Email = "admin@salesorder.com",
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Seed sample data
        if (!await context.Customers.AnyAsync())
        {
            var customers = new[]
            {
                new Customer
                {
                    Name = "Acme Corporation",
                    Email = "contact@acme.com",
                    Phone = "+1-555-0100",
                    Address = "123 Business St",
                    City = "New York",
                    Country = "USA",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Customer
                {
                    Name = "TechStart Inc",
                    Email = "info@techstart.com",
                    Phone = "+1-555-0200",
                    Address = "456 Tech Ave",
                    City = "San Francisco",
                    Country = "USA",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Customer
                {
                    Name = "Global Traders Ltd",
                    Email = "sales@globaltraders.com",
                    Phone = "+44-20-5550-0300",
                    Address = "789 Commerce Rd",
                    City = "London",
                    Country = "UK",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new[]
            {
                new Product
                {
                    Name = "Laptop Pro 15",
                    Description = "High-performance laptop with 15-inch display",
                    SKU = "LAP-001",
                    Price = 1299.99m,
                    StockQuantity = 50,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Product
                {
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with precision tracking",
                    SKU = "MOU-001",
                    Price = 29.99m,
                    StockQuantity = 200,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Product
                {
                    Name = "Mechanical Keyboard",
                    Description = "RGB mechanical keyboard with Cherry MX switches",
                    SKU = "KEY-001",
                    Price = 149.99m,
                    StockQuantity = 75,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Product
                {
                    Name = "USB-C Adapter",
                    Description = "Multi-port USB-C adapter with HDMI and USB 3.0",
                    SKU = "ADP-001",
                    Price = 49.99m,
                    StockQuantity = 150,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                },
                new Product
                {
                    Name = "24-inch Monitor",
                    Description = "Full HD IPS monitor with adjustable stand",
                    SKU = "MON-001",
                    Price = 249.99m,
                    StockQuantity = 30,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System"
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        if (!await context.SalesOrders.AnyAsync())
        {
            var customers = await context.Customers.ToListAsync();
            var products = await context.Products.ToListAsync();

            if (customers.Any() && products.Any())
            {
                var orders = new[]
                {
                    new SalesOrder
                    {
                        OrderNumber = "SO-2024-001",
                        OrderDate = DateTime.UtcNow.AddDays(-10),
                        CustomerId = customers[0].Id,
                        Status = OrderStatus.Delivered,
                        TotalAmount = 1579.97m,
                        Notes = "Express delivery requested",
                        CreatedDate = DateTime.UtcNow.AddDays(-10),
                        CreatedBy = "System",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductId = products[0].Id,
                                Quantity = 1,
                                UnitPrice = 1299.99m,
                                TotalPrice = 1299.99m,
                                CreatedDate = DateTime.UtcNow.AddDays(-10),
                                CreatedBy = "System"
                            },
                            new OrderItem
                            {
                                ProductId = products[1].Id,
                                Quantity = 2,
                                UnitPrice = 29.99m,
                                TotalPrice = 59.98m,
                                CreatedDate = DateTime.UtcNow.AddDays(-10),
                                CreatedBy = "System"
                            },
                            new OrderItem
                            {
                                ProductId = products[3].Id,
                                Quantity = 1,
                                UnitPrice = 49.99m,
                                TotalPrice = 49.99m,
                                CreatedDate = DateTime.UtcNow.AddDays(-10),
                                CreatedBy = "System"
                            }
                        }
                    },
                    new SalesOrder
                    {
                        OrderNumber = "SO-2024-002",
                        OrderDate = DateTime.UtcNow.AddDays(-5),
                        CustomerId = customers[1].Id,
                        Status = OrderStatus.Processing,
                        TotalAmount = 399.98m,
                        Notes = "Standard shipping",
                        CreatedDate = DateTime.UtcNow.AddDays(-5),
                        CreatedBy = "System",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductId = products[2].Id,
                                Quantity = 1,
                                UnitPrice = 149.99m,
                                TotalPrice = 149.99m,
                                CreatedDate = DateTime.UtcNow.AddDays(-5),
                                CreatedBy = "System"
                            },
                            new OrderItem
                            {
                                ProductId = products[4].Id,
                                Quantity = 1,
                                UnitPrice = 249.99m,
                                TotalPrice = 249.99m,
                                CreatedDate = DateTime.UtcNow.AddDays(-5),
                                CreatedBy = "System"
                            }
                        }
                    },
                    new SalesOrder
                    {
                        OrderNumber = "SO-2024-003",
                        OrderDate = DateTime.UtcNow.AddDays(-2),
                        CustomerId = customers[2].Id,
                        Status = OrderStatus.Pending,
                        TotalAmount = 179.97m,
                        Notes = "Gift wrap requested",
                        CreatedDate = DateTime.UtcNow.AddDays(-2),
                        CreatedBy = "System",
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductId = products[1].Id,
                                Quantity = 3,
                                UnitPrice = 29.99m,
                                TotalPrice = 89.97m,
                                CreatedDate = DateTime.UtcNow.AddDays(-2),
                                CreatedBy = "System"
                            },
                            new OrderItem
                            {
                                ProductId = products[3].Id,
                                Quantity = 2,
                                UnitPrice = 49.99m,
                                TotalPrice = 99.98m,
                                CreatedDate = DateTime.UtcNow.AddDays(-2),
                                CreatedBy = "System"
                            }
                        }
                    }
                };

                await context.SalesOrders.AddRangeAsync(orders);
                await context.SaveChangesAsync();
            }
        }
    }
}
