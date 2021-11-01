using IepProjekat.Models.Database;
using Microsoft.AspNetCore.Identity;

namespace IepProjekat.Models.Initializers {
    public class UserInitializer {

        private static string[] administrator = new string [] {
            "Admin","Admin","admin","admin@microsoft.com","admin12345", "Man"
        };

        private static bool addUser ( string[] row, UserManager<User> userManager, string role ) {
            string firstName = row[0];
            string lastName = row[1];
            string username = row[2];
            string email = row[3];
            string password = row[4];
            string genre = row[5];

            if ( userManager.FindByNameAsync ( username ).Result != null ) {
                return false;
            }

            User user = new User ( ) {
                firstName = firstName,
                lastName = lastName,
                UserName = username,
                Email = email,
                genre =genre
            };

            IdentityResult result = userManager.CreateAsync ( user, password ).Result;
            if ( !result.Succeeded ) {
                return false;
            }

            result = userManager.AddToRoleAsync ( user, role ).Result;

            if ( !result.Succeeded ) {
                return false;
            }

            return true;
        }

        public static void initialize  ( UserManager<User> userManager ) {
            addUser ( UserInitializer.administrator, userManager, Roles.administrator.Name );
        }
    }
}