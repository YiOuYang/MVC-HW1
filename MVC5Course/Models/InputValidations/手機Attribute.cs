using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC5Course.Models.InputValidations
{
    public class 手機Attribute: DataTypeAttribute
    {

        public 手機Attribute() : base(DataType.Text)
        {
            ErrorMessage = "請輸入正確的電話格式 e.g. 0911-111111";
        }

        public override bool IsValid(object value)
        {
            string str = (string)value;
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            else
            {
                return PhoneRreg(str);
            }


        }

        public bool PhoneRreg(string arg_Identify)
        {
            string pattern = @"\d{4}-\d{6}";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = regex.Match(arg_Identify);

            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}