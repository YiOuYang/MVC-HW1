using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace MVC5Course.Models
{


    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        private static MethodInfo orderbyInfo = null;
        private static MethodInfo orderbyDecInfo = null;



        public IQueryable Sort(string Field, string Type)
        {
            var param = Expression.Parameter(typeof(客戶銀行資訊), "");
            var orderExpression = Expression.Lambda<Func<客戶銀行資訊, object>>(Expression.Property(param, Field), param);
        
            if (Type == "Desc")
            {
                return base.All().OrderByDescending(orderExpression);
            }
            else if (Type == "Asc")
            {
                return base.All().OrderBy(orderExpression);
            }
            else
            {
                return base.All().Where(P => P.是否已刪除 == false);
            }
        }



        public 客戶銀行資訊 Find(int? id)
        {

            return this.All().FirstOrDefault(P => P.Id == id);
         }

        public IQueryable<客戶銀行資訊> 銀行名稱(string KeyWord)
        {

            var Bank = this.All();
            if (!string.IsNullOrEmpty(KeyWord))
            {
                Bank = Bank.Where(P => P.銀行名稱.Contains(KeyWord));
            }

            return Bank;
        }

        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(P => P.是否已刪除 == false);
            // return base.All();
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}