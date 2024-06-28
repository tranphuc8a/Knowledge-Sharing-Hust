using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities.Views
{
    [Table("ViewTotalUserStar")]
    public class ViewTotalUserStar
    {
        public Guid UserId { get; set; }

        // So luong user danh gia cac useritem cua user
        public int TotalStar { get; set; }

        // Tong so star cua toan bo user danh gia useritem cua user
        public int SumStar { get; set; }
    }
}
