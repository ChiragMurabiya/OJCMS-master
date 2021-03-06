//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using EasySoft.Helper;
using eCMS.BusinessLogic.Helpers;
using eCMS.BusinessLogic.Repositories;
using eCMS.DataLogic.Models;
using eCMS.DataLogic.Models.Lookup;
using eCMS.ExceptionLoging;
using eCMS.Shared;
using eCMS.Web.Controllers;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace eCMS.Web.Areas.Lookup.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IServiceProviderRepository serviceproviderRepository;
        public ServiceController(IServiceRepository serviceRepository,
             IServiceProviderRepository serviceproviderRepository,
             IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
             IRegionRepository regionRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.serviceproviderRepository = serviceproviderRepository;
            this.serviceRepository = serviceRepository;
            this.regionRepository = regionRepository;
        }

        /// <summary>
        /// This action returns the list of Service
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchService">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ActionResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest)
        {
            if (!ViewBag.HasAccessToAdminModule && !ViewBag.HasAccessToOtherConfigurationData)
            {
                WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
            }
            //create a new instance of service
            Service service = new Service();
            //return view result
            return View(service);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, Service searchService)
        {
            if (dsRequest.Filters == null)
            {
                dsRequest.Filters = new List<IFilterDescriptor>();

            }
           // var data = regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, 0).Where(item => item.IsActive == true).Select(m => m.ID.ToString()).ToList();


            DataSourceResult result = serviceRepository.FindAllByRegion(searchService,dsRequest);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action opens service editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">service id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id)
        {
            Service service = null;
            if (id > 0)
            {
                //find an existing service from database
                service = serviceRepository.FindByID(id);
                if (service == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Service not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                service = new Service();
            }

            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString(Constants.PartialViews.CreateOrEdit, service));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="service">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(Service service)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = service.ID == 0;

            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //call repository function to save the data in database
                    serviceRepository.InsertOrUpdate(service);
                    serviceRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        service.SuccessMessage = "Service has been added successfully";
                    }
                    else
                    {
                        service.SuccessMessage = "Service has been updated successfully";
                    }
                }
                catch (CustomException ex)
                {
                    service.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    service.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        service.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (service.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (service.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = service.ErrorMessage });
            }
            else
            {
                return Json(new { success = true, data = service.SuccessMessage });
            }
        }

        /// <summary>
        /// delete service from database usign ajax operation
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            if (!ViewBag.HasAccessToAdminModule)
            {
                BaseModel baseModel = new BaseModel();
                baseModel.ErrorMessage = "You are not eligible to do this action";
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, baseModel) }, JsonRequestBehavior.AllowGet);
            }
            //find the service in database
            Service service = serviceRepository.Find(id);
            if (service == null)
            {
                //set error message if it does not exist in database
                service = new Service();
                service.ErrorMessage = "Service not found";
            }
            else
            {
                try
                {
                    //delete service from database
                    serviceRepository.Delete(service);
                    serviceRepository.Save();
                    //set success message
                    service.SuccessMessage = "Service has been deleted successfully";
                }
                catch (CustomException ex)
                {
                    service.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    service.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            //return action status in json to display on a message bar
            if (service.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, service) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, service) });
            }
        }

        public JsonResult LoadServiceProviderAjax(int serviceTypeID, int? regionID=0)
        {
            List<SelectListItem> serviceproviderList = new List<SelectListItem>();
            //var data = regionRepository.FindAllByWorkerID(CurrentLoggedInWorker.ID, 0).Where(item => item.IsActive == true).Select(m => m.ID).ToList();
            var data = regionRepository.NewFindAllByWorkerID(CurrentLoggedInWorker.ID, 0).Select(m => m.ID).ToList();
            if (regionID > 0)
            {
                if (serviceTypeID == 1)
                {
                    serviceproviderList = serviceproviderRepository.All.AsEnumerable().Where(u => u.RegionID == regionID && u.IsExternal == false).Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                }
                else
                {
                    serviceproviderList = serviceproviderRepository.All.AsEnumerable().Where(u => u.RegionID == regionID && u.IsExternal == true).Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                }
            }
            else if (CurrentLoggedInWorker.ID == 1)
            {
                if (serviceTypeID == 1)
                {
                    serviceproviderList = serviceproviderRepository.All.AsEnumerable().Where(u =>  u.IsExternal == false ).Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                }
                else
                {
                    serviceproviderList = serviceproviderRepository.All.AsEnumerable().Where(u => u.IsExternal == true ).Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                }
            }
            else
            {
                if (serviceTypeID == 1)
                {
                    serviceproviderList = serviceproviderRepository.All.AsEnumerable().Where(u => u.RegionID > 0 && u.IsExternal == false && data.Contains(Convert.ToInt32(u.RegionID))).Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                }
                else
                {
                    serviceproviderList = serviceproviderRepository.All.AsEnumerable().Where(u => u.RegionID > 0 && u.IsExternal == true && data.Contains(Convert.ToInt32(u.RegionID))).Select(item => new SelectListItem() { Text = item.Name, Value = item.ID.ToString() }).ToList();
                }
            }
            
            return Json(serviceproviderList, JsonRequestBehavior.AllowGet);
        }

    }
}

