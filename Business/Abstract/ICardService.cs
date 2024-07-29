using System.Collections.Generic;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface ICardService
    {   
        IDataResult<List<Card>> GetMyCards(); 
        IDataResult <CardDetailDTO> GetCardDetails(int cardId);
        IResult CreateCard(CardDetail cardDetail);
        IResult DeleteCard(int cardId);
        IResult UpdateCard(CardDetail cardDetail, int cardId);
        IResult LeaveCard(int cardId);
        IResult AddMember(Card card);
        IResult RemoveMember(int cardId, int musteriNo);
        IResult AddExpense(int harcamaId);
        IResult DivideExpenseEqually(int cardId);
        IResult DivideExpenseCustom(int cardId);
    }
}

