using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RaterPrice.Persistence.Domain
{

    [Table("Cities")]
    public class City
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
    }

    [Table("Goods")]
    public class Good
    {
        public Good()
        {
            IsDraft = false;
        }

        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string ShortName { get; set; } // ShortName
        public string Vendor { get; set; } // Vendor
        public string Barcode { get; set; } // Barcode
        public bool IsDraft { get; set; } // IsDraft
    }

    [Table("Images")]
    public class Image
    {
        public int Id { get; set; } // Id (Primary key)
        public string FileName { get; set; } // FileName
        public string FileUrl { get; set; } // FileUrl
        public int ImageTypeId { get; set; } // ImageType
        public bool Main { get; set; } // Main
        public int ObjectId { get; set; } // ObjectId

        [ForeignKey("ImageTypeId")]
        public virtual ImageType ImageType { get; set; } 
    }

    // ImageType
    [Table("ImageTypes")]
    public class ImageType
    {
        public ImageType()
        {
            Images = new List<Image>();
        }

        public int Id { get; set; } 
        public string Name { get; set; }

        public virtual ICollection<Image> Images { get; set; } // Image.FK_dbo.Image_dbo.ImageType_ImageType
    }

    [Table("Prices")]
    public class Price
    {
        public int Id { get; set; } // Id (Primary key)
        public int UserId { get; set; } // UserId
        public int ShopId { get; set; } // ShopId
        public int GoodId { get; set; } // GoodId
        public decimal PriceValue { get; set; } // Price
        public DateTime DateUpdated { get; set; } // DateUpdated

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }

        [ForeignKey("GoodId")]
        public virtual Good Good { get; set; }
    }

    // Shop
    [Table("Shops")]
    public class Shop
    {
        public Shop()
        {
            IsDraft = false;
        }

        public int Id { get; set; } // Id (Primary key)
        public int CityId { get; set; } // CityId
        public string Name { get; set; } // Name
        public string Address { get; set; } // Address
        public string Latitude { get; set; } // Latitude
        public string Longitude { get; set; } // Longitude
        public bool IsDraft { get; set; } // IsDraft
        
        public int GroupId { get; set; }

        public int SubGroupId { get; set; }

        public string Phones { get; set; }

        public string Email { get; set; }

        public string Site { get; set; }

        public string Description { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        [ForeignKey("GroupId")]
        public virtual ShopGroup ShopGroup { get; set; }



        //[ForeignKey("SubGroupId")]
        //public virtual ShopGroup ShopSubGroup { get; set; }
       
    }

    [Table("ShopPaymentTypes")]
    public class ShopPaymentType
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int PaymentTypeId { get; set; }


        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }

        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }

    }

    [Table("ShopWeekdays")]
    public class ShopWeekDay
    {
       public int Id { get; set; }

        public int ShopId { get; set; }

        public int WeekdayId { get; set; }

        public TimeSpan? StartWorkHour { get; set; }

        public TimeSpan? EndWorkHour { get; set; }

        public TimeSpan? DinnerBreakStartHour { get; set; }

        public TimeSpan? DinnerBreakStopHour { get; set; }

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; }

        [ForeignKey("WeekdayId")]
        public virtual Weekday Weekday { get; set; }
    }

    
    [Table("Users")]
    public class User: IdentityUser<int, UserLogin, UserRole, UserClaim>, IUser<int>, IUser
    {
        public DateTime? ConfirmationDate { get; set; }
        public new int Id { get; set; }
        string IUser<string>.Id { get { return Id.ToString(); } }
        public string Login { get; set; } // Login
        public string Password { get; set; } // Password
        public Guid? Token { get; set; } // Token 
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

   
    public class UserLogin : IdentityUserLogin<int> { }

    public class UserRole : IdentityUserRole<int> { }

    public class UserClaim : IdentityUserClaim<int> { }

    public class Role : IdentityRole<int, UserRole> { }

    //public class IdentityRole : IdentityRole<int, IdentityUserRole<int>> { }

    [Table("ConfirmationCodes")]
    public class ConfirmationCode
    {
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreationDate { get; set; }
    }

    [Table("SmsSenders")]
    public class SmsSender
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    [Table("SmsMessages")]
    public class SmsMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    [Table("SmsMessageSends")]
    public class SmsMessageSend
    {
        public decimal? Cost { get; set; }
        public DateTime? SendDate { get; set; }
        public int Id { get; set; }
        public int SmsMessageId { get; set; }
        [ForeignKey("SmsMessageId")]
        public virtual SmsMessage SmsMessage { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int SmsSenderId { get; set; }
        [ForeignKey("SmsSenderId")]
        public virtual SmsSender SmsSender { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public DateTime? CreationDate { get; set; }

        public int? Status { get; set; }

        public string ServiceId { get; set; }

        public string ResponseToSendSmsRequest { get; set; }

        public string ResponseToCheckStatusRequest { get; set; }

        public int? ErrorCode { get; set; }
    }

    [Table("ShopGroups")]
    public class ShopGroup
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
    }

    [Table("Weekdays")]
    public class Weekday
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
    }

    [Table("PaymentType")]
    public class PaymentType
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
    }
}
