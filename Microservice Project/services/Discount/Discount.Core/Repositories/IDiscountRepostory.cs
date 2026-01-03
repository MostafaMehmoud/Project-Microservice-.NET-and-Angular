using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discount.Core.Entities;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepostory

    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon discount);
        Task<bool> UpdateDiscount(Coupon discount);
        Task<bool> DeleteDiscount(string productName);
    }
}
