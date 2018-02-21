using eCMS.DataLogic.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace eCMS.DataLogic.Models
{
    public class CaseSummary : EntityBaseModel
    {
        [Display(Name = "Reference Case", Prompt = "Reference Case")]
        [StringLength(32)]
        public String CaseNumber { get; set; }

        [StringLength(32)]
        public String DisplayID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollDate { get; set; }

        [Display(Name = "Referral Date")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReferralDate { get; set; }

        public string ProgramName { get; set; }

        //[Required(ErrorMessage = "Please select program")]
        //[Display(Name = "Program")]
        //[ForeignKey("Program")]
        //public Int32 ProgramID { get; set; }

        //[Required(ErrorMessage = "Please select primary coordinator")]
        //[Display(Name = "Primary Coordinator")]
        //[ForeignKey("SubProgram")]
        //public Int32 SubProgramID { get; set; }

        //[Required(ErrorMessage = "Please select region")]
        //[Display(Name = "Region")]
        //[ForeignKey("Region")]
        //public Int32 RegionID { get; set; }

        //[Required(ErrorMessage = "Please select jamatkhana")]
        //[Display(Name = "Jamatkhana")]
        //[ForeignKey("Jamatkhana")]
        //public Int32? JamatkhanaID { get; set; }

        [Display(Name = "Address", Prompt = "Address")]
        [StringLength(256)]
        public String Address { get; set; }

        [Display(Name = "City", Prompt = "City")]
        [StringLength(64)]
        public String City { get; set; }

        [Display(Name = "Postal Code", Prompt = "Postal Code")]
        [StringLength(16)]
        public String PostalCode { get; set; }

        [Required(ErrorMessage = "Please select intake method")]
        [Display(Name = "Intake Method")]
        [ForeignKey("IntakeMethod")]
        public Int32 IntakeMethodID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String IntakeMethodOther { get; set; }

        [Required(ErrorMessage = "Please select referral source")]
        [Display(Name = "Referral Source")]
        [ForeignKey("ReferralSource")]
        public Int32 ReferralSourceID { get; set; }

        [Display(Name = "Referral Source Comments")]
        [MaxLength]
        public String ReferralSourceComments { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String ReferralSourceOther { get; set; }

        [Display(Name = "Hearing Source")]
        [ForeignKey("HearingSource")]
        public Int32? HearingSourceID { get; set; }

        [Display(Name = "Other")]
        [StringLength(256)]
        public String HearingSourceOther { get; set; }

        [Required(ErrorMessage = "Please select case status")]
        [Display(Name = "Case Status")]
        [ForeignKey("CaseStatus")]
        public Int32 CaseStatusID { get; set; }

        [Display(Name = "Reference Case")]
        [ForeignKey("ReferenceCase")]
        public Int32? ReferenceCaseID { get; set; }

        [Display(Name = "Comments")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Comments { get; set; }

        [Required(ErrorMessage = "Please select risk type")]
        [Display(Name = "Risk Level")]
        public Int32 RiskTypeID { get; set; }

        [Display(Name = "Presenting Problem (Reason for seeking support)")]
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public String PresentingProblem { get; set; }

        public Case caseModel { get; set; }

    }
}
