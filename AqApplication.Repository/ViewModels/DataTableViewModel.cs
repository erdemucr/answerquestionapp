using System;
using System.Collections.Generic;
using System.Text;

namespace AnswerQuestionApp.Repository.ViewModels
{
    public class DataTableViewModel
    {
        public List<ColModel> Columns { get; set; }
        public List<Order> Order { get; set; }
        public Search Search { get; set; }
        public int? Start { get; set; }
        public int? Length { get; set; }
        public int? Draw { get; set; }

        //req data
        public int TotalCount { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
    }

    public class Order
    {
        public string column { get; set; }
        public string dir { get; set; }
    }

    public class ColModel
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
    }
}
