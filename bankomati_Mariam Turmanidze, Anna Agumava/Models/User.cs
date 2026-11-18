using System;
using System.Collections.Generic;
using System.Text;

namespace bankomati_Mariam_Turmanidze__Anna_Agumava.Models
{
    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "client"; // default - Client
        public decimal Balance { get; set; }

        public override string ToString() => $"{Username}|{Password}|{Role}|{Balance}"; // string-ად გადაყავს ყველა და |-ით ყოფს ერთმანეთისგან
        public static User Parse(string line) // ობიექტს ქმნის სტრინგიდან რომელიც ფაილშია შენახული
        {
            var p = line.Split('|');

            return new User
            {
                Username = p[0],
                Password = p[1],
                Role = p[2],
                Balance = decimal.Parse(p[3])
            };
        }
    }
}
