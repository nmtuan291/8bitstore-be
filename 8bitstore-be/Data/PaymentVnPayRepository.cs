using _8bitstore_be.Interfaces.Repositories;
using _8bitstore_be.Models;

namespace _8bitstore_be.Data
{
    public class PaymentVnPayRepository : Repository<PaymentVnPay>, IPaymentVnPayRepository
    {
        public PaymentVnPayRepository(_8bitstoreContext context) : base(context) { }
    }
} 