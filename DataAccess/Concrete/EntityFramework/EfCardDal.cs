using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.DataAccess.EntityFramework;
using Core.Entities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCardDal : EfEntityRepositoryBase<Card, DivPayDBContext>, ICardDal
    {
        IAccountDal _accountDal;
        IClientDal _clientDal;
        IPaymentRequestDal _paymentRequestDal;

        public EfCardDal(IAccountDal accountDal, IClientDal clientDal, IPaymentRequestDal paymentRequestDal)
        {
            _accountDal = accountDal;
            _clientDal = clientDal;
            _paymentRequestDal = paymentRequestDal;
        }

        public void AddExpense(int harcamaId)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
                var musteriNo = _clientDal.GetCurrentMusteriNo();

                Card card = Get(c => c.MusteriNo == musteriNo);

                Console.WriteLine(card);


               if(card.HarcamaId == 0)
                {
                    Console.WriteLine(harcamaId);
                    context.Database.ExecuteSqlInterpolated($"UPDATE Kart SET HarcamaId = {harcamaId} WHERE MusteriNo = {musteriNo}");
                    context.SaveChanges();
                }
                else
                {
                    Card newCard = new Card { KartId = card.KartId, MusteriNo = musteriNo, HarcamaId = harcamaId };
                    Console.WriteLine(newCard);
                    Add(newCard);
                }
            }
        }

        public void CreateCard(CardDetail cardDetail)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
                //int cardId = UniqueIdGenerator();   
                //var res = clientDal.GetCurrentMusteriNo();

                //Console.WriteLine(res.ToString());


                var addedEntity = context.Entry(cardDetail);
                addedEntity.State = EntityState.Added;

                Card card = new Card { KartId = cardDetail.KartId, MusteriNo = 99, HarcamaId = 0}; // olmayan bir müşteri gönderme!

                var addToKartTable = context.Entry(card);
                addToKartTable.State = EntityState.Added;
                Console.WriteLine(card);
                context.SaveChanges();
                
            }
        }

        public void DeleteCard(int cardId)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
                context.Database.ExecuteSqlInterpolated($"DELETE Kart WHERE KartId = {cardId}");
                context.Database.ExecuteSqlInterpolated($"DELETE KartDetay WHERE KartId = {cardId}");
                context.SaveChanges();
            }
        }

        public void DivideExpenseCustom(CardDetailDTO cardDetailDTO)
        {
            var mNO = _clientDal.GetCurrentMusteriNo();
            var currentAccount = _accountDal.Get(a => a.MusteriNo == mNO);

            List<Account> targetAccounts = new();

            foreach (var musteriNo in cardDetailDTO.Uyeler)
            {
                targetAccounts = _accountDal.GetAll(a => a.MusteriNo == musteriNo && a.MusteriNo != cardDetailDTO.KartYoneticisi);
            } //hedef hesapları getir.

            foreach (var account in targetAccounts)
            {
                _paymentRequestDal.SendTransferRequest(cardDetailDTO.KartId,currentAccount.HesapNo, account.HesapNo, cardDetailDTO.ToplamHarcama, DivideType.Custom);
            }

        }

        public void DivideExpenseEqually(CardDetailDTO cardDetailDTO)
        {
            var mNO = _clientDal.GetCurrentMusteriNo();
            var currentAccount = _accountDal.Get(a => a.MusteriNo == mNO);
            var groupMemberCount = cardDetailDTO.Uyeler.Count();
            
            double amount = cardDetailDTO.ToplamHarcama / groupMemberCount;

                List<Account> targetAccounts = new();

                foreach (var musteriNo in cardDetailDTO.Uyeler)
                {
                  targetAccounts = _accountDal.GetAll(a => a.MusteriNo == musteriNo && a.MusteriNo != cardDetailDTO.KartYoneticisi);
                } //hedef hesapları getir.

                foreach (var account in targetAccounts)
                {
                _paymentRequestDal.SendTransferRequest(cardDetailDTO.KartId,currentAccount.HesapNo, account.HesapNo, amount, DivideType.Equally);
                }
        }

        //private static int UniqueIdGenerator()
        //{
        //    Random random = new Random();
        //    int min = 100000000; // 9 basamaklı en küçük sayı
        //    int max = 999999999; // 9 basamaklı en büyük sayı
        //    return random.Next(min, max + 1);
        //}

        public CardDetailDTO GetCardDetails(int id)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
                var result = from k in context.Kart
                             join kd in context.KartDetay on k.KartId equals kd.KartId
                             join m in context.Musteri on k.MusteriNo equals m.MusteriNo
                             where kd.KartId == id
                             select new
                             DTOIslem
                             {
                                 KartId = kd.KartId,
                                 KartAdi = kd.KartAdi,
                                 KartRenk = kd.KartRenk,
                                 KartYoneticisi = kd.KartYoneticisi,
                                 MusteriNo = m.MusteriNo,                                 
                            };

                var harcamaResult = from k in context.Kart
                                    join h in context.Harcama on k.HarcamaId equals h.HarcamaId
                                    select new DTOIslem
                                    {
                                        HarcamaId = h.HarcamaId,

                                    };
                                    

                var harcamaDetay = from k in context.Kart
                                   join h in context.Harcama on k.HarcamaId equals h.HarcamaId
                                   select new Expense
                                   {
                                       HesapNo = h.HesapNo,
                                       Miktar = h.Miktar,
                                       HarcamaId = h.HarcamaId,
                                   };


                CardDetailDTO cardDetailDTO = new CardDetailDTO();
                cardDetailDTO.Uyeler = new List<int>();
                cardDetailDTO.Harcamalar = new List<int>();
                cardDetailDTO.ToplamHarcama = 0;

                foreach (var dto in harcamaDetay.ToList())
                {
                    Console.WriteLine(dto.Miktar);
                    cardDetailDTO.ToplamHarcama += dto.Miktar;
                }

                foreach (var dto in harcamaResult.ToList())
                {
                    cardDetailDTO.Harcamalar.Add(dto.HarcamaId);
                }
                Console.WriteLine(cardDetailDTO.Harcamalar);
                foreach (var dto in result.ToList())
                {
                   if (!cardDetailDTO.Uyeler.Contains(dto.MusteriNo))
                        {
                            cardDetailDTO.Uyeler.Add(dto.MusteriNo);
                        }
                    cardDetailDTO.KartId = dto.KartId;
                    cardDetailDTO.KartAdi = dto.KartAdi;
                    cardDetailDTO.KartRenk = dto.KartRenk;
                    cardDetailDTO.KartYoneticisi = dto.KartYoneticisi;
                }

                context.Database.ExecuteSqlInterpolated($"UPDATE KartDetay SET ToplamHarcama = {cardDetailDTO.ToplamHarcama} WHERE KartId = {cardDetailDTO.KartId}"); //update table's total expense


                return cardDetailDTO;
                
            }
        }

    

        public List<Card> GetMyCards(int musteriNo)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
                var res = context.Kart.Where(k => k.MusteriNo == musteriNo).Select(c => new Card
                {
                    id = c.id,
                    KartId = c.KartId,
                    MusteriNo = c.MusteriNo,

                }).ToList();
                return res;
            }
        }

        public void RemoveMemberFromCard(int cardId, int musteriNo)
        {
            using (DivPayDBContext context = new DivPayDBContext())
            {
              context.Database.ExecuteSqlInterpolated($"DELETE Kart WHERE MusteriNo = {musteriNo} and KartId = {cardId}");
            }
        }

        public void UpdateCard(CardDetail cardDetail, int cardId)
        {
        
            using (DivPayDBContext context = new DivPayDBContext())
            {
                if (!string.IsNullOrEmpty(cardDetail.KartAdi))
                {
                    context.Database.ExecuteSqlInterpolated($"UPDATE KartDetay SET KartAdi = {cardDetail.KartAdi} WHERE KartId = {cardId}");
                }

                if (cardDetail.KartRenk != 0)
                {
                    context.Database.ExecuteSqlInterpolated($"UPDATE KartDetay SET KartRenk = {cardDetail.KartRenk} WHERE KartId = {cardId}");
                }
               
                context.SaveChanges();
            }
        }
    }
}

