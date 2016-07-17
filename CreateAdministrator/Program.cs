using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateAdministrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a Username");
            string username = Console.ReadLine();
            Console.WriteLine("Enter a password");
            string password = Console.ReadLine();
            string passwordSalt = PasswordHelper.GenerateRandomSalt();
            string passwordhash = PasswordHelper.HashPassword(password, passwordSalt);
            Administrator admin = new Administrator
            {
                UserName = username,
                PasswordHash = passwordhash,
                PasswordSalt = passwordSalt
            };
            AdministratorRepository repo = new AdministratorRepository(Properties.Settings.Default.constr);
            repo.SignUp(admin);          
        }
    }
}
