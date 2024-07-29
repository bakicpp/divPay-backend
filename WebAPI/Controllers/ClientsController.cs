using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;

        }


        [HttpGet("getallclients")]
        public IActionResult GetAllClients()
        {
            var res = _clientService.GetAllClients();

            if (res.Success)
            {
                return Ok(res);
            }
            return BadRequest(res);
        }
    }
}

