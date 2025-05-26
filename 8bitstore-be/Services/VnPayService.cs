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

        public string CreatePaymentUrl(HttpContext context, string amount)
        {
            var vnpay = new VnPay();
            vnpay.AddRequestData("vnp_Amount", amount);
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

        public async Task<StatusResponse<string>> savePaymentAsync(VnPayResultDto result, string userId)
        {
            var vnPay = new VnPay();
            vnPay.AddResponseData("vnp_TmnCode", result.TmnCode);
            vnPay.AddResponseData("vnp_Amount", result.Amount);
            vnPay.AddResponseData("vnp_BankCode", result.BankCode);
            vnPay.AddResponseData("vnp_BankTranNo", result.BankTranNo);
            vnPay.AddResponseData("vnp_CardType", result.CardType);
            vnPay.AddResponseData("vnp_PayDate", result.PayDate);
            vnPay.AddResponseData("vnp_OrderInfo", result.OrderInfo);
            vnPay.AddResponseData("vnp_TransactionNo", result.TransactionNo);
            vnPay.AddResponseData("vnp_ResponseCode", result.ResponseCode);
            vnPay.AddResponseData("vnp_TransactionStatus", result.TransactionStatus);
            vnPay.AddResponseData("vnp_TxnRef", result.TxnRef);
            vnPay.AddResponseData("vnp_SecureHashType", "SHA512");

            bool validateResult = vnPay.ValidateSignature(result.SecureHash, _configuration["Vnpay:HashSecret"]);

            if (!validateResult) 
            {
                return new StatusResponse<string>
                {
                    Status = "ERROR",
                    Message = "Invalid secure hash"
                };
            }

            if (!DateTime.TryParseExact(result.PayDate, "yyyyMMddHHmmss",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out DateTime payDate))
            {
                throw new ArgumentException("Invalid PayDate format. Expected yyyyMMddHHmmss.");
            }
            payDate = DateTime.SpecifyKind(payDate, DateTimeKind.Utc);
            PaymentVnPay payment = new()
            {
                UserId = userId,
                TransactionNo = result.TransactionNo,
                BankCode = result.BankCode,
                BankTranNo = result.BankTranNo,
                CardType = result.CardType,
                PayDate = payDate,
                ResponseCode = result.ResponseCode,
                PaymentType = "vnpay",
                Status = "completed",
                OrderId = result.OrderId,
                Id = Guid.NewGuid().ToString(),
            };

            await _context.PaymentVnPays.AddAsync(payment);
            await _context.SaveChangesAsync();

            return new StatusResponse<string>
            {
                Status = "SUCCESS",
                Message = result.TransactionStatus
            };
        }
    }
}
