using _8bitstore_be.Libraries;
using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.DTO.Payment;
using _8bitstore_be.Data;
using _8bitstore_be.Models;

namespace _8bitstore_be.Services
{
    public class VnPayService: IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly _8bitstoreContext _context;

        public VnPayService(IConfiguration configuration, _8bitstoreContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string CreatePaymentUrl(HttpContext context)
        {
            var vnpay = new VnPay();

            vnpay.AddRequestData("vnp_Amount", "123500000");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", HashAndGetIP.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["Vnpay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", Guid.NewGuid().ToString());
            vnpay.AddRequestData("vnp_Version", VnPay.VERSION);

            return vnpay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
        }

        public async Task savePaymentAsync(VnPayResultDto result, string userId)
        {
            PaymentVnPay payment = new()
            {
                UserId = userId,
                TransactionNo = result.TransactionNo,
                BankCode = result.BankCode,
                BankTranNo = result.BankTranNo,
                CardType = result.CardType,
                PayDate = result.PayDate,
                ResponseCode = result.ResponseCode,
                PaymentType = "vnpay",
                Id = Guid.NewGuid().ToString(),
            };

            await _context.PaymentVnPays.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

    }
}
