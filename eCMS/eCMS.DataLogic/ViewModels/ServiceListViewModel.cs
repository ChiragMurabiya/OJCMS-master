//*********************************************************
//
//    Copyright � Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using eCMS.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCMS.DataLogic.ViewModels
{
    [NotMapped]
    public class ServiceListViewModel
    {
      
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public bool IsActive { get; set; }
        
        [Display(Name = "Service Provider")]
        public String ServiceProviderName { get; set; }

      
        [Display(Name = "Region")]
        public String RegionName { get; set; }

        public int RegionID { get; set; }

        public bool? ServiceTypeID { get; set; }

        public int ServiceProviderID { get; set; }

    }
}
