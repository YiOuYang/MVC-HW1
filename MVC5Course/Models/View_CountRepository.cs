using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class View_CountRepository : EFRepository<View_Count>, IView_CountRepository
	{

	}

	public  interface IView_CountRepository : IRepository<View_Count>
	{

	}
}