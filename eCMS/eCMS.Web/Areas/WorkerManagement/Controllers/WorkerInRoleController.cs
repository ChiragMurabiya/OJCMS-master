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
using eCMS.ExceptionLoging;
using eCMS.Shared;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Linq;
using System.Data.Entity.Infrastructure;

namespace eCMS.Web.Areas.WorkerManagement.Controllers
{
    public class WorkerInRoleController : WorkerBaseController
    {
        public WorkerInRoleController(IWorkerRepository workerRepository, 
            IWorkerRoleRepository workerroleRepository,
            IWorkerInRoleRepository workerinroleRepository,
            IWorkerRoleActionPermissionRepository workerroleactionpermissionRepository,
            IWorkerSubProgramRepository workersubprogramRepository,
            ISubProgramRepository subprogramRepository,
            IProgramRepository programRepository)
            : base(workerroleactionpermissionRepository)
        {
            this.workerRepository = workerRepository;
            this.workerroleRepository = workerroleRepository;
            this.workerinroleRepository = workerinroleRepository;
            this.workersubprogramRepository=workersubprogramRepository;
            this.subprogramRepository = subprogramRepository;
            this.programRepository = programRepository;
        }

        /// <summary>
        /// This action returns the list of WorkerInRole
        /// </summary>
        /// <param name="dsRequest">sort filter</param>
        /// <param name="searchWorkerInRole">search filter</param>
        /// <returns>view result</returns>
        [WorkerAuthorize]
        public ViewResult Index([DataSourceRequest(Prefix = "Grid")] DataSourceRequest dsRequest, WorkerInRole searchWorkerInRole, int workerId)
        {

            //if(searchWorkerInRole!=null)
            //{
            //	if (dsRequest.Filters == null || (dsRequest.Filters != null && dsRequest.Filters.Count == 0))
            //	{
            //		if (dsRequest.Filters == null)
            //		{
            //			dsRequest.Filters = new List<IFilterDescriptor>();
            //		}
            //	}
            //}
            //DataSourceRequest dsRequestTotalCountQuery = new DataSourceRequest();
            //dsRequestTotalCountQuery.Filters = dsRequest.Filters;
            //DataSourceResult totalCountQuery = workerinroleRepository.All.ToDataSourceResult(dsRequestTotalCountQuery);
            //ViewData["total"] = totalCountQuery.Data.AsQueryable().Count();
            //if (dsRequest.PageSize == 0)
            //{
            //    dsRequest.PageSize = Constants.CommonConstants.DefaultPageSize;
            //}
            //DataSourceResult result = workerinroleRepository.AllIncluding(workerId,workerinrole => workerinrole.CreatedByWorker, workerinrole => workerinrole.LastUpdatedByWorker, workerinrole => workerinrole.Worker, workerinrole => workerinrole.WorkerRole).ToDataSourceResult(dsRequest);
            //return View(result.Data);
            return View();
        }

        /// <summary>
        /// This action creates new workerinrole
        /// </summary>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Create(int workerId)
        {
            //create a new instance of workerinrole
            WorkerInRole workerinrole = new WorkerInRole();
            workerinrole.WorkerID = workerId;
            ViewBag.PossibleWorkerRoles = workerroleRepository.All;
            //return view result
            return View(workerinrole);
        }

        /// <summary>
        /// This action saves new workerinrole to database
        /// </summary>
        /// <param name="workerinrole">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Create(WorkerInRole workerinrole, int workerId)
        {
            workerinrole.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    workerinroleRepository.InsertOrUpdate(workerinrole);
                    workerinroleRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { workerId = workerId });
                }
                else
                {
                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            workerinrole.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (workerinrole.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                workerinrole.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                workerinrole.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            ViewBag.PossibleWorkerRoles = workerroleRepository.All;
            //return view with error message if operation is failed
            return View(workerinrole);
        }

        /// <summary>
        /// This action edits an existing workerinrole
        /// </summary>
        /// <param name="id">workerinrole id</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        public ActionResult Edit(int id)
        {
            //find the existing workerinrole from database
            WorkerInRole workerinrole = workerinroleRepository.Find(id);
            ViewBag.PossibleWorkerRoles = workerroleRepository.All;
            //return editor view
            return View(workerinrole);
        }

        /// <summary>
        /// This action saves an existing workerinrole to database
        /// </summary>
        /// <param name="workerinrole">data to save</param>
        /// <returns>action result</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult Edit(WorkerInRole workerinrole, int workerId)
        {
            workerinrole.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;

            try
            {
                //validate data
                if (ModelState.IsValid)
                {
                    //call the repository function to save in database
                    workerinroleRepository.InsertOrUpdate(workerinrole);
                    workerinroleRepository.Save();
                    //redirect to list page after successful operation
                    return RedirectToAction(Constants.Views.Index, new { workerId = workerId });
                }
                else
                {

                    foreach (var modelStateValue in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelStateValue.Errors)
                        {
                            workerinrole.ErrorMessage = error.ErrorMessage;
                            break;
                        }
                        if (workerinrole.ErrorMessage.IsNotNullOrEmpty())
                        {
                            break;
                        }
                    }

                }

            }
            catch (CustomException ex)
            {
                workerinrole.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                workerinrole.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            ViewBag.PossibleWorkerRoles = workerroleRepository.All;
            //return view with error message if the operation is failed
            return View(workerinrole);
        }

        /// <summary>
        /// This action loads data to kendo grid or listview asynchronously
        /// </summary>
        /// <param name="dsRequest">search/sort parameters</param>
        /// <returns>data in json</returns>
        [WorkerAuthorize]
        [OutputCache(Duration = 0)]
        public ActionResult IndexAjax([DataSourceRequest] DataSourceRequest dsRequest, string workerId)
        {
            List<WorkerInRole> roleList = workerinroleRepository.FindAllByWorkerID(workerId.ToInteger()).AsEnumerable().Select(item => new WorkerInRole() { ID = item.ID, WorkerRoleName = item.WorkerRole.Name, ProgramName = item.Program.Name, RegionName = item.Region.Name, EffectiveFrom = item.EffectiveFrom, EffectiveTo = item.EffectiveTo }).ToList();
            if (roleList != null)
            {
                foreach (WorkerInRole role in roleList)
                {
                    List<WorkerSubProgram> subProgramList = workersubprogramRepository.FindAllByWorkerInRoleID(role.ID);
                    if (subProgramList != null)
                    {
                        foreach (WorkerSubProgram subProgram in subProgramList)
                        {
                            role.SubProgramNames = role.SubProgramNames.Concate(',', subProgram.SubProgram.Name);
                        }
                    }
                }
            }
            DataSourceResult result = roleList.ToDataSourceResult(dsRequest);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This action displays the details of an existing workerinrole on popup
        /// </summary>
        /// <param name="id">workerinrole id</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult DetailsAjax(int id)
        {
            WorkerInRole workerinrole = workerinroleRepository.Find(id);
            if (workerinrole == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Worker In Role not found");
            }
            return Content(this.RenderPartialViewToString(Constants.PartialViews.Details, workerinrole));
        }

        /// <summary>
        /// This action opens workerinrole editor on popup in add/edit mode
        /// </summary>
        /// <param name="id">workerinrole id. id=0 means add mode, id other than 0 means edit mode</param>
        /// <returns>html content</returns>
        [WorkerAuthorize]
        public ActionResult EditorAjax(int id, string workerId)
        {
            WorkerInRole workerinrole = null;
            if (id > 0)
            {
                //find an existing workerinrole from database
                workerinrole = workerinroleRepository.Find(id);
                if (workerinrole == null)
                {
                    //throw an exception if id is provided but data does not exist in database
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Worker In Role not found");
                }
            }
            else
            {
                //create a new instance if id is not provided
                workerinrole = new WorkerInRole();
                workerinrole.WorkerID = workerId.ToInteger(true);
            }
            if (IsRegionalAdministrator)
            {
                workerinrole.AllSubPrograms = subprogramRepository.FindAllByWordkerID(CurrentLoggedInWorker.ID, 0).ToList();
            }
            else
            {
                workerinrole.AllSubPrograms = subprogramRepository.All.Where(item => item.IsActive == true).ToList();
            }
            workerinrole.AssignedSubPrograms = workersubprogramRepository.All.Where(item => item.WorkerInRoleID==id).ToList();
            //ViewBag.Programs = programRepository.All.AsEnumerable().Select(o => new { ID = o.ID, Name = o.Name }).ToList();
            //return the html of editor to display on popup
            return Content(this.RenderPartialViewToString("~/Areas/WorkerManagement/Views/Worker/_WorkerInRole.cshtml", workerinrole));
        }

        /// <summary>
        /// Save data to database in ajax mode
        /// </summary>
        /// <param name="workerinrole">data to save</param>
        /// <returns>status message in json</returns>
        [WorkerAuthorize]
        [HttpPost]
        public ActionResult SaveAjax(WorkerInRole workerinrole)
        {
            //id=0 means add operation, update operation otherwise
            bool isNew = workerinrole.ID == 0;
            //validate data
            if (ModelState.IsValid)
            {

                try
                {
                    //if(!isNew)
                    //if (workerinrole.CreatedByWorkerID != CurrentLoggedInWorker.ID && CurrentLoggedInWorkerRoleIDs.IndexOf(1) == -1)
                    //{
                    //    WebHelper.CurrentSession.Content.ErrorMessage = "You are not eligible to do this action";
                    //    return RedirectToAction(Constants.Actions.AccessDenied, Constants.Controllers.Home, new { Area = String.Empty });
                    //}

                    workerinrole.SubProgramIDs = Request.Form["SubProgramIDs"].ToString(true).ToStringArray(',');
                    if (workerinrole.SubProgramIDs == null || (workerinrole.SubProgramIDs != null && workerinrole.SubProgramIDs.Length == 0))
                    {
                        throw new CustomException("Please select at least one sub-program");
                    }

                    workerinrole.EffectiveFrom = Request.Form["WorkerInRole_EffectiveFrom"].ToDateTime();
                    workerinrole.EffectiveTo = Request.Form["WorkerInRole_EffectiveTo"].ToDateTime();
                    if (!workerinrole.EffectiveFrom.IsValidDate())
                    {
                        throw new CustomException("Please enter effective from date");
                    }
                    if (!workerinrole.EffectiveTo.IsValidDate())
                    {
                        throw new CustomException("Please enter effective to date");
                    }
                    //set the id of the worker who has added/updated this record
                    workerinrole.LastUpdatedByWorkerID = CurrentLoggedInWorker.ID;
                    //call repository function to save the data in database
                    workerinroleRepository.InsertOrUpdate(workerinrole);
                    workerinroleRepository.Save();
                    //set status message
                    if (isNew)
                    {
                        workerinrole.SuccessMessage = "Worker role has been added successfully";
                    }
                    else
                    {
                        workerinrole.SuccessMessage = "Worker role has been updated successfully";
                    }
                }
                catch (DbUpdateException ex)
                {
                    workerinrole.ErrorMessage = "There is a problem adding data to database";
                }
                catch (CustomException ex)
                {
                    workerinrole.ErrorMessage = ex.UserDefinedMessage;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Manage(ex);
                    workerinrole.ErrorMessage = Constants.Messages.UnhandelledError;
                }
            }
            else
            {
                foreach (var modelStateValue in ViewData.ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        workerinrole.ErrorMessage = error.ErrorMessage;
                        break;
                    }
                    if (workerinrole.ErrorMessage.IsNotNullOrEmpty())
                    {
                        break;
                    }
                }
            }
            //return the status message in json
            if (workerinrole.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, workerinrole) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.Alert, workerinrole) });
            }
        }

        /// <summary>
        /// delete workerinrole from database usign ajax operation
        /// </summary>
        /// <param name="id">workerinrole id</param>
        /// <returns>action status in json</returns>
        [WorkerAuthorize]
        public ActionResult DeleteAjax(int id)
        {
            //find the workerinrole in database
            BaseModel statusModel = new BaseModel();
            try
            {
                //delete workerinrole from database
                workerinroleRepository.Delete(id);
                workerinroleRepository.Save();
                //set success message
                statusModel.SuccessMessage = "Worker Role has been deleted successfully";
            }
            catch (CustomException ex)
            {
                statusModel.ErrorMessage = ex.UserDefinedMessage;
            }
            catch (Exception ex)
            {
                ExceptionManager.Manage(ex);
                statusModel.ErrorMessage = Constants.Messages.UnhandelledError;
            }
            //return action status in json to display on a message bar
            if (statusModel.ErrorMessage.IsNotNullOrEmpty())
            {
                return Json(new { success = false, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
            else
            {
                return Json(new { success = true, data = this.RenderPartialViewToString(Constants.PartialViews.AlertSliding, statusModel) });
            }
        }

    }
}

