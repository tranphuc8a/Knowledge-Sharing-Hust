﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Tables
{
    [Table("Category")]
    public class Category : Entity
    {
        public Guid CategoryId { get; set; } = Guid.Empty;
        public string CategoryName { get; set; } = string.Empty;

        protected override Category Init()
        {
            return new Category();
        }
    }
}
