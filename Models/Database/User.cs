using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using IepProjekat.Models.View;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IepProjekat.Models.Database {
    public class User : IdentityUser {
        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public string genre{get;set;}

        [Required]
        public string state{get;set;}

        [Required]
        [Display(Name = "Tokens")]
        public int tokens{get; set;}

    }

    public class UserProfile : Profile {
        public UserProfile ( ) {
            base.CreateMap<RegisterModel, User> ( )
                .ForMember (
                    destination => destination.Email,
                    options => options.MapFrom ( data => data.email )
                ).ForMember (
                    destination => destination.UserName,
                    options => options.MapFrom ( data => data.username )
                ).ReverseMap ( );
        }
    }
}