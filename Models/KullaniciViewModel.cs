using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TeknolojiHaber.Models
{
    public class KullaniciViewModel
    {
        public string Eposta { get; set; }
        public string Sifre { get; set; }

        public string Token { get; set; }
    }
}