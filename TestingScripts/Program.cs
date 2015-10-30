using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_DMD.Classes;
using Project_DMD.Models;

namespace TestingScripts
{
    class Program
    {
        static void Main(string[] args)
        {
            AppUser user = new AppUser()
            {
                Email = "alikhil@mail.ru",
                FirstName = "azaz",
                LastName = "khil",
                PasswordHash = "ASDasdadasd",
                UserName = "alikhil@mail.ru",
                Id = "228"
            };
            var command = AutoSqlGenerator.Instance.Add(user);
            var dictionary = AutoSqlGenerator.Instance.ExecuteCommand("select * from client;");
            
            Console.WriteLine(dictionary);
            Console.ReadKey();
            /*
            Console.WriteLine(command);
            Console.ReadKey();
            var dict = new List<KeyValuePair<string, string>>();
            dict.Add(new KeyValuePair<string, string>("lastname", "'khil'"));
            dict.Add(new KeyValuePair<string, string>("firstname", "'azaz'"));
            var res = AutoSqlGenerator.Instance.Get<AppUser>(command);
            Console.WriteLine(res);
            Console.ReadKey();
             * */
        }
    }
}
