using KnowledgeSharingApi.Domains.Annotations.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSharingApi.Domains.Interfaces.ModelInterfaces.ApiResponseModelInterfaces
{
    [ResponseUserItemConverter]
    public interface IResponseUserItemModel
    {
        // Stars:
        double? AverageStars { get; set; }
        double? MyStars { get; set; }
        int TotalStars { get; set; }
    }
}
