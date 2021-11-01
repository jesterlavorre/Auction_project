using System.ComponentModel.DataAnnotations;
using IepProjekat.Controllers;
using Microsoft.AspNetCore.Mvc;
using IepProjekat.Models.Database;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using X.PagedList;

namespace IepProjekat.Models.View
{
    public class UserListModel{
        
        public User loggedInUser;

        public IPagedList<User> userList {get; set;}
        public IPagedList<Auction> auctionList {get; set;} 

    }
}