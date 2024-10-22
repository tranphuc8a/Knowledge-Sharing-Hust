﻿using KnowledgeSharingApi.Domains.Enums;
using KnowledgeSharingApi.Domains.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.ApiResponseModels
{
    public class ResponseCourseCardModel : Entity
    {
        public Guid UserItemId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Abstract { get; set; } = string.Empty;

        public string? Thumbnail { get; set; }

        public EPrivacy Privacy { get; set; }

        //public string Introduction { get; set; } = string.Empty;

        public decimal Fee { get; set; }

        //public int EstimateTimeInMinutes { get; set; }

        public bool IsFree { get; set; }



        public ECourseRoleType? CourseRoleType { get; set; }

        public Guid? CourseRelationId { get; set; }



        public Guid UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string? Avatar { get; set; } = null;

        protected override ResponseCourseCardModel Init()
        {
            return new ResponseCourseCardModel();
        }
    }
}
