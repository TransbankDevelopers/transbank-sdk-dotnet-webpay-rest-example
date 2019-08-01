﻿using System;
using System.Web.Mvc;
using Transbank.Webpay.WebpayPlus;
using Transbank.Webpay.Common;
using System.Collections.Generic;
using System.Linq;
using Transbank.Webpay.WebpayPlus.Responses;

namespace transbanksdkdotnetrestexample.Controllers
{
    public class WebpayPlusController : Controller
    {
        #region Webpay Plus
        public ActionResult NormalCreate()
        {
            var random = new Random();
            var buyOrder = random.Next(999999999).ToString();
            var sessionId = random.Next(999999999).ToString();
            var amount = random.Next(1000, 999999);
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("NormalReturn", "WebpayPlus", null, Request.Url.Scheme);
            var result = Transaction.Create(buyOrder, sessionId, amount, returnUrl);

            ViewBag.BuyOrder = buyOrder;
            ViewBag.SessionId = sessionId;
            ViewBag.Amount = amount;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Result = result;
            ViewBag.Action = result.Url;
            ViewBag.Token = result.Token;
            return View();
        }

        [HttpPost]
        public ActionResult NormalReturn()
        {
            var token = Request.Form["token_ws"];
            var result = Transaction.Commit(token);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);

            ViewBag.Token = token;
            ViewBag.Action = urlHelper.Action("ExecuteRefund", "WebpayPlus", null, Request.Url.Scheme);
            ViewBag.Result = result;
            ViewBag.SaveToken = token;

            return View();
        }

        public ActionResult NormalRefund()
        {
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteRefund", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteRefund()
        {
            var token = Request.Form["token_ws"];
            var refundAmount = 500;
            var result = Transaction.Refund(token, refundAmount);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteStatus", "WebpayPlus", null, Request.Url.Scheme);

            ViewBag.Token = token;
            ViewBag.Amount = refundAmount;
            ViewBag.Result = result;

            return View();
        }


        public ActionResult NormalStatus()
        {
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteStatus", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteStatus()
        {
            var token = Request.Form["token_ws"];
            var result = Transaction.Status(token);

            ViewBag.Result = result;

            return View();
        }

        #endregion Webpay Plus

        #region Webpay Plus Deferred

        public ActionResult DeferredCreate()
        {
            var random = new Random();
            var buyOrder = random.Next(999999999).ToString();
            var sessionId = random.Next(999999999).ToString();
            var amount = random.Next(1000, 999999);
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("DeferredReturn", "WebpayPlus", null, Request.Url.Scheme);
            var result = DeferredTransaction.Create(buyOrder, sessionId, amount, returnUrl);

            ViewBag.BuyOrder = buyOrder;
            ViewBag.SessionId = sessionId;
            ViewBag.Amount = amount;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Result = result;
            ViewBag.Action = result.Url;
            ViewBag.Token = result.Token;
            return View();
        }

        [HttpPost]
        public ActionResult DeferredReturn()
        {
            var token = Request.Form["token_ws"];
            var result = DeferredTransaction.Commit(token);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);

            ViewBag.Token = token;
            ViewBag.Action = urlHelper.Action("ExecuteDeferredCapture", "WebpayPlus", null, Request.Url.Scheme);
            ViewBag.Result = result;
            ViewBag.SaveToken = token;
            ViewBag.SaveAmount = result.Amount.ToString();
            ViewBag.SaveAuthorizationCode = result.AuthorizationCode;

            return View();
        }

        [HttpPost]
        public ActionResult ExecuteDeferredCapture()
        {
            var token = Request.Form["token_ws"];
            var buyOrder = Request.Form["buy_order"];
            var authorizationCode = Request.Form["authorization_code"];
            var captureAmount = decimal.Parse(Request.Form["capture_amount"]);
            var result = DeferredTransaction.Capture(token, buyOrder, authorizationCode, captureAmount);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteDeferredRefund", "WebpayPlus", null, Request.Url.Scheme);
            ViewBag.Token = token;
            ViewBag.SaveToken = token;

            ViewBag.BuyOrder = buyOrder;
            ViewBag.AuthorizationCode = authorizationCode;
            ViewBag.CaptureAmount = captureAmount;
            ViewBag.Result = result;

            return View();
        }

        public ActionResult DeferredRefund()
        {
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteDeferredRefund", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteDeferredRefund()
        {
            var token = Request.Form["token_ws"];
            var amount = decimal.Parse(Request.Form["amount"]);
            var result = DeferredTransaction.Refund(token, amount);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteDeferredStatus", "WebpayPlus", null, Request.Url.Scheme);
            ViewBag.Token = token;

            ViewBag.Result = result;

            return View();
        }

        public ActionResult DeferredStatus(){
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.Action = urlHelper.Action("ExecuteDeferredStatus", "WebpayPlus", null, Request.Url.Scheme);
            return View();
        }

        [HttpPost]
        public ActionResult ExecuteDeferredStatus()
        {
            var token = Request.Form["token_ws"];
            var result = DeferredTransaction.Status(token);

            ViewBag.Result = result;

            return View();
        }

        #endregion Webpay Plus Deferred

        #region Webpay Plus Mall

        public ActionResult MallCreate()
        {
            var random = new Random();
            var buyOrder = random.Next(9999999).ToString();
            var sessionId = random.Next(9999999).ToString();
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("MallReturn", "WebpayPlus", null, Request.Url.Scheme);

            var transactions = new List<TransactionDetail>();
            transactions.Add(new TransactionDetail(
                amount: random.Next(9999999),
                commerceCode: "597055555536",
                buyOrder: random.Next(9999999).ToString()
            ));
            transactions.Add(new TransactionDetail(
                amount: random.Next(9999999),
                commerceCode: "597055555537",
                buyOrder: random.Next(9999999).ToString()
            ));

            var result = MallTransaction.Create(buyOrder, sessionId, returnUrl, transactions);

            ViewBag.Result = result;
            ViewBag.BuyOrder = buyOrder;
            ViewBag.SessionId = sessionId;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Transactions = transactions;
            ViewBag.Action = result.Url;
            ViewBag.Token = result.Token;

            return View();
        }

        [HttpPost]
        public ActionResult MallReturn()
        {
            var token = Request.Form["token_ws"];
            var result = MallTransaction.Commit(token);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);

            ViewBag.Token = token;
            ViewBag.Action = urlHelper.Action("ExecuteMallRefund", "WebpayPlus", null, Request.Url.Scheme);
            ViewBag.Result = result;
            ViewBag.SaveToken = token;
            ViewBag.SaveAmount = result.Details.First().Amount;
            ViewBag.SaveCommerceCode = result.Details.First().CommerceCode;
            ViewBag.SaveBuyOrder = result.Details.First().BuyOrder;

            ViewBag.Success = result.Details.All(detail => detail.ResponseCode == 0);
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
            var token = Request.Form["token_ws"];
            var buyOrder = Request.Form["buy_order"];
            var commerceCode = Request.Form["commerce_code"];
            var amount = decimal.Parse(Request.Form["amount"]);

            ViewBag.SaveAmount = amount;
            ViewBag.SaveCommerceCode = commerceCode;
            ViewBag.SaveToken = token;
            ViewBag.SaveBuyOrder = buyOrder;

            var result = MallTransaction.Refund(token, buyOrder, commerceCode, amount);

            ViewBag.Result = result;

            return View();
        }

        #endregion Webpay Plus Mall
    }
}
