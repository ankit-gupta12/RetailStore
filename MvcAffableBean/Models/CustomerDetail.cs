using MvcAffableBean.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcAffableBean.Models
{
    public class CustomerDetail
    {
        MvcAffableBeanContext db = new MvcAffableBeanContext();
        public void SaveCustomerDetail(Customer customerdetail)
        {
            if(customerdetail !=null)
            {
                //if user exists , update other details.
                var existinguser = db.Customers.Where(x => x.UserId == customerdetail.UserId).FirstOrDefault();
                if(existinguser !=null)
                {
                    existinguser.Address = customerdetail.Address;
                    existinguser.FirstName = customerdetail.FirstName;
                    existinguser.LastName = customerdetail.LastName;
                    //db.Customers.Add(existinguser);
                    db.Entry(existinguser).State = EntityState.Modified;
                    db.SaveChanges();

                }                
                //customerdetail = new Customer
                //{
                //    FirstName = customerdetail.FirstName,
                //    LastName = customerdetail.LastName,                    
                //    UserId = customerdetail.UserId
                //};
               // db.Customers.Add(customerdetail);
                //db.SaveChanges();
            }
        }
        public int SaveMobileNumber(Customer customerdetail)
        {
            int Id=0;
            if(customerdetail !=null)
            {
                var existinguser = db.Customers.Where(x => x.UserId == customerdetail.UserId).FirstOrDefault();
                if (existinguser == null)
                {
                    db.Customers.Add(customerdetail);
                    db.SaveChanges();
                    Id = customerdetail.Id;
                }
            }
            return Id;
        }
    }
}