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
    public class UserController : Controller {

        private IepProjekatContext context;
        private UserManager<User> userManager;
        private IMapper mapper;
        private SignInManager<User> signInManager;

        public UserController ( IepProjekatContext context, UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager ) {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }

        public IActionResult isEmailUnique ( string email ) {
            bool exists = this.context.Users.Where ( user => user.Email == email ).Any ( );

            if ( exists ) {
                return Json ( "Email already taken!" );
            } else {
                return Json ( true );
            }
        }

        public IActionResult isUsernameUnique ( string username ) {
            bool exists = this.context.Users.Where ( user => user.UserName == username ).Any ( );

            if ( exists ) {
                return Json ( "Username already taken!" );
            } else {
                return Json ( true );
            }
        }

        public IActionResult Register ( ) {
            return View ( );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register ( RegisterModel model ) {
            if ( !ModelState.IsValid ) {
                return View ( model );
            }

            User user = this.mapper.Map<User> ( model );
            
            IdentityResult result = await this.userManager.CreateAsync ( user, model.password );

            if ( !result.Succeeded ) {
                foreach ( IdentityError error in result.Errors ) {
                    ModelState.AddModelError ( "", error.Description );
                }

                return View ( model );
            }

            result = await this.userManager.AddToRoleAsync ( user, Roles.user.Name );

            if ( !result.Succeeded ) {
                foreach ( IdentityError error in result.Errors ) {
                    ModelState.AddModelError ( "", error.Description );
                }

                return View ( model );
            }

            return RedirectToAction ( nameof ( HomeController.Index ), "Home" );
        }

        public IActionResult LogIn ( string returnUrl ) {
           LogInModel model = new LogInModel ( ) {
               returnUrl = returnUrl
           };
           return View ( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn ( LogInModel model ) {
            if ( !ModelState.IsValid ) {
                return View ( model );
            }

            var result = await this.signInManager.PasswordSignInAsync ( model.username, model.password, false, false );

            if ( !result.Succeeded ) {
                ModelState.AddModelError ( "", "Username or password not valid!" );
                return View ( model );
            }

            if ( model.returnUrl != null ) {
                return Redirect ( model.returnUrl );
            } else {
                return RedirectToAction ( nameof ( HomeController.Index ), "Home" );
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut ( ) {
            await this.signInManager.SignOutAsync ( );
            return RedirectToAction ( nameof ( HomeController.Index ), "Home" );
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeProfile ( ProfileModel model ) {
            if ( !ModelState.IsValid ) {
                return View ( model );
            }

            User loggedInUser = await this.userManager.GetUserAsync(base.User);
                       

            if(loggedInUser.Email != model.email){
                var resultMail = await this.userManager.SetEmailAsync(loggedInUser, model.email);

                if( !resultMail.Succeeded ){
                    ModelState.AddModelError ( "", "Mail not valid!" );
                    return View ( model );
                }
            }

            if( model.password != null){
                var resultMail = await this.userManager.ChangePasswordAsync(loggedInUser, model.oldPassword, model.password);

                if( !resultMail.Succeeded ){
                    ModelState.AddModelError ( "", "Old Password not valid!" );
                    return View ( model );
                }
            }

            if(loggedInUser.UserName != model.username){
                var resultMail = await this.userManager.SetUserNameAsync(loggedInUser, model.username);

                if( !resultMail.Succeeded ){
                    ModelState.AddModelError ( "", "Username not valid!" );
                    return View ( model );
                }
            }

            if(loggedInUser.firstName != model.firstName){
                loggedInUser.firstName = model.firstName;
            }

            if(loggedInUser.lastName != model.lastName){
                loggedInUser.lastName = model.lastName;
            }

            if(loggedInUser.genre != model.genre){
                loggedInUser.genre = model.genre;
            }


            await this.userManager.UpdateAsync(loggedInUser);
            await this.signInManager.RefreshSignInAsync(loggedInUser);

            return RedirectToAction("Profile");
            
        }
         
        public async Task<IActionResult> Profile(){
            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            return View(loggedInUser);
        }

        public async Task<IActionResult> ChangeProfile(){
            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            ProfileModel model = new ProfileModel()
            {
                firstName = loggedInUser.firstName,
                username = loggedInUser.UserName,
                lastName = loggedInUser.lastName,
                email = loggedInUser.Email,
                genre = loggedInUser.genre
            };
            return View(model);
        }

        public IActionResult CreateAuction(){

            CreateAuctionModel model = new CreateAuctionModel()
            {
                openDate = DateTime.UtcNow
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuction ( CreateAuctionModel model ) {
            if ( !ModelState.IsValid ) {
                return View ( model );
            }

            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            if(model.openDate < DateTime.Today){
                ModelState.AddModelError ( "", "Open date not valid!" );
                    return View ( model );
            }

            if(model.openDate >= model.closeDate){
                ModelState.AddModelError ( "", "Open date and Close date not valid!" );
                    return View ( model );
            }
            
            if(!Microsoft.VisualBasic.Information.IsNumeric(model.startPrice)){
                ModelState.AddModelError ( "", "Start price not valid!" );
                    return View ( model );
            }

            using ( BinaryReader reader = new BinaryReader ( model.image.OpenReadStream ( ) ) ) {
                Auction auction = new Auction ( ) {
                    name = model.name,
                    description = model.description,
                    startPrice = Convert.ToInt32(model.startPrice),
                    currentPrice = Convert.ToInt32(model.startPrice),
                    createDate = DateTime.Now,
                    openDate = model.openDate,
                    closeDate = model.closeDate,
                    state = "DRAFT",
                    owner = loggedInUser,
                    winner = null,
                    image = reader.ReadBytes ( Convert.ToInt32 ( reader.BaseStream.Length ) )
                };
            
            await this.context.Auctions.AddAsync ( auction);

            await this.context.SaveChangesAsync ( );

            return RedirectToAction ( nameof ( HomeController.Index ), "Home" );
            }
        
        }

        public async Task<IActionResult> MyAuctions(int? page){
            User loggedInUser = await this.userManager.GetUserAsync(base.User);
            
            UserListModel model = new UserListModel();

            IList<Auction> list = await this.context.Auctions.Include(t => t.owner).Where(t => t.owner == loggedInUser).OrderByDescending(t => t.createDate).ToListAsync();

            model.auctionList = list.ToPagedList(page ?? 1,3);
           
            return View(model);
        }

         public async Task<IActionResult> EditAuction(int id){
            User loggedInUser = await this.userManager.GetUserAsync(base.User);

            Auction auction = this.context.Auctions.FirstOrDefault(s => s.Id == id);

            if(auction != null && auction.owner == loggedInUser){
                EditAuctionModel model = new EditAuctionModel()
                {
                    name = auction.name,
                    description = auction.description,
                    startPrice = Convert.ToString(auction.startPrice),
                    openDate = auction.openDate,
                    closeDate = auction.closeDate,
                    auctionId = auction.Id
                };

                
                

                return View(model);
            }

            return View();
            
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuction ( EditAuctionModel model ) {


            Auction auction = this.context.Auctions.FirstOrDefault(s => s.Id == model.auctionId);

            if(auction.name != model.name){
                auction.name = model.name;
            }

            if(auction.description != model.description){
                auction.description = model.description;
            }

            if(auction.startPrice != Convert.ToInt32(model.startPrice)){
                if(!Microsoft.VisualBasic.Information.IsNumeric(model.startPrice)){
                    ModelState.AddModelError ( "", "Start price not valid!" );
                    return View ( model );
                }
                auction.startPrice = Convert.ToInt32(model.startPrice);
                auction.currentPrice = Convert.ToInt32(model.startPrice);
            }

            if(auction.openDate != model.openDate){
                if(model.openDate < DateTime.Today){
                    ModelState.AddModelError ( "", "Open date not valid!" );
                    return View ( model );
                }
                if(auction.closeDate != model.closeDate){
                    if(model.openDate >= model.closeDate){
                        ModelState.AddModelError ( "", "Open date and close date not valid!" );
                        return View ( model );
                    }
                }else{
                    if(model.openDate >= auction.closeDate){
                        ModelState.AddModelError ( "", "New open date and auction close date not valid!" );
                        return View ( model );
                    }
                }
                auction.openDate = model.openDate;
            }

            if(auction.closeDate != model.closeDate){
                auction.closeDate = model.closeDate;
            }
            if(model.image != null){
                using ( BinaryReader reader = new BinaryReader ( model.image.OpenReadStream ( ) ) ) {
                    auction.image = reader.ReadBytes ( Convert.ToInt32 ( reader.BaseStream.Length ) );
                };
            }

            await this.context.SaveChangesAsync ( );


            return RedirectToAction(nameof(UserController.MyAuctions), "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseBid(int? auctionId){


            Auction auction = await this.context.Auctions.Include(a => a.winner).Where(a => a.Id == (int)auctionId).FirstOrDefaultAsync();

            if(auction != null){
                if(auction.winner != null){
                    auction.state = "SOLD";
                }else{
                    auction.state = "EXPIRED";
                }
            }

            try{
                this.context.Update(auction);
                this.context.SaveChanges( );
            }
            catch(DbUpdateConcurrencyException e){
                await Console.Out.WriteLineAsync(e.ToString());
                return Json(new { flag = false });

            }

            await this.context.SaveChangesAsync();

            return Json(new {
                flag = true
            });      
        }

    }
}