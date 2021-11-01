using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IepProjekat.Models;
using Microsoft.AspNetCore.Authorization;
using IepProjekat.Models.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IepProjekat.Models.View;
using X.PagedList;

namespace IepProjekat.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IepProjekatContext context;
        private UserManager<User> userManager;

        public HomeController(IepProjekatContext context, UserManager<User> userManager, ILogger<HomeController> logger)
        {
            this.context = context;
            this.userManager = userManager;
            _logger = logger;
        }


       
        public async Task<IActionResult> Index()
        {
            User logg = await this.userManager.GetUserAsync(base.User);

             IList<Auction> list = await this.context.Auctions.Include(t => t.owner).Where(t => t.state == "OPEN").OrderByDescending(t => t.openDate).ToListAsync();
             
             int auctionsCount = list.Count;
             
             var winu = this.context.Users.Join(context.Auctions,
                    user => user,
                    win => win.winner,
                    (user, win) => new
                    {
                        UserName = user.UserName,
                        auctionId = win.Id
                    }
                ).ToList();
             
             var winner = new List<KeyValuePair<int, string>>();

             foreach(var a in winu){
                winner.Add(new KeyValuePair<int, string>(a.auctionId, a.UserName));
             }
             
             IndexModel model = new IndexModel()
             {
                 loggedInUser = logg,
                 currentPage = 1,
                 pageNumbers = (int)Math.Ceiling(auctionsCount/12.0),
                 auctionsList = list.ToPagedList(1,12),
                 winners = winner
             };

             

            return View(model);
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ViewResult GetInfoAuction(int id){
            Auction model;

            model = this.context.Auctions.Include(a => a.owner)
            .FirstOrDefault(s => s.Id == id);

            return View(model);
        }
         public async Task<IActionResult> NextPage(int? page)
        {
            User logg = await this.userManager.GetUserAsync(base.User);
            
             IList<Auction> list = await this.context.Auctions.Include(t => t.owner).Where(t => t.state == "OPEN").OrderByDescending(t => t.openDate).ToListAsync();
             
             int auctionsCount = list.Count;

             var winu = this.context.Users.Join(context.Auctions,
                    user => user,
                    win => win.winner,
                    (user, win) => new
                    {
                        UserName = user.UserName,
                        auctionId = win.Id
                    }
                ).ToList();
             
             var winner = new List<KeyValuePair<int, string>>();

             foreach(var a in winu){
                 await Console.Out.WriteAsync(a.UserName);
                winner.Add(new KeyValuePair<int, string>(a.auctionId, a.UserName));
             }
             
             IndexModel auction = new IndexModel()
             {
                 loggedInUser = logg,
                 currentPage = (int)page,
                 pageNumbers = (int)Math.Ceiling(auctionsCount/12.0),
                 auctionsList = list.ToPagedList((int)page,12),
                 winners = winner
             };

            return PartialView ("List", auction);
           
        }

        public async Task<IActionResult> FilteredPage(int? page, String search, int? minimumPrice, int? maximumPrice, String state)
        {
            User logg = await this.userManager.GetUserAsync(base.User);

            IQueryable<Auction> FilterListAuction = this.context.Auctions;
            
            if(search!=null) FilterListAuction = FilterListAuction.Where(a => a.name.Contains(search));
            
            if(minimumPrice != null)
                if(minimumPrice >= 0) FilterListAuction = FilterListAuction.Where(a => a.currentPrice >= minimumPrice);
            
            if(maximumPrice != null)
                if(maximumPrice > 0) FilterListAuction = FilterListAuction.Where(a => a.currentPrice <= maximumPrice);
            
            if(state != "OPEN") FilterListAuction = FilterListAuction.Where(a => a.state==state);
            else FilterListAuction = FilterListAuction.Where(a => a.state == "OPEN");
            
            IList<Auction> list = await FilterListAuction.OrderByDescending(a => a.createDate).ToListAsync();
             
             int auctionsCount = list.Count;
             
             
             IndexModel auction = new IndexModel(){
                 loggedInUser = logg,
                 currentPage = (int)page,
                 pageNumbers = (int)Math.Ceiling(auctionsCount/12.0),
                 auctionsList = list.ToPagedList((int)page,12),
             };

            return PartialView ("List", auction);
           
        }
    }
}
