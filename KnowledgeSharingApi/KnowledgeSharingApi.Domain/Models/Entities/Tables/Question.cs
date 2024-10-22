﻿using KnowledgeSharingApi.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Question")]
    public class Question : Post
    {
        public Guid? CourseId { get; set; }
        public bool IsAccept { get; set; }


        //public override EPostType PostType { get => EPostType.Question; }
        protected override Question Init()
        {
            return new Question();
        }
    }
}
