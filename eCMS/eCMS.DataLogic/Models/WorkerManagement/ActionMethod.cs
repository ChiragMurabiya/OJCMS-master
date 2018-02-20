﻿//*********************************************************
//
//    Copyright (c) JobSlice LLC. All rights reserved.
//	  Technical Contact: Alain Templeman, info@jobslice.com
//	  http://www.jobslice.com
//
//*********************************************************

using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.Models
{
    public class ActionMethod : EntityBaseModel
    {
        [Required]
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }

        [Required]
        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }

        [Required]
        [Display(Name = "Action or Method Name")]
        public string ActionMethodName { get; set; }

        [Required]
        [Display(Name = "Action or Method Display Name")]
        public string DisplayActionMethodName { get; set; }

        [Required]
        [Display(Name = "Display Case Tab Name")]
        public string DisplayName { get; set; }

    }
}
