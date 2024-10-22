﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Star")]
    public class Star : Entity
    {
        public Guid StarId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserItemId { get; set; }
        public int Stars { get; set; }
        protected override Star Init()
        {
            return new Star();
        }
    }
}
