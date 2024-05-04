using KnowledgeSharingApi.Domains.Annotations.Converters;
using KnowledgeSharingApi.Domains.Annotations.Validators;
using KnowledgeSharingApi.Domains.Resources.Vietnamese;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Models.Entities
{
    /// <summary>
    /// Lớp cha cho mọi lớp Entity của hệ thống kế thừa
    /// Phải có 4 trường bắt buộc: CreateBy|Date, ModifiedBy|Date
    /// Cung cấp hai phương thức Clone và GetProperties
    /// </summary>
    /// Created: PhucTV (25/12/23)
    /// Modified: None
    public abstract class Entity
    {
        public string? CreatedBy { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [GreaterThanTodayValidator(ErrorMessage = ViConstantResource.CREATED_DATE_GREATER_THAN_TODAY)]
        public DateTime? CreatedTime { get; set; }

        public string? ModifiedBy { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [GreaterThanTodayValidator(ErrorMessage = ViConstantResource.MODIFIED_DATE_GREATER_THAN_TODAY)]
        public DateTime? ModifiedTime { get; set; }

        /// <summary>
        /// Lấy về mảng các props của entity
        /// </summary>
        /// <returns> Mảng props[]</returns>
        /// Created: PhucTV (6/1/24)
        /// Modified: none
        public PropertyInfo[] GetProperties()
        {
            return GetType().GetProperties();
        }

        /// <summary>
        /// Khởi tạo đối tượng cơ sở của riêng mỗi lớp
        /// </summary>
        /// <returns> Đối tượng của lớp </returns>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        protected abstract Entity Init();

        /// <summary>
        /// Tạo ra một đối tượng clone từ đối tượng hiện tại
        /// </summary>
        /// <returns></returns>
        /// Created: PhucTV (27/12/23)
        /// Modified: None
        public virtual Entity Clone()
        {
            Entity entity = Init();
            var listProps = GetProperties();
            foreach (var prop in listProps)
            {
                prop.SetValue(entity, prop.GetValue(this));
            }
            return entity;
        }

        /// <summary>
        /// Hàm copy từ entity khác vào entity hiện tại
        /// </summary>
        /// <param name="entity"> Đối tượng được copy </param>
        /// Created: PhucTV (30/1/24)
        /// Modified: None
        public virtual Entity Copy(object entity, bool isAcceptNull = false)
        {
            PropertyInfo[] listProps = this.GetType().GetProperties();
            foreach (var prop in listProps)
            {
                var tarProp = entity.GetType().GetProperty(prop.Name);
                // Kiểm tra xem có thuộc tính trùng tên và có thể gán được hay không.
                if (tarProp != null &&
                    (prop.PropertyType.IsAssignableFrom(tarProp.PropertyType) ||
                     Nullable.GetUnderlyingType(prop.PropertyType) == tarProp.PropertyType ||
                     prop.PropertyType == Nullable.GetUnderlyingType(tarProp.PropertyType)))
                {
                    var value = tarProp.GetValue(entity);
                    if (value != null || isAcceptNull)
                    {
                        // Nếu prop là kiểu nullable và tarProp là kiểu non-nullable (hoặc ngược lại)
                        // thì giá trị có thể được gán tùy thuộc vào kiểu cơ sở tương thích.
                        prop.SetValue(this, value);
                    }
                }
            }
            return this;
        }

    }
}
