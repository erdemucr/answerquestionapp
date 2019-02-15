using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Question;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AqApplication.Service.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }

}
