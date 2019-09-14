using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OMSDataAccessLayer;
using System.Threading;
using System.Net.Mail;
using OrderManagementSystemAPI.Models;

namespace OrderManagementSystemAPI.Controllers
{
    public class BuyersActionsController : ApiController
    {
        [HttpPost]
        [Route("api/BuyersActions/ADDbuyers")]
        public HttpResponseMessage ADDbuyers([FromBody] Buyer buyer)
        {
            try
            {
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    entities.Buyers.Add(buyer);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, buyer);
                    message.Headers.Location = new Uri(Request.RequestUri + buyer.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/BuyersActions/PlaceOrder/{ProID}/{ordQty}")]
        [BasicAuthentication]
        public HttpResponseMessage PlaceOrder(string ProID, int ordQty)
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    Table_OrderDetails od = new Table_OrderDetails();
                    string BuyerID = (from b in entities.Buyers where b.Email == email select b.BuyerID).FirstOrDefault();
                    string BuyerName = (from b in entities.Buyers where b.Email == email select b.BuyerName).FirstOrDefault();
                    if (entities.Buyers.Any(buyer => buyer.BuyerID.Equals(BuyerID) && buyer.BuyerName.Equals(BuyerName) || BuyerID != null || BuyerName != null))
                    {
                        if (entities.Table_ProductsDetails.Any(pro => pro.ProductID.Equals(ProID)))
                        {

                            od.OrderID = "OD" + BuyerID;
                            od.OrderItemID = "OD" + BuyerID + ProID;
                            if (entities.Table_OrderDetails.Any(odrs => odrs.OrderItemID.Equals(od.OrderItemID) && odrs.OrderStatus != "Completed"))
                            {
                                string messg = "Product Already present in Previous Order Please Update Your Previous Order";
                                return Request.CreateResponse(HttpStatusCode.Ambiguous, messg);
                            }
                            else
                            {
                                od.BuyerEmail = email;
                                od.BuyerID = BuyerID;
                                od.OrderQuantity = ordQty;
                                od.ProductID = ProID;
                                od.OrderStatus = "Placed";
                                entities.Table_OrderDetails.Add(od);
                            }

                        }
                        else
                        {
                            string msg = "Product Not Found";
                            return Request.CreateResponse(HttpStatusCode.NotFound, msg);
                        }


                        Nullable<int> avilableQuantity = (from p in entities.Table_ProductsDetails where p.ProductID == ProID select p.AvailableQuantity).FirstOrDefault();
                        var entity = entities.Table_ProductsDetails.FirstOrDefault(p => p.ProductID == ProID);
                        entity.AvailableQuantity = avilableQuantity - ordQty;
                        entities.SaveChanges();

                        //Sending EMail to gmail

                        MailMessage mm = new MailMessage();
                        mm.From = new MailAddress("sam077royal@gmail.com");
                        mm.To.Add(email);
                        mm.Subject = "OMS Order";
                        mm.Body = "Your Order has been successfully placed ";
                        mm.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.UseDefaultCredentials = true;
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential("sam077royal@gmail.com", "doitb4udie");
                        smtp.Send(mm);


                        var message = Request.CreateResponse(HttpStatusCode.Created, od);
                        message.Headers.Location = new Uri(Request.RequestUri + od.OrderID.ToString());
                        return message;
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/BuyersActions/viewOrders")]
        [BasicAuthentication]
        public IHttpActionResult viewOrders()
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    string BuyerID = (from b in entities.Buyers where b.Email == email select b.BuyerID).FirstOrDefault();
                    string BuyerName = (from b in entities.Buyers where b.Email == email select b.BuyerName).FirstOrDefault();
                    if (entities.Buyers.Any(buyer => buyer.BuyerID.Equals(BuyerID) && buyer.BuyerName.Equals(BuyerName) || BuyerID != null || BuyerName != null))
                    {
                        IList<JoinOrderDetails> jod = entities.JoinOrderDetails(BuyerID).Select(x => new JoinOrderDetails()
                        {
                            BuyerName = x.BuyerName,
                            ShippingAddress = x.ShippingAddress,
                            OrderID = x.OrderID,
                            BuyerEmail = x.BuyerEmail,
                            OrderItemID = x.OrderItemID,
                            OrderQuantity = x.OrderQuantity,
                            OrderStatus = x.OrderStatus,
                            ProductImage = x.ProductImage,
                            ProductName = x.ProductName,
                            ProductWeight = x.ProductWeight,
                            ProductHeight = x.ProductHeight
                        }).ToList();


                        return Ok(jod);
                    }
                    else
                    {

                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

        [HttpDelete]
        [Route("api/BuyersActions/DeleteOrder/{oItmID}")]
        [BasicAuthentication]
        public HttpResponseMessage DeleteOrder(string oItmID)
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    string BuyerID = (from b in entities.Buyers where b.Email == email select b.BuyerID).FirstOrDefault();
                    string BuyerName = (from b in entities.Buyers where b.Email == email select b.BuyerName).FirstOrDefault();
                    if (entities.Buyers.Any(buyer => buyer.BuyerID.Equals(BuyerID) && buyer.BuyerName.Equals(BuyerName) || BuyerID != null || BuyerName != null))
                    {
                        if (entities.Table_OrderDetails.Any(order => order.BuyerID.Equals(BuyerID) && order.OrderItemID.Equals(oItmID)))
                        {
                            Nullable<int> OrderQuantity = (from ods in entities.Table_OrderDetails where ods.OrderItemID == oItmID select ods.OrderQuantity).FirstOrDefault();
                            string productID = (from ods in entities.Table_OrderDetails where ods.OrderItemID == oItmID select ods.ProductID).FirstOrDefault();
                            Nullable<int> AvailableProductQuantity = (from pro in entities.Table_ProductsDetails where pro.ProductID == productID select pro.AvailableQuantity).FirstOrDefault();
                            entities.Table_OrderDetails.Remove(entities.Table_OrderDetails.FirstOrDefault(odr => odr.OrderItemID == oItmID));
                            var entity = entities.Table_ProductsDetails.FirstOrDefault(p => p.ProductID == productID);
                            entity.AvailableQuantity = AvailableProductQuantity + OrderQuantity;
                            entities.SaveChanges();

                            var message = "Order with OrderItemID " + oItmID + " Has been Deleted Successfully";
                            return Request.CreateResponse(HttpStatusCode.OK, message);
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.Unauthorized);
                            return message;
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [Route("api/BuyersActions/UpdateOrderQuantity/{oItmID}/{qtty}")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateOrderQuantity(string oItmID, int qtty)
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    Table_OrderDetails od = new Table_OrderDetails();
                    string BuyerID = (from b in entities.Buyers where b.Email == email select b.BuyerID).FirstOrDefault();
                    string BuyerName = (from b in entities.Buyers where b.Email == email select b.BuyerName).FirstOrDefault();
                    if (entities.Buyers.Any(buyer => buyer.BuyerID.Equals(BuyerID) && buyer.BuyerName.Equals(BuyerName) || BuyerID != null || BuyerName != null))
                    {
                        if(entities.Table_OrderDetails.Any(odrs => odrs.OrderItemID.Equals(oItmID) && odrs.BuyerID.Equals(BuyerID)))
                        {
                            Nullable<int> CurrentOrderQtty = (from ord in entities.Table_OrderDetails where ord.OrderItemID.Equals(oItmID) && ord.OrderStatus != "Completed" select ord.OrderQuantity).FirstOrDefault();
                            string ProductID = (from pr in entities.Table_OrderDetails where pr.OrderItemID.Equals(oItmID) select pr.ProductID ).FirstOrDefault();
                            var entiti = entities.Table_OrderDetails.FirstOrDefault(e => e.OrderItemID.Equals(oItmID) && e.OrderStatus != "Completed");
                            entiti.OrderQuantity = CurrentOrderQtty + (qtty);
                            Nullable<int> avilableQuantity = (from p in entities.Table_ProductsDetails where p.ProductID == ProductID  select p.AvailableQuantity).FirstOrDefault();
                            var proEntity = entities.Table_ProductsDetails.FirstOrDefault(e => e.ProductID.Equals(ProductID));
                            proEntity.AvailableQuantity = avilableQuantity + (-qtty);
                            entities.SaveChanges();

                            string msg = "quantity Updated Successfully";
                            return Request.CreateResponse(HttpStatusCode.Accepted, msg);
                        }
                        else
                        {
                            string msg = "Either The Product has not been ordered yet OR you are unautharized To update the Order";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, msg);
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}