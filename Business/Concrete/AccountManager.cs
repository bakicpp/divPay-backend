using System;
using System.Collections.Generic;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class AccountManager : IAccountService
    {

        IAccountDal _accountDal;
        IClientService _clientService;

        public AccountManager(IAccountDal accountDal, IClientService clientService)
        {
            _accountDal = accountDal;
            _clientService = clientService;
        }

        public IDataResult<Account> GetAccountByMusteriNo(int musteriNo)
        {
            return new SuccessDataResult<Account>(_accountDal.Get(a => a.MusteriNo == musteriNo));
        }

        public IDataResult<Account> GetAccountInformation(int hesapNo)
        {            
          
            return new SuccessDataResult<Account>(_accountDal.Get(a => a.HesapNo == hesapNo), "Success!");
        }

        public IDataResult<List<Account>> GetAllAccounts()
        {
            return new SuccessDataResult<List<Account>>(_accountDal.GetAll(null), "Tüm hesaplar getirildi.");
        }

        public IResult MakeTransfer(double amount, int targetAccountNo)
        {
            var musteriNo = _clientService.GetCurrentMusteriNo();

            var currentAccount = _accountDal.Get(a => a.MusteriNo == musteriNo);
            var targetAccount = _accountDal.Get(a => a.HesapNo == targetAccountNo);

            if(currentAccount.Bakiye >= amount)
            {
                _accountDal.MakeTransfer(amount, currentAccount, targetAccount);
                return new SuccessResult("Transfer başarıyla gerçekleşti!");
            }
            {
                return new ErrorResult("Yetersiz bakiye!");
            }


        }
    }
}

