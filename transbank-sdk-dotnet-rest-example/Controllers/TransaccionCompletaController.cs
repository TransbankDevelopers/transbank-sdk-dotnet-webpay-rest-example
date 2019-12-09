using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Transbank.Webpay.TransaccionCompleta;
using Transbank.Webpay.TransaccionCompletaMall;
using Transbank.Webpay.TransaccionCompletaMall.Common;
using Newtonsoft.Json;

namespace transbanksdkdotnetrestexample.Controllers
{
    public class TransaccionCompletaController : Controller
    {
        #region Transaccion Completa
        public ActionResult Create()
        {
            var random = new Random();

            var buy_order = random.Next(999999999).ToString();
            var session_id = random.Next(9999999).ToString();
            var amount = 10000;
            var cvv = 123;
            var card_number = "4051885600446623";
            var card_expiration_date = "22/10";
            
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Installments", "TransaccionCompleta", null, Request.Url.Scheme);

            var result = FullTransaction.Create(
                buyOrder: buy_order,
                sessionId: session_id,
                amount: amount,
                cvv: cvv,
                cardNumber: card_number,
                cardExpirationDate: card_expiration_date);

            ViewBag.Action = returnUrl;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Token = result.Token;
            ViewBag.Result = result;

            ViewBag.BuyOrder = buy_order;
            ViewBag.SessionId = session_id;
            ViewBag.Amount = amount;
            ViewBag.Cvv = cvv;
            ViewBag.CardNumber = card_number;
            ViewBag.CardExpirationDate = card_expiration_date;

            return View();
        }
        
        [HttpPost]
        public ActionResult Installments()
        {
            var token = Request.Form["token_ws"];
            var installments_number = 10;
            ViewBag.Token = token;
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.SaveToken = token;
            var returnUrl = urlHelper.Action("Commit", "TransaccionCompleta", null, Request.Url.Scheme);
            ViewBag.Action = returnUrl;
            var result = FullTransaction.Installments(
                token,
                installments_number);
            ViewBag.InstallmentsNumber = installments_number;
            ViewBag.SaveIdQueryInstallments = result.IdQueryInstallments.ToString();
            ViewBag.Result = result;
            ViewBag.ReturnUrl = returnUrl;

            return View();

        }
        [HttpPost]
        public ActionResult Commit()
        {
            var token = Request.Form["token_ws"];
            var idQueryInstallments = int.Parse(Request.Form["id_query_installments"]);
            var deferredPeriodsIndex = 10;
            var gracePeriods = false;

            var result = FullTransaction.Commit(token, idQueryInstallments, deferredPeriodsIndex, gracePeriods);
            
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Status", "TransaccionCompleta", null, Request.Url.Scheme);


            ViewBag.SaveIdQueryInstallments = idQueryInstallments;
            ViewBag.DeferredPeriodIndex = deferredPeriodsIndex;
            ViewBag.GracePrediods = gracePeriods;
            ViewBag.Action = returnUrl;
            ViewBag.Token = token;
            ViewBag.Result = result;

            return View();
        }
        
        [HttpPost]
        public ActionResult Status()
        {
            var token = Request.Form["token_ws"];

            var result = FullTransaction.Status(token);
            
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Refund", "TransaccionCompleta", null, Request.Url.Scheme);

            ViewBag.Action = returnUrl;
            ViewBag.Token = token;
            ViewBag.Result = result;

            return View();
        }

        [HttpPost]
        public ActionResult Refund()
        {
            var token = Request.Form["token_ws"];
            var amount = 10000;

            var result = FullTransaction.Refund(token, amount);
            
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Status", "TransaccionCompleta", null, Request.Url.Scheme);

            ViewBag.Amount = amount;
            ViewBag.Action = returnUrl;
            ViewBag.Token = token;
            ViewBag.Result = result;

            return View();
        }


        #endregion

        #region Transaccion Completa mall
        public ActionResult MallCreate()
        {
            var random = new Random();

            var buy_order = random.Next(999999999).ToString();
            var session_id = random.Next(9999999).ToString();
            var amount = 10000;
            var cvv = 123;
            var card_number = "4051885600446623";
            var card_expiration_date = "22/10";
            var details = new List<CreateDetails>();
            details.Add(new CreateDetails(
                2000,
                "597055555552",
                "3856061"
            ));
            details.Add(new CreateDetails(
                1000,
                "597055555553",
                "3874147"
            ));

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("MallInstallments", "TransaccionCompleta", null, Request.Url.Scheme);

            var result = MallFullTransaction.Create(
                buyOrder: buy_order,             
                sessionId: session_id,
                cardNumber: card_number,
                cardExpirationDate: card_expiration_date,
                details:details
                );

            ViewBag.Action = returnUrl;
            ViewBag.Result = result;
            ViewBag.Details = details;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Token = result.Token;
            ViewBag.BuyOrder = buy_order;
            ViewBag.SessionId = session_id;
            ViewBag.Amount = amount;
            ViewBag.Cvv = cvv;
            ViewBag.CardNumber = card_number;
            ViewBag.CardExpirationDate = card_expiration_date;

            return View();
        }

        [HttpPost]
        public ActionResult MallInstallments()
        {
            var token = Request.Form["token_ws"];
            var details = Request.Form["details"];

            var InstallmentsDetails = new List<MallInstallmentsDetails>();
            InstallmentsDetails.Add(new MallInstallmentsDetails(
                "597055555552",
                "3856061",
                5
            ));
            InstallmentsDetails.Add(new MallInstallmentsDetails(
                "597055555553",
                "3874147",
                10
            ));
            ViewBag.Token = token;
            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            ViewBag.SaveToken = token;
            var returnUrl = urlHelper.Action("MallCommit", "TransaccionCompleta", null, Request.Url.Scheme);

            ViewBag.Action = returnUrl;
            var result = MallFullTransaction.Installments(
                token,
                InstallmentsDetails
                );
            //JsonConvert.DeserializeObject<MallInstallmentsDetailsResponse>(result);
            //ViewBag.InstallmentsNumber = result.[0].installments_number;
            //ViewBag.SaveIdQueryInstallments = result.IdQueryInstallments.ToString();
            ViewBag.Result = result;
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
        [HttpPost]
        public ActionResult MallCommit()
        {
            var token = Request.Form["token_ws"];
            var idQueryInstallments = int.Parse(Request.Form["id_query_installments"]);
            var deferredPeriodsIndex = 10;
            var gracePeriods = false;

            var result = FullTransaction.Commit(token, idQueryInstallments, deferredPeriodsIndex, gracePeriods);

            UrlHelper urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Status", "TransaccionCompleta", null, Request.Url.Scheme);


            ViewBag.SaveIdQueryInstallments = idQueryInstallments;
            ViewBag.DeferredPeriodIndex = deferredPeriodsIndex;
            ViewBag.GracePrediods = gracePeriods;
            ViewBag.Action = returnUrl;
            ViewBag.Token = token;
            ViewBag.Result = result;

            return View();
        }

        [HttpPost]
        public ActionResult MallStatus()
        {
            var token = Request.Form["token_ws"];

            var result = FullTransaction.Status(token);

            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Refund", "TransaccionCompleta", null, Request.Url.Scheme);

            ViewBag.Action = returnUrl;
            ViewBag.Token = token;
            ViewBag.Result = result;

            return View();
        }

        [HttpPost]
        public ActionResult MallRefund()
        {
            var token = Request.Form["token_ws"];
            var amount = 10000;

            var result = FullTransaction.Refund(token, amount);

            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var returnUrl = urlHelper.Action("Status", "TransaccionCompleta", null, Request.Url.Scheme);

            ViewBag.Amount = amount;
            ViewBag.Action = returnUrl;
            ViewBag.Token = token;
            ViewBag.Result = result;

            return View();
        }


        #endregion



    }
}