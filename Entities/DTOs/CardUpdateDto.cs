using System;
using System.Collections.Generic;
using Core.Entities;
using Entities.Concrete;

namespace Entities.DTOs
{
    public class CardUpdateDto : IDto
    {
        public string KartAdi { get; set; }
        public int KartRenk { get; set; }

    }
}

