﻿
using AqApplication.Repository.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AqApplication.Manage.Models
{
    public class SearchModel
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public List<SearchInput> SearchInput { get; set; }

        public SearchModelPosition Position { get; set; }
    }
    public enum SearchModelPosition
    {
        Horizontal,
        Vertical
    }

    public class SearchInput
    {
        public InputType InputType { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string CssClass { get; set; }

        public List<SelectListItem> SelectList { get; set; }
        public SearchInput(InputType inputType, string name, string id, string displayName)
        {
            InputType = inputType;
            Name = name;
            Id = id;
            DisplayName = displayName;
            SetCssClass();
        }
        public SearchInput(InputType inputType, string name, string id, string displayName, List<SelectListItem> selectList)
        {
            InputType = inputType;
            Name = name;
            Id = id;
            DisplayName = displayName;
            SelectList = selectList;
            SetCssClass();
        }

        private void SetCssClass()
        {
            switch (InputType)
            {
                case InputType.Text: CssClass = "form-control";
                    break;
                case InputType.Date: CssClass = "form-control dpicker";
                    break;
                case InputType.Number:CssClass = "form-control number";
                    break;
                
            }
            
        }




    }
}