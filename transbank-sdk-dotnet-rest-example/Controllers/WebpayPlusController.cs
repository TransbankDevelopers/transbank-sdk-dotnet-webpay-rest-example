using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Transbank.Webpay.Common;
using Transbank.Webpay.WebpayPlus;
using Transbank.Exceptions;

namespace transbanksdkdotnetrestexample.Controllers
{
    public class WebpayPlusController : Controller
    {
        public ActionResult NormalCreate()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("NormalReturn", "WebpayPlus", null, Request.Url.Scheme);

            string buyOrder = new Random().Next(int.MaxValue).ToString();
            string sessionId = new Random().Next(int.MaxValue).ToString();
            var amount = new Random().Next(1, 99999);

            ViewBag.buyOrder = buyOrder;
            ViewBag.SessionId = sessionId;
            ViewBag.Amount = amount;
            ViewBag.ReturnUrl = returnUrl;

            try
            {
                var response = Transaction.Create(buyOrder, sessionId, amount, returnUrl);
                string token = response.Token;
                string url = response.Url;
                ViewBag.Result = response.ToString();

                ViewBag.Action = response.Url;
                ViewBag.Token = response.Token;
            }
            catch (TransactionCreateException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return View();
        }

        [HttpPost]
        public ActionResult NormalReturn()
        {
            var token = Request.Form["token_ws"];
            ViewBag.Token = token;

            var result = Transaction.Commit(token);

            ViewBag.Result = result;
            ViewBag.ResponseCode = result.ResponseCode;
            return View();
        }

        public ActionResult NormalRefund()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteRefund", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteRefund()
        {
            return View();
        }


        public ActionResult NormalStatus()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteStatus", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteStatus()
        {
            return View();
        }

        public ActionResult DeferredCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeferredReturn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteDeferredCapture()
        {
            return View();
        }

        public ActionResult DeferredRefund()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteDeferredRefund", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteDeferredRefund()
        {
            return View();
        }

        public ActionResult DeferredStatus()
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteDeferredStatus", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteDeferredStatus()
        {
            return View();
        }

        public ActionResult MallCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MallReturn()
        {
            return View();
        }

        public ActionResult MallRefund()
        {
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteMallRefund", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteMallRefund()
        {
            return View();
        }

        public ActionResult MallStatus()
        {
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteMallStatus", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        public ActionResult ExecuteMallStatus()
        {
            return View();
        }

        public ActionResult MallDeferredCreate()
        {
            return View();
        }
        
        public ActionResult MallDeferredCommit()
        {
            return View();
        }
        
        public ActionResult ExecuteMallDeferredCapture()
        {
            return View();
        }

        public ActionResult ExecuteMallDeferredRefund()
        {
            return View();
        }

        public ActionResult ExecuteMallDeferredStatus()
        {
            return View();
        }
    }
}