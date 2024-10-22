﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class Star : Entity
    {
        [Key]
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
