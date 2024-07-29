using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class Client : IEntity
    {
        public int id { get; set; }

        public int MusteriNo { get; set; }

        public string MusteriAdi { get; set; }

        public string MusteriSoyadi { get; set; }

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }


    }
}

