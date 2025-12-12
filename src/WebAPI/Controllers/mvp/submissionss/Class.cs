namespace WebAPI.Controllers.mvp.submissionss
{
    public class Class
    {
        //  bản chất ở đây thì sẽ có train cơ bản về tư duy của mediatr + specification (nhớ nhắc -,- kẻo hay quên)
        //  sẽ hiểu được lợi thế điểm mạnh lợi ích của design pattern mang lại
        //  keyword cho mọi người tìm hiểu trước là:
            //  1. read and write
            //  2. CQRs
            //  3. MediatR
            //  4. Specification
            //  5. Clean architecture
            //  6. Hexagonal arch (base application thì có áp dụng cái này)
        //  đoạn dưới là 1 mẫu code dùng meditar → tạm hiểu là dùng này thì đếch care application layer code chó gì
                                                    //  chỉ cần gọi đúng cqrs (tức là usecase) là được
                                                    //  UploadSubmission là tên của usecase đó
    }
    /*[HttpPost("submissions")]
public async Task<IActionResult> Upload(IFormFile file, Guid problemId, string lang, [FromServices] IMediator mediator)
{
    using var stream = file.OpenReadStream();
    var id = await mediator.Send(new UploadSubmission(CurrentUserId, problemId, stream, lang));
    return Created($"/submissions/{id}", new { id });
}
*/
}
