using System;
using System.Linq;
using System.Web.Mvc;
using MvcAffableBean.DAL;
using MvcAffableBean.Models;
using System.Data.Entity;

namespace MvcAffableBean.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {

        private MvcAffableBeanContext db = new MvcAffableBeanContext();
        const String PromoCode = "FREE";
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult AddressAndPayment(FormCollection values)
        //{
        //    var order = new CustomerOrder();

        //    TryUpdateModel(order);

        //    try
        //    {
        //        //if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == false)
        //        //{
        //        //    return View(order);
        //        //}
        //        //else
        //        //{
        //            order.CustomerUserName = User.Identity.Name;
        //            order.DateCreated = DateTime.Now;

        //            db.CustomerOrders.Add(order);
        //            db.SaveChanges();

        //            var cart = ShoppingCart.GetCart(this.HttpContext);
        //            cart.CreateOrder(order);

        //            db.SaveChanges();//we have received the total amount lets update it

        //            return RedirectToAction("Complete", new {id = order.Id});
        //        //}
        //    }
        //    catch(Exception ex)
        //    {
        //        ex.InnerException.ToString();
        //        return View(order);
        //    }
        //}

        [HttpPost]
        public PartialViewResult PlaceOrder(OrderDetail  deliverydetail)
        {          
            var order = new Customer();
           // TryUpdateModel(order);

            try
            {               
                bool isValid=false;
                var existingcustomer = db.Customers.Where(x => x.UserId == User.Identity.Name && x.Address=="").FirstOrDefault();
                if (existingcustomer != null)
                {
                    existingcustomer.Address = deliverydetail.Address;
                    db.Entry(existingcustomer).State = EntityState.Modified;
                    db.SaveChanges();
                }
                    OrderDetail orderdetail = new OrderDetail
                    {
                        UserId = User.Identity.Name,
                        Address = deliverydetail.Address,
                        CookingInstruction = deliverydetail.CookingInstruction,
                        CreatedBy = User.Identity.Name,
                        CreatedDate = deliverydetail.CreatedDate,
                        Amount = deliverydetail.Amount,
                        DeliveryStatus=1
                    };
                    db.OrderDetail.Add(orderdetail);
                    db.SaveChanges();
                    if(orderdetail.Id>0)
                    {
                    var usercart = db.Carts.Where(x => x.CartId == User.Identity.Name && x.OrderId == 0).ToList();
                    if(usercart!=null)
                    {
                        foreach(var item in usercart)
                        {
                            var c = db.Carts.Where(x => x.CartId.Equals(item.CartId)&& x.OrderId==0).FirstOrDefault();
                            if (c != null)
                            {
                                c.OrderId = orderdetail.Id;
                                db.SaveChanges();
                            }
                        } 
                    }
                    
                    isValid = db.OrderDetail.Any(
                                        o => o.Id == orderdetail.Id &&
                                             o.UserId == User.Identity.Name
                                        ); 
                    }
                return isValid ? PartialView("_OrderConfirmationView") : PartialView("_Error");                 
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                // return View(order);
                return PartialView("_Error"); 
            }
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.OrderDetail.Any(
                o => o.Id == id &&
                     o.UserId == User.Identity.Name
                );

            if (isValid)
            {
                return View("_OrderConfirmationView", id);
            }
            else
            {
                return View("Error");
            }
        } 


    }
}