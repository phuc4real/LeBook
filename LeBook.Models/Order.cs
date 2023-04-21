using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models
{
    public class Order
    {
        [Key]
        [DisplayName("Mã hoá đơn")]
        public int Id { get; set; }

        [DisplayName("Ngày lập")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        [DisplayName("Tổng thành tiền")]
        [DisplayFormat(DataFormatString = "{0:#,###.##đ}")]
        public double OrderTotal { get; set; }

        [DisplayName("Tình trạng đơn hàng")]
        public string? OrderStatus { get; set; }

        [Required]
        [DisplayName("Hình thức giao hàng")]
        public string? ShippingType { get; set; }

        [DisplayName("Thời gian giao hàng dự kiến")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ShippingDate { get; set; }

        [DisplayName("Mã vận chuyển")]
        public string? TrackingNumber { get; set; }

        [DisplayName("Đơn vị vận chuyển")]
        public string? Carrier { get; set; }

        [Required(ErrorMessage = "Chưa chọn phương thức thanh toán")]
        [DisplayName("Phương thức thanh toán")]
        public string? PaymentType { get; set; }

        [DisplayName("Tình trạng thanh toán")]
        public string? PaymentStatus { get; set; }

        [DisplayName("Ngày thanh toán")]
        public DateTime PaymentDate { get; set; }

        [DisplayName("Ngày trả tiền COD")]
        public DateTime PaymentDateCOD { get; set; }

        public string? SesstionId { get; set; }
        [DisplayName("Mã giao dịch")]
        public string? TransactionId { get; set; }

        [Required(ErrorMessage = "Chưa chọn địa chỉ giao hàng")]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        [ValidateNever]
        [JsonIgnore]
        public virtual UserAddress? ShippingAdress { get; set; }

        [DisplayName("Người đặt hàng")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
    }
}
