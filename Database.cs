﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartWithBackend.Data;
using ShoppingCartWithBackend.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartWithBackend
{
    public class Database
    {
        public static void InitialiseDatabase(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<StoreContext>())
            {
                context.Database.Migrate();

                if (context.Carts.Count() == 0)
                {
                    var cart = new ShoppingCart
                    {
                        LineItems = new List<LineItem>
                        {
                            new LineItem{ Image = "test-image.jpg", Name="Big T-shirt", Price=39.50m, Quantity = 2 },
                            new LineItem{ Image = "test-image.jpg", Name="Small White T-shirt", Price=19.50m, Quantity = 1 },
                            new LineItem{ Image = "test-image.jpg", Name="Smart Speaker", Price=23.00m, Quantity = 1 },
                        }
                    };
                    context.Carts.Add(cart);
                    context.SaveChanges();
                }
            }
        }
    }
}
