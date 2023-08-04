using System.ComponentModel.DataAnnotations;
using System.Configuration;


namespace Blog
{
    public static class Const
    {
        public enum Month
        {
            [Display(Name = "01")]
            JAN = 1,
            [Display(Name = "02")]
            FEB = 2,
            [Display(Name = "03")]
            MAR = 3,
            [Display(Name = "04")]
            APR = 4,
            [Display(Name = "05")]
            MAY = 5,
            [Display(Name = "06")]
            JUN = 6,
            [Display(Name = "07")]
            JUL = 7,
            [Display(Name = "08")]
            AUG = 8,
            [Display(Name = "09")]
            SEP = 9,
            [Display(Name = "10")]
            OCT = 10,
            [Display(Name = "11")]
            NOV = 11,
            [Display(Name = "12")]
            DEC = 12,
        }

        public const string ROLE_ADMIN = "admin";

        public const string UPLOAD_DIR = "uploads";
        public const string UPLOAD_IMAGE_DIR = UPLOAD_DIR + "/images";
        public const string PUBLIC_BUCKET = "public";
    }
}
