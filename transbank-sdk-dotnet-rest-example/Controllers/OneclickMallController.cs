using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Transbank.Webpay.Common;
using Transbank.Webpay.Oneclick;

namespace transbanksdkdotnetrestexample.Controllers
{
    public class OneclickMallController : Controller
    {
        public ActionResult InscriptionStart()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("InscriptionFinish", "OneclickMall", null, Request.Url.Scheme);
            var userName = "goncafa";
            var email = $"{RandomString(5)}@{RandomString(5)}.com";

            ViewBag.UserName = userName;
            ViewBag.Email = email;
            ViewBag.ReturnUrl = returnUrl;

            var response = Inscription.Start(userName, email, returnUrl);
            ViewBag.Result = response;
            
            ViewBag.Action = response.Url;
            ViewBag.Token = response.Token;
            
            return View();
        }
        
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult InscriptionFinish()
        {
            var token = Request.Form["TBK_TOKEN"];
            ViewBag.Token = token;

            var result = Inscription.Finish(token);

            ViewBag.AuthorizationCode = result.AuthorizationCode;
            ViewBag.ResponseCode = result.ResponseCode;
            ViewBag.TbkUser = result.TbkUser;
            ViewBag.CreditCardType = result.CreditCardType;
            ViewBag.LastFourCardDigits = result.LastFourCardDigits;
            ViewBag.Result = result;
            
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("TransactionAuthorize", "OneclickMall", null, Request.Url.Scheme);

            return View();
        }

        public ActionResult TransactionAuthorize()
        {
            var userName = Request.Form["user_name"];
            var tbkUser = Request.Form["tbk_user"];
            var buyOrder = RandomString(10);
            
            var childCommerceCode = "597055555542";
            var childBuyOrder = RandomString(10);
            var amount = Decimal.Parse(Request.Form["amount"]);
            var installmentsNumber = 1;

            List<PaymentRequest> details = new List<PaymentRequest>();
            details.Add(new PaymentRequest(childCommerceCode, childBuyOrder, amount, installmentsNumber));

            var result = MallTransaction.Authorize(userName, tbkUser, buyOrder, details);
            Console.WriteLine(result);

            ViewBag.UserName = userName;
            ViewBag.TbkUser = tbkUser;
            ViewBag.BuyOrder = buyOrder;
            ViewBag.Details = details.First();
            ViewBag.Result = result;
            
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("TransactionRefund", "OneclickMall", null, Request.Url.Scheme);

            return View();
        }
        
        public ActionResult InscriptionDelete()
        {
            var userName = Request.Form["user_name"];
            var tbkUser = Request.Form["TBK_TOKEN"];

            Inscription.Delete(userName, tbkUser);

            ViewBag.UserName = userName;
            ViewBag.TbkUser = tbkUser;

            return View();
        }
        
        public ActionResult TransactionRefund()
        {
            
            var buyOrder = Request.Form["buy_order"];
            var childCommerceCode = Request.Form["child_commerce_code"];
            var childBuyOrder = Request.Form["child_buy_order"];
            var amount = decimal.Parse(Request.Form["amount"]);
            var token = Request.Form["TBK_TOKEN"];
            var userName = Request.Form["user_name"];

            var result = MallTransaction.Refund(buyOrder, childCommerceCode,childBuyOrder,amount);
            Console.WriteLine(result);

            ViewBag.BuyOrder = buyOrder;
            ViewBag.ChildCommerceCode = childCommerceCode;
            ViewBag.ChildBuyOrder = childBuyOrder;
            ViewBag.Amount = amount;
            ViewBag.Result = result;
            ViewBag.Token = token;
            ViewBag.UserName = userName;
            
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("InscriptionDelete", "OneclickMall", null, Request.Url.Scheme);

            return View();
        }
    }
}