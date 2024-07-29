using System;
using Core.Entities;

namespace Entities.DTOs
{
    public class UserForLoginDto : IDto
    {
        public int MusteriNo { get; set; }
        public string Sifre { get; set; }
    }
}

