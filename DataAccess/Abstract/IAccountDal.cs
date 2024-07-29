using System;
using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
	public interface IAccountDal : IEntityRepository<Account>
	{
		void MakeTransfer(double amount, Account currentAccountNo, Account targetAccount);
		
	}
}

