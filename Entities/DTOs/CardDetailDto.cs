﻿using System;
using System.Collections.Generic;
using Core.Entities;
using Entities.Concrete;

namespace Entities.DTOs
{
    public class CardDetailDTO : IDto
    {
        public int KartId { get; set; }
        public string KartAdi { get; set; }
        public int KartRenk { get; set; }
        //public int MusteriNo { get; set; }
        public int KartYoneticisi { get; set; }
        public List<int> Uyeler { get; set; }
        public List<int>Harcamalar { get; set; }
        public double ToplamHarcama { get; set; }


    }
}

