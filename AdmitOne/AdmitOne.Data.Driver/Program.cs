﻿using AdmitOne.Data.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data.Driver
{
    public static class Program
    {
        private static void InitializeTestData(Repository repo)
        {
            repo.Customers = new List<Customer>
            {
                new Customer
                {
                    ID = "123",
                    CompanyName = "Chicken Farmers Inc.",
                    ContactTitle = "Sales Rep",
                    ContactName = "Jose",
                    Address = "123 Watson Ave.",
                    City = "Clawford",
                    PostalCode = "89354",
                    Country = "USA",
                    Phone = "8921234567",
                    Fax = "7891234567",
                }
            }
            .AsQueryable();
        }

        public static void Test_FindCustomer()
        {
            var r = new Repository();

            InitializeTestData(r);

            Customer c = r.FindCustomer("123");

            Debug.Assert(c != null);
        }

        public static void Test_LiveFindCustomer()
        {
            throw new NotImplementedException();

            Debug.Assert(false);
        }

        public static void Main(string[] args)
        {
            try
            {
                Test_FindCustomer();
                Test_LiveFindCustomer();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception! {0}", e.GetType().ToString());
                Console.WriteLine("{0}, {1}", e.TargetSite, e.Message);
            }
            finally
            {
                Console.Write("Press enter to exit...");
                Console.Read();
            }
        }
    }
}
