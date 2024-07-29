using System;
using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class CardManager : ICardService

    {
        ICardDal _cardDal;
        IClientService _clientService;
        IAccountService _accountService;
        IExpenseService _expenseService;

        public CardManager(ICardDal cardDal, IClientService clientService, IAccountService accountService, IExpenseService expenseService)
        {
            _cardDal = cardDal;
            _clientService = clientService;
            _accountService = accountService;
            _expenseService = expenseService;
        }
        

        public IResult AddMember(Card card)
        {

            IResult result = BusinessRules.Run(CheckIfCurrentUserIsAdmin(card.KartId) ,CheckIsClientExist(card.MusteriNo, card.KartId), CheckIfClientExistInCard(card.MusteriNo, card.KartId));
            if (result != null)
            {
                return result;
            }

            _cardDal.Add(card);
            return new SuccessResult(Messages.UserAdded);
        }

     

        public IResult CreateCard(CardDetail cardDetail)
        {
            _cardDal.CreateCard(cardDetail);
            return new SuccessResult(Messages.CardCreated);
        }

        public IResult AddExpense(int harcamaId)
        {
           IResult result = BusinessRules.Run(CheckIfClientsExpense(harcamaId));
           if (result != null)
           {
               return result;
           }            
            _cardDal.AddExpense(harcamaId);
            return new SuccessResult("Harcama eklendi!");
        }

        public IDataResult<List<Card>> GetMyCards()
        {
            var mNo = _clientService.GetCurrentMusteriNo();
            
            return new SuccessDataResult<List<Card>>(_cardDal.GetMyCards(mNo), "Kullanıcının kartları getirildi.");
        }

        public IResult DeleteCard(int cardId)
        {

            IResult result = BusinessRules.Run(CheckIfCurrentUserIsAdmin(cardId));

            if (result != null)
            {
                return result;
            }

            _cardDal.DeleteCard(cardId);
            return new SuccessResult(Messages.CardDeleted);
        }

        public IDataResult<CardDetailDTO> GetCardDetails(int id)
        {   
            return new SuccessDataResult<CardDetailDTO>(_cardDal.GetCardDetails(id), message : Messages.FetchedCardDetail);
        }

        public IResult LeaveCard(int cardId)
        {
            throw new NotImplementedException();
        }

        public IResult RemoveMember(int cardId ,int musteriNo)
        {
            IResult result = BusinessRules.Run(CheckIsClientExist(musteriNo, cardId), CheckIfClientExistInCardForDeletion(musteriNo, cardId));

            if(result != null)
            {
                return result;
            }

            _cardDal.RemoveMemberFromCard(cardId, musteriNo);
            return new SuccessResult(Messages.ClientRemoved);
        }

      
        public IResult UpdateCard(CardDetail cardDetail, int cardId)
        {
            try
            {
                 _cardDal.UpdateCard(cardDetail, cardId);
                return new SuccessResult(Messages.CardUpdated);

            }
            catch(Exception e)
            {
                return new ErrorResult(e.Message);
            }

           
        }

        public IResult DivideExpenseEqually(int cardId)
        {
            var res = _cardDal.GetCardDetails(cardId);
            _cardDal.DivideExpenseEqually(res);
            return new SuccessResult("Grup kart harcaması eşit olarak bölündü ve transfer istekleri gönderildi."); 
        }

        public IResult DivideExpenseCustom(int cardId)
        {
            var res = _cardDal.GetCardDetails(cardId);
            _cardDal.DivideExpenseCustom(res);
            return new SuccessResult("Grup kart harcaması özel olarak bölünmek üzere transfer istekleri gönderildi.");
        }

        private IResult CheckIfCurrentUserIsAdmin(int cardId)
        {
            var mNo = _clientService.GetCurrentMusteriNo();

            var res = _cardDal.GetCardDetails(cardId);

            Console.WriteLine(res);

       
            if (res.KartYoneticisi == mNo)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Yalnızca kart yöneticisi kartı silebilir ya da üye ekleyebilir.");
        }

        private IResult CheckIfClientsExpense(int harcamaId)
        {
            var musteriHarcamalari = _expenseService.GetAllExpenses().Data;
            Console.WriteLine(musteriHarcamalari);
            for (int i = 0; i < musteriHarcamalari.Count; i++)
            {
                if(harcamaId != musteriHarcamalari[i].HarcamaId)
                {
                    Console.WriteLine(musteriHarcamalari[i].HarcamaId);
                    return new ErrorResult("Müşteriye ait harcama bulunamadı!");
                }
                else if(harcamaId == musteriHarcamalari[i].HarcamaId)
                {
                    break;
                }
            }
            return new SuccessResult();
        }

        private IResult CheckIsClientExist(int musteriNo, int cardId)
        {
            var result = _clientService.GetAllClients().Data.Any(c=>c.MusteriNo == musteriNo);

            if (!result)
            {
                return new ErrorResult("Müşteri bulunamadı.");
            }
            return new SuccessResult();
        }

        private IResult CheckIfClientExistInCard(int musteriNo, int cardId)
        {
            var result = _cardDal.GetCardDetails(cardId).Uyeler.Any(x => x == musteriNo);  //client is exist in card

            if (result)
            {
                return new ErrorResult("Müşteri zaten kartta mevcut.");
            }
            return new SuccessResult();
        }

        private IResult CheckIfClientExistInCardForDeletion(int musteriNo, int cardId)
        {

            var result = _cardDal.GetCardDetails(cardId).Uyeler.Any(x=>x==musteriNo); //client is exist in card

            if (result)
            {
                return new SuccessResult();
            }

            return new ErrorResult("Müşteri zaten kartta mevcut değil!");
        }

     
    }
}

