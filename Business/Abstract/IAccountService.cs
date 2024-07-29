using System;
using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
	public interface IAccountService
	{
        IDataResult<Account> GetAccountInformation(int hesapNo);
        IResult MakeTransfer(double amount, int targetAccountNo);
        IDataResult<Account> GetAccountByMusteriNo(int musteriNo);
        IDataResult<List<Account>> GetAllAccounts();
    }
}

