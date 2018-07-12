namespace MVC5Course.Models
{
    using MVC5Course.Models.InputValidations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    //public partial class 客戶聯絡人
    //{
    //}
    public partial class 客戶聯絡人 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (this.Id == 0)
            {
                //主Key沒值 代表是新增 欄位屬性是int 故預設為0
            }
            客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
            var Contact = repo.FindeMail(this.客戶Id, this.Email);
            if (Contact != null)
            {
                if (this.Id == Contact.Id && this.客戶Id == Contact.客戶Id && this.Email == Contact.Email)
                {
                    //與現有DB資料完全相同
                }
                else
                {
                    yield return new ValidationResult("同一個客戶下的聯絡人，其 Email 不能重複 ", new string[] { "客戶Id", "Email", });
                }

            }
            else
            {

            }
        }
    }


    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        public string Email { get; set; }
        
       [手機]
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
       public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 電話 { get; set; }
        [Required]
        public bool 是否已刪除 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
