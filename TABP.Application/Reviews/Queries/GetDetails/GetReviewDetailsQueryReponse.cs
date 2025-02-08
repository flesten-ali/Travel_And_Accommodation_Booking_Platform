namespace TABP.Application.Reviews.Queries.GetDetails;
public class GetReviewDetailsQueryReponse
{
    public string Comment { get; set; }
    public int Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ReviwerName { get; set; }
}