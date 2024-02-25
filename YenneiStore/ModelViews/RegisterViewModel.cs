using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YenneiStore.ModelViews
{
    public class RegisterViewModel
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        public string FullName { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string Email { get; set; }

        [MaxLength(11)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string Phone { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        public string Password { get; set; }

        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tối thiểu 5 ký tự")]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }



        [NotMapped] // Không được ánh xạ vào cơ sở dữ liệu
        [Display(Name = "Age")]
        public int Age
        {
            get
            {
                DateTime now = DateTime.Now;
                int age = now.Year - Birthday.Year;
                if (now < Birthday.AddYears(age))
                {
                    age--;
                }
                return age;
            }
        }


        [Column("Birthday")]
        [Display(Name = "Birth day")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        [CustomValidation(typeof(RegisterViewModel), "ValidateAge")] // Gọi phương thức ValidateAge của Employee để kiểm tra tuổi
        public DateTime Birthday { get; set; }
        public static ValidationResult ValidateAge(DateTime birthday, ValidationContext validationContext)
        {
            int age = DateTime.Now.Year - birthday.Year;
            if (DateTime.Now < birthday.AddYears(age))
            {
                age--;
            }
            if (age < 18)
            {
                return new ValidationResult("Khách hàng chưa đủ 18 tuổi.");
            }
            return ValidationResult.Success;
        }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }
    }
}
