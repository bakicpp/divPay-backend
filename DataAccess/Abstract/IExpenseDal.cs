using System;
using System.Collections.Generic;
using Core.DataAccess;
using Core.Utilities.Results;
using Entities.Concrete;

namespace DataAccess.Abstract
{
	public interface IExpenseDal : IEntityRepository<Expense> 
	{
	}
}

