using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepostory
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon=await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName=@productName", new { ProductName = productName });
            if (coupon == null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon discount)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName,Description,Amount) VALUES(@ProductName,@Description,@Amount)",
                new
                {
                    ProductName = discount.ProductName,
                    Description = discount.Description,
                    Amount = discount.Amount
                });
            if (affected == 0)
            {
                return false;
            }
            return true;
            
        }
        public async Task<bool> UpdateDiscount(Coupon discount)
        {
            await using var connection =new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("UPDATE Coupon SET ProductName=@ProductName,Description=@Description,Amount=@Amount WHERE Id=@Id",
                new
                {
                    ProductName = discount.ProductName,
                    Description = discount.Description,
                    Amount = discount.Amount,
                    Id = discount.Id
                });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName=@ProductName", new
            {
                ProductName = productName
            });
            if(affected == 0)
                { return false; }
            return true;
        }




    }
}
