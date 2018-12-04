using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAffableBean.DAL;
using System.Data.Entity;

namespace MvcAffableBean.Models
{
    public partial class ShoppingCart
    {
        MvcAffableBeanContext db = new MvcAffableBeanContext();

        public string ShoppingCartId { get; set; }

        public const string CartSessionKey = "cartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();

            cart.ShoppingCartId = cart.GetCartId(context);

            return cart;
        }

        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Product product,bool ? Navflag)
        {
            var cartItem = db.Carts.SingleOrDefault(c=>c.CartId == ShoppingCartId && c.ProductId == product.Id && c.OrderId==0);
            

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = product.Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    Price = product.Price,
                    DateCreated = DateTime.Now


                };
                db.Carts.Add(cartItem);
            }
            else
            {
                if (Navflag == true)
                {
                    cartItem.Count++;
                    cartItem.Price = cartItem.Price + product.Price;
                }
                else if (Navflag == false)
                {
                    cartItem.Count--;
                    cartItem.Price = cartItem.Price - product.Price;
                }
                else
                {
                    cartItem.Count++;
                    cartItem.Price = cartItem.Price + product.Price;
                }
            }

            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = db.Carts.SingleOrDefault(cart => cart.CartId == ShoppingCartId && cart.ProductId == id && cart.OrderId==0);
            int itemCount = 0;

            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                //if (cartItem.Count > 1)
                //{
                //    cartItem.Count--;
                //    itemCount = cartItem.Count;
                //}
                //else
                //{
                //    db.Carts.Remove(cartItem);
                //}

                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId && cart.OrderId==0);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId && cart.OrderId ==0).ToList();
        }

        public int GetCount()
        {
            int? count =
                (from cartItems in db.Carts where cartItems.CartId == ShoppingCartId && cartItems.OrderId==0 select (int?) cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in db.Carts
                where cartItems.CartId == ShoppingCartId && cartItems.OrderId==0
                select (int?) cartItems.Count*cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(OrderDetail orderdetail)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderedProduct = new OrderedProduct
                {
                    ProductId = item.ProductId,
                    CustomerOrderId = orderdetail.Id,
                    Quantity = item.Count,
                    Amount=item.Price
                };
                
                orderTotal += (item.Count * item.Product.Price);

                db.Orderedproducts.Add(orderedProduct);
            }
            orderdetail.Amount = orderTotal;
            db.Entry(orderdetail).State = EntityState.Modified;
            db.SaveChanges();
            EmptyCart();

            return orderdetail.Id;
        }

        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }

                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }

    }
}