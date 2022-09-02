using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace TeknolojiHaber.Models
{
    public class Haber
    {
        public Haber()
        {
            Resimler = new List<Resim>();
            KategoriHaberler = new List<KategoriHaber>();
        }

        public int Id { get; set; }


        [Display(Name="Haber Başlığı")]
        [Required (ErrorMessage="{0} Alanı Boş Bırakılamaz.")]
        [StringLength(50,MinimumLength=5,ErrorMessage="Haber Başlığı En Az 5 En Fazla 50 Olabilir")]
        public string HaberBaslik { get; set; }

        [Required (ErrorMessage="{0} Alanı Boş Bırakılamaz.")]
        
        [Display(Name="Haber Bilgisi")]
        public string HaberBilgi { get; set; }

        [Display(Name="Haber Tarihi")]
        [DataType(DataType.Date)]
        public DateTime HaberTarih { get; set; }

        
        [Required(ErrorMessage = "Lütfen Bir Sayı Giriniz")] 
        [DisplayFormat(ApplyFormatInEditMode=false)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "0-9 Arası Sayı Girin.")]
        [Range(0, 9, ErrorMessage = "İlgi Düzeyi 0-9 Arası Olmalıdır.")]
        
        public decimal IlgiDuzeyi { get; set; }


        [NotMapped]
        public IFormFile[] Dosyalar { get; set; }

        public List<Resim> Resimler { get; set; } = new List<Resim>();

        public List<Kategori> Kategorileri{get; set;} = new List<Kategori>();

        public List<KategoriHaber> KategoriHaberler{get; set;} 
    }
    }