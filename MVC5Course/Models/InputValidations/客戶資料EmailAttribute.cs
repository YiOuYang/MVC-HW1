using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MVC5Course.Models.InputValidations
{
    public class 客戶資料EmailAttribute: DataTypeAttribute
    {

        public 客戶資料EmailAttribute() : base(DataType.Text)
        {
            ErrorMessage = "請輸入正確的EMail格式 ";
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}