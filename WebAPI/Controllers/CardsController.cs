using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;

        }
        
        [HttpGet("getmycards")]
        public IActionResult GetMyCards()
        {
            var res = _cardService.GetMyCards();

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet("getcarddetails")]
        public IActionResult GetCardDetails(int id)
        {
            var res = _cardService.GetCardDetails(id);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost("divide/equal")]
        public IActionResult DivideEqually(int id)
        {
            var res = _cardService.DivideExpenseEqually(id);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost("divide/custom")]
        public IActionResult DivideCustom(int id)
        {
            var res = _cardService.DivideExpenseCustom(id);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost("addexpense")]
        public IActionResult AddExpense(int harcamaId)
        {
            var res = _cardService.AddExpense(harcamaId);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }


        [HttpPost("createcard")]
        public IActionResult CreateCard(CardDetail cardDetail)
        {
            var res = _cardService.CreateCard(cardDetail);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete("deletecard")]
        public IActionResult DeleteCard(int id)
        {
            var res = _cardService.DeleteCard(id);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPatch("updatecard")]
        public IActionResult UpdateCard(CardDetail cardDetail, int id)
        {
            var res = _cardService.UpdateCard(cardDetail, id);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost("addmember")]
        public IActionResult AddMember(Card card)
        {
            var res = _cardService.AddMember(card);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpDelete("removemember")]
        public IActionResult RemoveMember(int cardId, int musteriNo)
        {
            var res = _cardService.RemoveMember(cardId, musteriNo);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

    }
}

