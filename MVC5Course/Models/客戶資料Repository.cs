using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MVC5Course.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 Find(int? id)
        {
            return this.All().FirstOrDefault(P => P.Id  == id);
        }

        public override IQueryable<客戶資料> All()
        {
            return base.All().Where (P => P.是否已刪除==false  );
        }

        public IQueryable Sort(string Field, string Type)
        {
            var param = Expression.Parameter(typeof(客戶資料), "x");
            var orderExpression = Expression.Lambda<Func<客戶資料, object>>(Expression.Property(param, Field), param);
            if (Type=="Desc")
            {
                return base.All().OrderByDescending(orderExpression);
            }
            else if (Type=="Asc")
            {
                return base.All().OrderBy (orderExpression);
            }
            else
            {
                return base.All();
            }
       
        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }

        public IQueryable<客戶資料> 搜尋名稱(string KeyWord)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(KeyWord))
            {
                client = client.Where(p => p.客戶名稱.Contains(KeyWord));
            }

            return client;
        }

        public IQueryable <客戶資料> 客戶分類(string KeyWord)
        {
            var client = this.All();
            if (!string.IsNullOrEmpty(KeyWord))
            {
                client = client.Where(p => p.客戶分類 ==KeyWord);
            }

            return client;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}