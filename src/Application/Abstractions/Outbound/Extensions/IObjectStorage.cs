using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Outbound.Extensions
{
    public interface IObjectStorage
    {
        Task<string> PutAsync(string bucket , string key , Stream content , CancellationToken ct);
        //  giả dụ ae ưng ngầu oách thì đó vps thì con khác nhưng cloud đồ thì xài của google hoặc aws
    }
}
