using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnCor;
using EnCor.Security;

namespace EnCorTest.AlphaModule
{
    public class AlphaService : BizService
    {
        private IAlphaDataProvider _DataProvider;
        [ActionPermission("GetCustomer")]
        public Customer GetCustomer(int customerID)
        {
            return _DataProvider.GetCustomer(customerID);
        }

        [ActionPermission("UpdateCustomer")]
        public Customer UpdateCustomer(Customer customer)
        {
            return _DataProvider.UpdateCustomer(customer);
        }
    }

    public class Customer
    {
        public int CustomerId;
        public string Name;
        public string Website;
        public string Tel;
    }
}
