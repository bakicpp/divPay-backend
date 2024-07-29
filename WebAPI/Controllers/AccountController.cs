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
    public class AccountController : ControllerBase
    {
        IAccountService _accountService;
        IExpenseService _expenseService;
        IPaymentRequestService _paymentRequestService;

        public AccountController(IAccountService accountService, IExpenseService expenseService, IPaymentRequestService paymentRequestService)
        {
            _accountService = accountService;
            _expenseService = expenseService;
            _paymentRequestService = paymentRequestService;

        }

        [HttpGet("information")]
        public IActionResult GetAccountInformation(int hesapNo)
        {
            var res = _accountService.GetAccountInformation(hesapNo);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPatch("transfer")]
        public IActionResult MakeTransfer(double amount, int targetAccountNo)
        {
            var res = _accountService.MakeTransfer(amount, targetAccountNo);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet("getall")]
        public IActionResult GetAllAccounts()
        {
            var res = _accountService.GetAllAccounts();

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet("transfer/requests/incoming")]
        public IActionResult GetMyIncomingTransferRequests()
        {
            var res = _paymentRequestService.GetMyIncomingTransferRequests();

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpPost("transfer/requests/approve")]
        public IActionResult ApproveTransferRequest(int id)
        {
            var res = _paymentRequestService.ApproveTransferRequest(id);

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }

        [HttpGet("expenses")]
        public IActionResult GetAccountExpenses()
        {
            var res = _expenseService.GetAllExpenses();

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }



    }
}

