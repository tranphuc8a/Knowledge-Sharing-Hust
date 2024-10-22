﻿using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("CourseRegister")]
    public class CourseRegister : Entity
    {
        public Guid CourseRegisterId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }


        protected override CourseRegister Init()
        {
            return new CourseRegister();
        }
    }
}
