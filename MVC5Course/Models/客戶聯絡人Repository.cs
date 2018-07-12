using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MVC5Course.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{

        public IQueryable Sort(string Field, string Type)
        {
            var param = Expression.Parameter(typeof(客戶聯絡人), "");
            var orderExpression = Expression.Lambda<Func<客戶聯絡人, object>>(Expression.Property(param, Field), param);
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
                return base.All();
            }

        }

        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where (p => p.是否已刪除==false);
        }

        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }

        public 客戶聯絡人 Find(int? id)
        {
            return All().FirstOrDefault(P => P.Id == id);
           
        }

        public 客戶聯絡人 FindeMail(int? id, string Mail)
        {
            return All().FirstOrDefault(P => P.客戶Id  == id && P.Email ==Mail);

        }

        public IQueryable <客戶聯絡人> 客戶名稱(string KeyWord)
        {
           return  this.All().Where(P => P.姓名.Contains(KeyWord));
        }

        public IQueryable<客戶聯絡人> 職稱(string KeyWord)
        {
            return this.All().Where(P => P.職稱==KeyWord);
        }

    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}