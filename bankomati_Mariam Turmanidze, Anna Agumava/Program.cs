using bankomati_Mariam_Turmanidze__Anna_Agumava.Models;
using bankomati_Mariam_Turmanidze__Anna_Agumava.Services;

var userService = new UserServices();
var atmService = new ATMService(userService);
User? currentUser = null;

while (true)
{
    Console.WriteLine("\n1. Account\n2. Balance\n0. Exit"); // მთავარი მენიუ
    Console.Write("Choice: ");
    string MainChoice = Console.ReadLine() ?? "";

    if (MainChoice == "0") break;

    if (MainChoice == "1")
    {
        if (currentUser == null)
            GuestAccountMenu();
        else
            LoggedInAccountMenu();
    }
    else if (MainChoice == "2")
    {
        if (currentUser == null)
        {
            Console.WriteLine("Please log in first.");
        }
        else
        {
            BalanceMenu(currentUser);
        }
    }
    else
    {
        Console.WriteLine("Invalid choice. Enter 1, 2 or 0.");
    }
}

void GuestAccountMenu() //რეგისტრაცია და შესვლა
{
    while (true)
    {
        Console.WriteLine("\n1. Register\n2. Log In\n0. Back");
        Console.Write("Choice: ");
        string accChoice = Console.ReadLine() ?? "";

        if (accChoice == "0") return;

        if (accChoice == "1")
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            string role;
            while (true)
            {
                Console.Write("Role (client/admin): ");
                role = (Console.ReadLine() ?? "").Trim().ToLower();

                if (role == "client" || role == "admin") break;

                Console.WriteLine("Invalid role. Enter only 'client' or 'admin'.");
            }

            var newUser = new User { Username = username, Password = password, Role = role };
            bool success = userService.Register(newUser);

            if (success)
            {
                currentUser = newUser;
                Console.WriteLine($"Successfully registered. Welcome, {newUser.Username}! Role: {newUser.Role}");
                return;
            }
            else
            {
                Console.WriteLine("User already exists");
            }
        }
        else if (accChoice == "2")
        {
            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            User? user = userService.Login(username, password);

            if (user == null)
            {
                Console.WriteLine("Invalid username or password. Try again.");
                continue;
            }

            currentUser = user;
            Console.WriteLine($"Welcome, {user.Username}! Role: {user.Role}");
            return;
        }
        else
        {
            Console.WriteLine("Invalid choice. Enter 1, 2 or 0.");
        }
    }
}

void LoggedInAccountMenu() // მხოლოდ წაშლა და გამოსვლა

{
    while (true)
    {
        Console.WriteLine("\n1. Delete Account\n2. Log Out\n0. Back");
        Console.Write("Choice: ");
        string accChoice = Console.ReadLine() ?? "";

        if (accChoice == "0") return;

        if (accChoice == "1")
        {
            Console.Write("Enter your password to confirm: ");
            string confirmPassword = Console.ReadLine() ?? "";

            if (confirmPassword != currentUser!.Password)
            {
                Console.WriteLine("Wrong password. Account was not deleted.");
                continue;
            }

            Console.Write("Are you sure you want to delete your account? (y/n): ");
            string answer = (Console.ReadLine() ?? "").Trim().ToLower();

            if (answer == "y")
            {
                userService.DeleteUser(currentUser.Username);
                Console.WriteLine("Account deleted.");
                currentUser = null;
                return;
            }
            else
            {
                Console.WriteLine("Cancelled.");
            }
        }
        else if (accChoice == "2")
        {
            Console.WriteLine($"Goodbye, {currentUser!.Username}!");
            currentUser = null;
            return;
        }
        else
        {
            Console.WriteLine("Invalid choice. Enter 1, 2 or 0.");
        }
    }
}

void BalanceMenu(User user)
{
    Console.WriteLine("\n1. Check Balance\n2. Deposit\n3. Withdraw\n0. Back"); // ბალანსი
    Console.Write("Choice: ");
    string balChoice = Console.ReadLine() ?? "";

    if (balChoice == "1")
    {
        decimal balance = atmService.GetBalance(user.Username);
        Console.WriteLine($"Your balance is: {balance}");
    }
    else if (balChoice == "2")
    {
        Console.Write("Enter amount to deposit: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            user.Balance += amount;
            userService.UpdateUser(user);
            Console.WriteLine($"Successfully deposited {amount}. New balance: {user.Balance}");
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }
    else if (balChoice == "3")
    {
        Console.Write("Enter amount to withdraw: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            if (user.Balance >= amount)
            {
                user.Balance -= amount;
                userService.UpdateUser(user);
                Console.WriteLine($"Successfully withdrew {amount}. New balance: {user.Balance}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Insufficient funds. Your balance is " + user.Balance);
                Console.ResetColor();
            }
        }
        else
        {
            Console.WriteLine("Invalid amount.");
        }
    }
    else if (balChoice != "0")
    {
        Console.WriteLine("Invalid choice. Enter 1, 2, 3 or 0.");
    }
}