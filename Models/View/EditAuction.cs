using System.ComponentModel.DataAnnotations;
using IepProjekat.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using IepProjekat.Models.Database;

namespace IepProjekat.Models.View
{
    public class EditAuctionModel{
       
        [Display(Name = "Name")]
        public string name{get; set;}

       
        [Display(Name = "Description")]
        public string description{get; set;}
       
        [Display ( Name = "image")]
        public IFormFile image { get; set; }

      
        [Display(Name = "Start price")]
        public string startPrice{get; set;}

        
        [Display ( Name = "Open date" ) ]
        [DataType(DataType.Date)]
        public DateTime openDate{get; set;}

        
        [Display ( Name = "Close date" ) ]
        [DataType(DataType.Date)]
        public DateTime closeDate{get; set;}

        public int auctionId{get; set;}

    }
}