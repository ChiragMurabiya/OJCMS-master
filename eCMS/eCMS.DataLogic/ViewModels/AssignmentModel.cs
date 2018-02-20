//*********************************************************
//
//    Copyright � Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class AssignmentModel:BaseModel
    {
        public CaseWorker CaseWorker { get; set; }
        public CaseSupportCircle CaseSupportCircle { get; set; }
    }
}
