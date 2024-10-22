﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Entities
{
    public class CoursePayment : Entity
    {
        [Key]
        public Guid CoursePaymentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public decimal Fee { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;

        protected override CoursePayment Init()
        {
            return new CoursePayment();
        }
    }
}
