using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using AutoMapper;
using IepProjekat.Models.Database;
using IepProjekat.Models.View;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using IepProjekat.Controllers;

namespace IepProjekat.Controllers {
    public class AdministratorController : Controller {

        private IepProjekatContext context;
        private UserManager<User> userManager;
        private IMapper mapper;
        private SignInManager<User> signInManager;

        public AdministratorController(IepProjekatContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> AuctionList(int? page){
            IList<Auction> list = await this.context.Auctions.Include(t => t.owner).Where(t => t.state == "DRAFT").OrderByDescending(t => t.createDate).ToListAsync();
            
            UserListModel model = new UserListModel(){
                auctionList = list.ToPagedList(page ?? 1,10)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptAuction ( int? id ) {

            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            
            Auction auction = await this.context.Auctions.FirstOrDefaultAsync(s => s.Id == id);
            


            if(auction != null){
                auction.state = "OPEN";
            }


            await this.context.SaveChangesAsync();
            return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineAuction ( int? id ) {

            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            
            Auction auction = await this.context.Auctions.FirstOrDefaultAsync(s => s.Id == id);
            


            if(auction != null){
                auction.state = "DELETED";
            }

            await this.context.SaveChangesAsync();

           

            return View();
            
        }

          public async Task<IActionResult> UserListAsync(int? page){
            var users = userManager.Users; 

            UserListModel model = new UserListModel(); 

            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            model.loggedInUser = loggedInUser;

            IList<User> list = await this.userManager.Users.OrderBy(s => s.state).ToListAsync();
            
            model.userList = list.ToPagedList(page ?? 1,10);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ban ( string username ) {

            User loggedInUser = await this.userManager.GetUserAsync(base.User);
                       
            User user = await this.userManager.FindByNameAsync(username);

            if(user != null){
                user.state = "Banned";

                foreach(var auction in context.Auctions){
                    if(auction.owner == user){
                        auction.state = "DELETED";

                        this.context.Auctions.Update (auction);
                    }
                }
            }

            await this.userManager.UpdateAsync(user);

            await this.context.SaveChangesAsync ( );

           

            return PartialView("UnbanUser", user);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unban ( string username ) {

            User loggedInUser = await this.userManager.GetUserAsync(base.User);
                       
            User user = await this.userManager.FindByNameAsync(username);

            if(user != null){
                user.state = "Active";
            }

            await this.userManager.UpdateAsync(user);


            return PartialView("BanUser", user);
            
        }

    }
}