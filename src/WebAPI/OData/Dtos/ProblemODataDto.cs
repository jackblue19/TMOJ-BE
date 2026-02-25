namespace WebAPI.OData.Dtos;

/// <summary>
/// 1. Cái này là bản ví dụ dto dành riêng cho odata nếu ae muốn có
/// 2. nên tạo riêng từng class dto cho từng cái
/// 3. Chỉ là ví dụ nên cần bổ sung thêm nếu xài (dĩ nhiên là chưa đủ)
/// </summary>
public class ProblemODataDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
