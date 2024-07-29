using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPaymentRequestDal : EfEntityRepositoryBase<PaymentRequest, DivPayDBContext>, IPaymentRequestDal
    {
        IAccountDal _accountDal;
        IExpenseDal _expenseDal;
  
        public EfPaymentRequestDal(IAccountDal accountDal, IExpenseDal expenseDal)
        {
            _accountDal = accountDal;
            _expenseDal = expenseDal;
      
        }
        public void ApproveTransferRequest(int odemeIstegiId, CardDetailDTO cardDetail, double customAmount = 0 )
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
               
               var paymentRequest = Get(o => o.OdemeIstegiId == odemeIstegiId);
               context.Database.ExecuteSqlInterpolated($"DELETE OdemeIstekleri WHERE OdemeIstegiId = {odemeIstegiId}");

                var currentAccount = _accountDal.Get(a => a.HesapNo == paymentRequest.AliciHesapNo);
                var targetAccount = _accountDal.Get(a => a.HesapNo == paymentRequest.GondericiHesapNo);

                var currentAccountExpenses = _expenseDal.GetAll(e => e.HesapNo == currentAccount.HesapNo);

                var targetAccountExpenses = _expenseDal.GetAll(e => e.HesapNo == targetAccount.HesapNo);

                var currentAccountMNoCount = context.Kart.Where(k=>k.MusteriNo == currentAccount.MusteriNo).ToList().Count();
                var targetAccountMNoCount = context.Kart.Where(k => k.MusteriNo == targetAccount.MusteriNo).ToList().Count();


                if (paymentRequest.Tip == "equally")
                {
                    var updatedTotalExpense = cardDetail.ToplamHarcama - paymentRequest.Miktar;

                    _accountDal.MakeTransfer(paymentRequest.Miktar, currentAccount, targetAccount);

                    context.Database.ExecuteSqlInterpolated($"UPDATE KartDetay SET ToplamHarcama = {updatedTotalExpense} WHERE KartId = {cardDetail.KartId}");

                    foreach (var exp in currentAccountExpenses)
                    {
                        if (currentAccountMNoCount > 1)
                        {
                            context.Database.ExecuteSqlInterpolated($"DELETE Kart WHERE HarcamaId = {exp.HarcamaId}");
                        }
                        else
                        {
                            context.Database.ExecuteSqlInterpolated($"UPDATE Kart SET HarcamaId = {0} WHERE KartId = {cardDetail.KartId}");

                        }
                    }
                    foreach (var exp in targetAccountExpenses)
                    {
                        if (targetAccountMNoCount > 1)
                        {
                            context.Database.ExecuteSqlInterpolated($"DELETE Kart WHERE HarcamaId = {exp.HarcamaId}");
                        }
                        else
                        {
                            context.Database.ExecuteSqlInterpolated($"UPDATE Kart SET HarcamaId = {0} WHERE KartId = {cardDetail.KartId}");
                        }
                    }

                }
                else
                {
                    var updatedTotalExpense = cardDetail.ToplamHarcama - customAmount;
                    context.Database.ExecuteSqlInterpolated($"UPDATE KartDetay SET ToplamHarcama = {updatedTotalExpense} WHERE KartId = {cardDetail.KartId}");
                    foreach (var exp in currentAccountExpenses)
                    {
                        if (currentAccountMNoCount > 1)
                        {
                            context.Database.ExecuteSqlInterpolated($"DELETE Kart HarcamaId = {exp.HarcamaId} WHERE KartId = {cardDetail.KartId}");
                        }
                        else
                        {
                            context.Database.ExecuteSqlInterpolated($"UPDATE Kart SET HarcamaId = {0} WHERE KartId = {cardDetail.KartId}");
                        }
                    }
                    foreach (var exp in targetAccountExpenses)
                    {
                        if (targetAccountMNoCount > 1)
                        {
                            context.Database.ExecuteSqlInterpolated($"DELETE Kart HarcamaId = {exp.HarcamaId} WHERE KartId = {cardDetail.KartId}");
                        }
                        else
                        {
                            context.Database.ExecuteSqlInterpolated($"UPDATE Kart SET HarcamaId = {0} WHERE KartId = {cardDetail.KartId}");
                        }
                    }
                    _accountDal.MakeTransfer(customAmount, currentAccount, targetAccount);
                }

                context.SaveChanges();
            }

        }

        public List<PaymentRequest> GetMyIncomingTransferRequests(int hesapNo)
        {

            using (DivPayDBContext context = new DivPayDBContext())
            {
                var res = context.OdemeIstekleri
                      .Where(o => o.AliciHesapNo == hesapNo)
                      .ToList();
                return res;
            }
        }

        

        public void SendTransferRequest(int cardId, int senderHesapNo, int receiverHesapNo, double transferAmount, DivideType divideType)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {

                PaymentRequest paymentRequest = new PaymentRequest
                {
                    Aciklama = $"{senderHesapNo} numarali hesap tarafindan {transferAmount}TL grup kart odemesi talep edildi.",
                    AliciHesapNo = receiverHesapNo,
                    Durum = true,
                    GondericiHesapNo = senderHesapNo,
                    Miktar = transferAmount,
                    Tip = divideType == DivideType.Equally ? "equally" : "custom",
                    KartId = cardId,
                };
                var addedEntity = context.Entry(paymentRequest);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
            }
        }
    }

    
}

public enum DivideType
{
    Equally,
    Custom
}

