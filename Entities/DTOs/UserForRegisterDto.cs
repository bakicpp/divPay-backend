using System;
using Core.Entities;

namespace Entities.DTOs
{
    public class UserForRegisterDto : IDto
    {
        //public string Email { get; set; }
        public int MusteriNo { get; set; }
        public string Sifre { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriSoyadi { get; set; }
    }
}

