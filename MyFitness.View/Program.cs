using MyFitness.BL.Controllers;
using MyFitness.BL.Models;

namespace MyFitness.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MyFitness was started!");

            Console.WriteLine("Enter username: ");
            var username = Console.ReadLine();

            Console.WriteLine("Enter gender: ");
            var gender = Console.ReadLine();

            Console.WriteLine("Enter date of birth: ");
            //DateTime birthday;
            //if (DateTime.TryParse(Console.ReadLine(), out birthday)) 
            //    throw new ArgumentException("Incorrect date of birth.", nameof(birthday));
            var birthday = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Enter weight: ");
            var weight = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter height: ");
            var height = Convert.ToInt32(Console.ReadLine());

            var userController = new UserController(username, gender, birthday, weight, height);

            userController.SaveUser();
        }
    }
}