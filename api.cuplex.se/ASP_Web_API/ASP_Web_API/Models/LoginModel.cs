using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;

namespace ASP_Web_API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LogInModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}