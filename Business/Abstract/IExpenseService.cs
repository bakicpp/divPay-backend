using System;
using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
	public interface IExpenseService
	{
		IDataResult<List<Expense>> GetAllExpenses();
		IDataResult<Expense> GetMyExpenses(int musteriNo);
	}
}

