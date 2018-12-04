using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MvcAffableBean.Models;
using MvcAffableBean.DAL;


namespace MvcAffableBean.Controllers
{
    public class AccountsController : Controller
    {
        private MvcAffableBeanContext db = new MvcAffableBeanContext();
        CustomerDetail objcustomerdetail = new CustomerDetail();
        private void MigrateShoppingCart(string userName)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(userName);
            Session[ShoppingCart.CartSessionKey] = userName;
        }

        public ActionResult LogOn()
        {
             //return View();
           return PartialView("~/Views/Accounts/_Logon.cshtml");
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    MigrateShoppingCart(model.UserName);

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "ShoppingCart");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart();

            return RedirectToAction("Index", "Home");
        }
                
        public ActionResult Register(string mobile)
        {
            int customerid = 0;
            if (mobile !=null)
            {
                var objcustomer = new Customer
                {UserId=mobile};
                customerid = objcustomerdetail.SaveMobileNumber(objcustomer);
            }
            string[] mobiledetail = { customerid.ToString(), mobile };
            ViewData["MobileDetail"]= mobile;
            if(customerid != 0)//for new user 
            return PartialView("~/Views/Accounts/_Register.cshtml", mobile);
            else //for existing user
                return PartialView("~/Views/Accounts/_Logon.cshtml");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registration,Customer customerdetail)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(registration.UserName, registration.Password, registration.Email, "question", "answer", true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    MigrateShoppingCart(registration.UserName);

                    FormsAuthentication.SetAuthCookie(registration.UserName, false /* createPersistentCookie */);
                    
                    objcustomerdetail.SaveCustomerDetail(customerdetail);
                    Session["UserName"] = registration.UserName;
                    return RedirectToAction("LoadDeliveryDetailsView", "Accounts");
               //return PartialView("~/ Views / Accounts / DeliveryView.cshtml");
            }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("~/Views/Accounts/_Register.cshtml", registration);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult LoadMobileRegistrationView()
        {
            var deliveryaddress = "";
            var existingcustomer = db.Customers.Where(x => x.UserId == User.Identity.Name && x.Address != "").FirstOrDefault();
            if (existingcustomer != null)
            {
                deliveryaddress = existingcustomer.Address.ToString();
                ViewData["address"] = deliveryaddress;
            }
            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                return PartialView("~/Views/Accounts/DeliveryView.cshtml");
            }
            else { return PartialView("~/Views/Accounts/_MobileRegistration.cshtml"); }
        }
            

        public ActionResult LoadDeliveryDetailsView()
        {
            return PartialView("~/Views/Accounts/DeliveryView.cshtml");
        }

        public ActionResult LoadRegistrationView(decimal? amount)
        {
            return View("Register.cshtml");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
