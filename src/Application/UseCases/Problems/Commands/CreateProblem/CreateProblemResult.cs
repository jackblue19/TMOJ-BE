using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Problems.Commands.CreateProblem;
public record CreateProblemResult(Guid Id);

// optional tuỳ thuộc mà có hay ko, tuỳ ae thích config thế nào
// 1 dạng như dto ấy (xem nó như kiểu result ~= responsedto)

/*      ver 2
public record CreateProblemResult(
    Guid Id ,
    string Slug 
);
*/

//public sealed record CreateProblemResponse(Guid Id , string Slug);        
//   có thể là gọi response thay vì result cho dễ hiểu cũng đc
