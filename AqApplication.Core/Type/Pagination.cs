using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AqApplication.Core.Type
{

    public class Paginition : FilterModel
    {
        private int totalCount { get; set; }
        private int pageSize { get; set; }
        private int currentPage { get; set; }
        private int pageCount { get; set; }
        public Paginition(int totalCount, int pageSize, int currentPage, string name, string dateStart, string dateEnd)
        {
            this.totalCount = totalCount;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.pageCount = totalCount > 0 ? (int)Math.Round(totalCount / pageSize * (1.0)) : 1;
            this.Name = name;
            this.StartDate = dateStart;
            this.EndDate = dateEnd;
        }
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }
        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
        }
        public int PageCount
        {
            get
            {
                return pageCount;
            }
        }

    }

    public abstract class BaseFilterModel : FilterModel
    {

        public int? TotalPage { get; set; }
        public int? CurrentPage { get; set; }


    }
    public abstract class FilterModel
    {
        public string Name { get; set; }
        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
    public static class FilterModelExt
    {
        public static DateTime? ToDate(this string date)
        {
            DateTime a;
            if (DateTime.TryParse(date,out a))
            {
                return Convert.ToDateTime(date);
            }
            return null;
        }
    }
}
