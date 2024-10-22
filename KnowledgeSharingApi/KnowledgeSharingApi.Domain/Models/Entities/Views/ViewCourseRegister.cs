﻿using KnowledgeSharingApi.Domains.Models.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewCourseRegister")]
    public class ViewCourseRegister : CourseRegister
    {
        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Cover { get; set; }

        public string? Thumbnail { get; set; }

        public decimal? Fee { get; set; }

        public bool? IsFree { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Abstract { get; set; } = string.Empty;

        public Guid CourseOwnerUserId { get; set; }

        public string CourseOwnerUsername { get; set; } = string.Empty;
        
        public string CourseOwnerFullName { get; set; } = string.Empty;

        public string? CourseOwnerAvatar { get; set; }

        public string? CourseOwnerCover { get; set; }

        protected override ViewCourseRegister Init()
        {
            return new ViewCourseRegister();
        }
    }
}
