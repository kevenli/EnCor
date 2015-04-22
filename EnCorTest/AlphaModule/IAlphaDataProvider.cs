using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnCorTest.AlphaModule
{
    public interface IAlphaDataProvider
    {
        Customer GetCustomer(int customerId);

        Customer UpdateCustomer(Customer customer);
    }
}
