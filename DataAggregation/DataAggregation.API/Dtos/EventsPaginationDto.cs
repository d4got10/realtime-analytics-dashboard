using FastEndpoints;

namespace DataAggregation.API.Dtos;

public class EventsPaginationDto
{
    [QueryParam]
    [BindFrom("page_index")]
    public int PageIndex { get; set; }
    
    [QueryParam]
    [BindFrom("per_page_count")]
    public int PerPageCount { get; set; }
}