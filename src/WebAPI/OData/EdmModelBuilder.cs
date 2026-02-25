using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using WebAPI.OData.Dtos;

namespace WebAPI.OData;
/// <summary>
/// → nhớ regist odata dto của mn ở đây
/// </summary>
public class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        // /odata/Problems
        builder.EntitySet<ProblemODataDto>("Problems");

        return builder.GetEdmModel();
    }
}
