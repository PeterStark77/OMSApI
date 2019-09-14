using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OMSDataAccessLayer;
using System.Threading;
using System.Web;
using System.IO;
using System.Collections.Generic;
using OrderManagementSystemAPI.Models;
using System.Threading.Tasks;

namespace OrderManagementSystemAPI.Controllers
{
    public class AdministratorActionsController : ApiController
    {
        [HttpPost]
        [Route("api/AdministratorActions/ADDProduct/{proID}")]
        [BasicAuthentication]
        public HttpResponseMessage ADDProduct(string proID, [FromBody] Table_ProductsDetails Pro)
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    string AdminName = (from ad in entities.AppAdministrators where ad.AdminEmail == email select ad.AdminName).FirstOrDefault();
                    if (entities.AppAdministrators.Any(ad => ad.AdminEmail == email && ad.AdminName == AdminName || AdminName != null))
                    {
                        if(entities.Table_ProductsDetails.Any(pr => pr.ProductID.Equals(proID)))
                        {
                            string mesge = "Product ID Exists";
                            return Request.CreateResponse(HttpStatusCode.Conflict, mesge);
                        }
                        else
                        {
                            Pro.ProductID = proID;
                            entities.Table_ProductsDetails.Add(Pro);
                            entities.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.Created, Pro);
                            message.Headers.Location = new Uri(Request.RequestUri + Pro.ID.ToString());
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

        [HttpPost]
        [Route("api/AdministratorActions/UploadProductImage/{PID}")]
        [BasicAuthentication]
        public Task<HttpResponseMessage> UploadProductImage(string PID)
        {
            string email = Thread.CurrentPrincipal.Identity.Name;
            using (OMSdbEntities entities = new OMSdbEntities())
            {
                string AdminName = (from ad in entities.AppAdministrators where ad.AdminEmail == email select ad.AdminName).FirstOrDefault();
                if (entities.AppAdministrators.Any(ad => ad.AdminEmail == email && ad.AdminName == AdminName || AdminName != null))
                {
                    if(entities.Table_ProductsDetails.Any(pro => pro.ProductID.Equals(PID)))
                    {
                        List<string> savedFilePath = new List<string>();
                        // Check if the request contains multipart/form-data
                        if (!Request.Content.IsMimeMultipartContent())
                        {
                            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                        }
                        //Get the path of folder where we want to upload all files.
                        string rootPath = HttpContext.Current.Server.MapPath("~/ProductImages");
                        var provider = new MultipartFileStreamProvider(rootPath);
                        // Read the form data.
                        //If any error(Cancelled or any fault) occurred during file read , return internal server error
                        var task = Request.Content.ReadAsMultipartAsync(provider).
                            ContinueWith<HttpResponseMessage>(t =>
                            {
                                if (t.IsCanceled || t.IsFaulted)
                                {
                                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                                }
                                foreach (MultipartFileData dataitem in provider.FileData)
                                {
                                    try
                                    {
                                    //Replace / from file name
                                    string name = dataitem.Headers.ContentDisposition.FileName.Replace("\"", "");

                                    //Fetching File name without extension
                                    string[] fullname = name.Split('.');
                                        string fname = fullname[0];

                                        string newFileName = fname + PID + Path.GetExtension(name);
                                    //Move file from current location to target folder.
                                    File.Move(dataitem.LocalFileName, Path.Combine(rootPath, newFileName));

                                    }
                                    catch (Exception ex)
                                    {
                                        string message = ex.Message;
                                    }
                                }

                                return Request.CreateResponse(HttpStatusCode.Created, savedFilePath);
                            });
                        foreach (MultipartFileData dataitem in provider.FileData)
                        {
                            try
                            {
                                //Replace / from file name
                                string name = dataitem.Headers.ContentDisposition.FileName.Replace("\"", "");

                                string[] fullname = name.Split('.');
                                string fname = fullname[0];

                                string newFileName = fname + PID + Path.GetExtension(name);

                                Table_ProductImage pim = new Table_ProductImage();
                                pim.ProductID = PID;
                                pim.ProductImage = "~/ProductImages/" + newFileName;
                                entities.Table_ProductImage.Add(pim);
                                entities.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                string message = ex.Message;
                            }
                        }
                        return task;
                    }
                    else
                    {
                       
                        var message = Request.CreateResponse(HttpStatusCode.NotFound);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }

                }
                else
                {
                    var message = Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<HttpResponseMessage>(message);
                }

            }

        }

        [HttpPut]
        [Route("api/AdministratorActions/UpdateOrderStatus/{OitmID}")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateOrderStatus(string OitmID, [FromBody] Table_OrderDetails od)
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    string AdminName = (from ad in entities.AppAdministrators where ad.AdminEmail == email select ad.AdminName).FirstOrDefault();
                    if (entities.AppAdministrators.Any(ad => ad.AdminEmail == email && ad.AdminName == AdminName || AdminName != null))
                    {
                        var entity = entities.Table_OrderDetails.FirstOrDefault(e => e.OrderItemID == OitmID && e.OrderStatus != "Completed");
                        entity.OrderStatus = od.OrderStatus;
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created);
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
        [Route("api/AdministratorActions/ViewOrders")]
        [BasicAuthentication]
        public IHttpActionResult ViewOrders()
        {
            try
            {
                string email = Thread.CurrentPrincipal.Identity.Name;
                using (OMSdbEntities entities = new OMSdbEntities())
                {
                    string AdminName = (from ad in entities.AppAdministrators where ad.AdminEmail == email select ad.AdminName).FirstOrDefault();
                    if (entities.AppAdministrators.Any(ad => ad.AdminEmail == email && ad.AdminName == AdminName || AdminName != null))
                    {
                        IList<JoinOrderDetails> jod = entities.JoinOrderDetailsForAdmin().Select(x => new JoinOrderDetails()
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
    }
}