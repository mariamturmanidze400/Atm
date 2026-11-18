using System;
using System.Collections.Generic;
using System.Text;

namespace bankomati_Mariam_Turmanidze__Anna_Agumava.Services
{
    internal class ATMService
    {
        private readonly UserServices userServices;

        public ATMService(UserServices userServices)
        {
            this.userServices = userServices;
        }

        public decimal GetBalance(string username)
        {
            var all = userServices.GetAll();
            foreach (var user in all)
            {
                if (user.Username == username)
                {
                    return user.Balance;
                }
            }
            return 0;
        }
    }
}
