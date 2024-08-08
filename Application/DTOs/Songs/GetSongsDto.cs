namespace Application.DTOs.Songs;

public record GetSongsDto
(
    string SearchTerm,
    string SortByFieldName,
    int Page = 1,
    int PageSize = 20,
    bool SortAscending = true
);