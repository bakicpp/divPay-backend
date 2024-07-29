using System;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAccountDal : EfEntityRepositoryBase<Account, DivPayDBContext>, IAccountDal
    {
        public void MakeTransfer(double amount, Account currentAccount, Account targetAccount)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
                var currentAccountBalance = currentAccount.Bakiye;

                var targetAccountBalance = targetAccount.Bakiye;

                context.Database.ExecuteSqlInterpolated($"UPDATE Hesap SET Bakiye = {currentAccountBalance - amount} WHERE HesapNo = {currentAccount.HesapNo}");

                context.Database.ExecuteSqlInterpolated($"UPDATE Hesap SET Bakiye = {targetAccountBalance + amount} WHERE HesapNo = {targetAccount.HesapNo}");

            }
        }
    }
}

