using System;
using System.Collections.Generic;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class ExpenseManager : IExpenseService
	{

        IExpenseDal _expenseDal;
        IClientService _clientService;
        IAccountService _accountService;

        public ExpenseManager(IExpenseDal expenseDal, IClientService clientService, IAccountService accountService)
        {
            _expenseDal = expenseDal;
            _clientService = clientService;
            _accountService = accountService;
        }

        public IDataResult<List<Expense>> GetAllExpenses()
        {
            var currentMusteriNo = _clientService.GetCurrentMusteriNo();
            var currentAccount = _accountService.GetAccountByMusteriNo(currentMusteriNo).Data;

            return new SuccessDataResult<List<Expense>>(_expenseDal.GetAll(e => e.HesapNo == currentAccount.HesapNo), $"{currentMusteriNo} no'lu müşterinin harcamaları getirildi.");
        }

        public IDataResult<Expense> GetMyExpenses(int musteriNo)
        {
            throw new NotImplementedException();
        }
    }
}

