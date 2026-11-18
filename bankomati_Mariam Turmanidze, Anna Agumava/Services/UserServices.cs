using bankomati_Mariam_Turmanidze__Anna_Agumava.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace bankomati_Mariam_Turmanidze__Anna_Agumava.Services
{
    public class UserServices
    {
        private readonly string _path = "Data/users.txt"; // ფაილს ქმნის

        public UserServices() // კონსტრუქტორი , თუ ფაილი არ არსებობს მაშინ ახალს შექმნის
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            if (!File.Exists(_path))
            {
                File.Create(_path).Close();
            }
        }

        public List<User> GetAll() // ფაილიდან წამოიღებს ყველაფერს
        {
            var lines = File.ReadAllLines(_path);
            var users = new List<User>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                users.Add(User.Parse(line));
            }
            return users;
        }

        public void SaveAll(List<User> users) // ფაილში ჩაწერს ყველაფერს
        {
            File.WriteAllLines(_path, users.Select(u => u.ToString()));
        }

        public bool Register(User user) // რეგისტრაცია
        {
            var users = GetAll();
            if (users.Any(u => u.Username == user.Username))
            {
                return false; // მომხმარებელი უკვე თუ არსებობს
            }
            users.Add(user);
            SaveAll(users);
            return true; // თუ არ არსებობს ახლიდან ნაემატება
        }

        public User? Login(string username, string password) // შესვლა
        {
            var users = GetAll();
            foreach (var user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user; // მომხმარებელი შევიდა
                }
            }
            return null; // მომხმარებელი ვერ იპოვა
        }

        public void UpdateUser(User updatedUser)
        {
            var users = GetAll();
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Username == updatedUser.Username)
                {
                    users[i] = updatedUser;
                    break;
                }
            }
            SaveAll(users);
        }
        public bool DeleteUser(string username)
        {
            var users = GetAll();
            int removed = users.RemoveAll(u => u.Username == username);
            if (removed == 0) return false;

            SaveAll(users);
            return true;
        }
    }
}
