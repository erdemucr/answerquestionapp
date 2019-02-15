using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AqApplication.Core.Type
{
    public class Result
    {
        public Result()
        {
        }
        /// <summary>
        /// Exception	Object
//        SystemException Exception
//IndexOutOfRangeException SystemException
//NullReferenceException SystemException
//AccessViolationException SystemException
//InvalidOperationException SystemException
//ArgumentException SystemException
//ArgumentNullException ArgumentException
//ArgumentOutOfRangeException ArgumentException
//ExternalException SystemException
//COMException ExternalException
//SEHException ExternalException
        /// </summary>
        /// <param name="ex"></param>
        public Result(Exception ex)
        {
            switch (ex.GetType().ToString())
            {
                case "System.Data.Entity.Validation.DbEntityValidationException":
                    this.Message = "";
                    foreach (var eve in ((DbEntityValidationException)ex).EntityValidationErrors)
                    {
                        this.Message+= string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);

                        foreach (var ve in eve.ValidationErrors)
                        {
                            this.Message+= "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    break;
                case "System.Data.Entity.Infrastructure.DbUpdateException":
                    this.Message = ex.InnerException.ToString();
                    break;
                case "System.Exception":
                    this.Message = ex.InnerException != null ? ex.InnerException.ToString() : "";
                        break;

            }
            this.Success = false;
        }
     
        public bool Success { get; set; }
        public string Message { get; set; }
        public int InstertedId { get; set; }
        public int CurrentIp { get; set; }
        public Paginition Paginition { get; set; }




    }


}
