using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Submissions;

public sealed class SubmissionCreatedEvent
{
    public Guid SubmissionId { get; init; }
    public int ProblemId { get; init; }
    public int UserId { get; init; }
    public string Language { get; init; } = default!;
    public string Code { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
}

/*        WebAPI publish event
 *            
await _eventPublisher.PublishAsync(
    new SubmissionCreatedEvent
    {
        SubmissionId = submission.Id,
        ProblemId = submission.ProblemId,
        UserId = submission.UserId,
        Language = submission.Language,
        Code = submission.Code,
        CreatedAt = DateTime.UtcNow
    });
*/
