using System;
using System.Collections.Generic;
using Core.DataAccess;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface ICardDal : IEntityRepository<Card>

    {
        CardDetailDTO GetCardDetails(int id);

        void RemoveMemberFromCard(int cardId, int musteriNo);

        void CreateCard(CardDetail cardDetail);

        void UpdateCard(CardDetail cardDetail, int cardId);

        void DeleteCard(int cardId);

        List<Card> GetMyCards(int musteriNo);

        void DivideExpenseEqually(CardDetailDTO cardDetailDTO);

        void DivideExpenseCustom(CardDetailDTO cardDetailDTO);

        void AddExpense(int harcamaId);

    }
}

