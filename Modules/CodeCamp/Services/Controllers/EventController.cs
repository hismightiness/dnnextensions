﻿/*
 * Copyright (c) 2015, Will Strohl
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list 
 * of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this 
 * list of conditions and the following disclaimer in the documentation and/or 
 * other materials provided with the distribution.
 * 
 * Neither the name of Will Strohl, nor the names of its contributors may be used 
 * to endorse or promote products derived from this software without specific prior 
 * written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    // TODO: add validation catches and formal error responses for all end points

    /// <summary>
    /// This is a partial class that spans multiple class files, in order to keep the code manageable. Each method is necessary to support the front end SPA implementation.
    /// </summary>
    /// <remarks>
    /// The SupportModules attribute will require that all API calls set and include module headers, event GET requests. Even Fiddler will return 401 Unauthorized errors.
    /// </remarks>
    [SupportedModules("CodeCampEvents")]
    public partial class EventController : ServiceBase
    {
        /// <summary>
        /// Get an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetEvents
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetEvents()
        {
            try
            {
                var codeCamps = CodeCampDataAccess.GetItems(ActiveModule.ModuleID);
                var response = new ServiceResponse<List<CodeCampInfo>> { Content = codeCamps.ToList() };

                if (codeCamps == null)
                {
                    ServiceResponseHelper<List<CodeCampInfo>>.AddNoneFoundError("CodeCampInfo", ref response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetEventByModuleId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetEventByModuleId()
        {
            try
            {
                var codeCamp = CodeCampDataAccess.GetItemByModuleId(ActiveModule.ModuleID);

                if (codeCamp != null)
                {
                    codeCamp.BeginDate = codeCamp.BeginDate.ToLocalTime();
                    codeCamp.CreatedByDate = codeCamp.CreatedByDate.ToLocalTime();
                    codeCamp.EndDate = codeCamp.EndDate.ToLocalTime();
                    codeCamp.LastUpdatedByDate = codeCamp.LastUpdatedByDate.ToLocalTime();
                }

                var response = new ServiceResponse<CodeCampInfo> { Content = codeCamp };

                if (codeCamp == null)
                {
                    ServiceResponseHelper<CodeCampInfo>.AddNoneFoundError("CodeCampInfo", ref response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetEvent(int itemId)
        {
            try
            {
                var codeCamp = CodeCampDataAccess.GetItem(itemId, ActiveModule.ModuleID);
                var response = new ServiceResponse<CodeCampInfo> { Content = codeCamp };

                if (codeCamp == null)
                {
                    ServiceResponseHelper<CodeCampInfo>.AddNoneFoundError("CodeCampInfo", ref response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(int itemId)
        {
            try
            {
                CodeCampDataAccess.DeleteItem(itemId, ActiveModule.ModuleID);
                var response = new ServiceResponse<string> { Content = SUCCESS_MESSAGE };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Create an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CeateEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateEvent(CodeCampInfo newEvent)
        {
            try
            {
                newEvent.CreatedByDate = DateTime.Now;
                newEvent.CreatedByUserId = UserInfo.UserID;
                newEvent.LastUpdatedByDate = DateTime.Now;
                newEvent.LastUpdatedByUserId = UserInfo.UserID;
                newEvent.ModuleId = ActiveModule.ModuleID;

                CodeCampDataAccess.CreateItem(newEvent);

                var response = new ServiceResponse<string> { Content = Globals.RESPONSE_SUCCESS };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateEvent(CodeCampInfo updatedEvent)
        {
            try
            {
                CodeCampDataAccess.UpdateItem(updatedEvent);

                var response = new ServiceResponse<string> { Content = SUCCESS_MESSAGE };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Use to determine if the user has edit permissions
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UserCanEditEvent
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage UserCanEditEvent(int itemId)
        {
            ServiceResponse<string> response = null;

            if (UserInfo.IsSuperUser || UserInfo.IsInRole(PortalSettings.AdministratorRoleName) || ModulePermissionController.HasModulePermission(ActiveModule.ModulePermissions, "Edit"))
            {
                response = new ServiceResponse<string>() { Content = Globals.RESPONSE_SUCCESS };
            }
            else
            {
                var codeCamp = CodeCampDataAccess.GetItem(itemId, ActiveModule.ModuleID);

                if (codeCamp != null && codeCamp.CreatedByUserId == UserInfo.UserID || codeCamp.LastUpdatedByUserId == UserInfo.UserID)
                {
                    response = new ServiceResponse<string>() {Content = Globals.RESPONSE_SUCCESS };
                }
                else
                {
                    response = new ServiceResponse<string>() { Content = Globals.RESPONSE_FAILURE };
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
        }
    }
}