using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using System.ComponentModel;
using System.Data;

namespace MyFitness.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MyFitness was started!");

            Console.WriteLine("Enter username: ");
            var username = Console.ReadLine();
            
            var userController = new UserController(username);

            if (userController.IsNewUser)
            {
                Console.WriteLine("Enter gender: ");
                var gender = Console.ReadLine();

                Console.WriteLine("Enter date of birth: ");
                var dateOfBirth = GetParsedValue<DateTime>("date of birth");


                Console.WriteLine("Enter weight: ");
                var weight = GetParsedValue<double>("weight");

                Console.WriteLine("Enter height: ");
                var height = GetParsedValue<double>("height");

                userController.CreateUserData(gender, dateOfBirth, weight, height);
            }

            Console.WriteLine(userController.CurrentUser);
        }

        private static T? GetParsedValue<T>(string paramName)
        {
            while (true)
            {
                var value = Console.ReadLine();
                if (value is null) continue;
                if (TryParse(value, out T? newValue)) return newValue;
                Console.WriteLine($"Incorrect {paramName}. Please, try again:");
            }
        }

        private static bool TryParse<T>(string value, out T? newValue)
        {
            while (true)
            {
                newValue = default;
                try
                {
                    newValue = (T)Convert.ChangeType(value, typeof(T));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        } 
    }
}