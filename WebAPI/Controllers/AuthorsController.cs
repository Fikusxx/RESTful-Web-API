using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Dynamic;
using System.Text.Json;
using WebAPI.Models;
using WebAPI.ResourceParameters;
using WebAPI.Services;
using WebAPI.Utilities;

namespace WebAPI;


[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IRepository db;
    private readonly IMapper mapper;
    private readonly IPropertyCheckerService checkerService;

    public AuthorsController(IRepository db, IMapper mapper, IPropertyCheckerService checkerService)
    {
        this.db = db;
        this.mapper = mapper;
        this.checkerService = checkerService;
    }

    [HttpGet(Name = "GetAuthors")]
    [HttpHead]
    public async Task<IActionResult> GetAuthors([FromQuery] AuthorsResourceParameters parameters)
    {
        if (checkerService.TypeHasProperties<AuthorDTO>(parameters.Fields) == false)
        {
            return BadRequest();
        }

        var authors = db.GetAuthors(parameters);

        var paginationMetaData = new
        {
            totalItemsCount = authors.TotalItemsCount,
            pageSize = authors.PageSize,
            currentPage = authors.CurrentPage,
            totalPages = authors.TotalPages,
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

        var links = CreateLinksForAuthors(parameters, authors.HasNext, authors.HasPrevious);

        var linkedResource = mapper.Map<List<AuthorDTO>>(authors).ShapeDataCollection(parameters.Fields);

        var shapedAuthorsWithLinks = linkedResource.Select(author =>
        {
            var authorAsDictionary = author as IDictionary<string, object>;
            var authorLinks = CreateLinksForAuthor((Guid)authorAsDictionary["Id"], null);
            authorAsDictionary.Add("links", authorLinks);
            return authorAsDictionary;
        });

        var linkedCollectionResource = new
        {
            value = shapedAuthorsWithLinks,
            authorsLinks = links
        };

        return Ok(linkedCollectionResource);
    }

    [HttpGet]
    [Route("{authorId:guid}")]
    [Produces("application/json",
            "application/vnd.marvin.hateoas+json",
            "application/vnd.marvin.author.full+json",
            "application/vnd.marvin.author.full.hateoas+json",
            "application/vnd.marvin.author.friendly+json",
            "application/vnd.marvin.author.friendly.hateoas+json")]
    public async Task<IActionResult> GetAuthor([FromRoute] Guid authorId, [FromQuery] string? fields,
        [FromHeader(Name = "Accept")] string mediaType)
    {
        if (MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType) == false)
        {
            return BadRequest();
        }

        if (checkerService.TypeHasProperties<AuthorDTO>(fields) == false)
        {
            return BadRequest();
        }

        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No author with guid {authorId} exists");

        var includeLinks = parsedMediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        IEnumerable<LinkDTO> links = new List<LinkDTO>();

        if (includeLinks)
        {
            links = CreateLinksForAuthor(authorId, fields);
        }

        var primaryMediaType = includeLinks ?
            parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
            : parsedMediaType.SubTypeWithoutSuffix;

        var resource = new ExpandoObject() as IDictionary<string, object>;

        // full
        if (primaryMediaType == "vnd.marvin.author.full")
        {
            resource = mapper.Map<AuthorFullDTO>(author).ShapeData(fields)!;

            if (includeLinks)
            {
                resource?.Add("links", links);
            }
             
            return Ok(resource);
        }

        // friendly
        resource = mapper.Map<AuthorDTO>(author).ShapeData(fields)!;

        if (includeLinks)
            resource?.Add("links", links);

        return Ok(resource);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreationDTO authorCreationDTO)
    {
        var author = mapper.Map<Author>(authorCreationDTO);
        db.AddAuthor(author);
        db.Save();

        var authorDTO = mapper.Map<AuthorDTO>(author);

        var links = CreateLinksForAuthor(authorDTO.Id, null);
        var linkedResource = authorDTO.ShapeData(null) as IDictionary<string, object>;
        linkedResource.Add("links", links);

        return StatusCode(StatusCodes.Status201Created, linkedResource);
    }

    [HttpOptions]
    public IActionResult GetAuthorsOptions()
    {
        Response.Headers.Add("Allow", "GET,OPTIONS,POST");
        return Ok();
    }

    [HttpDelete]
    [Route("{authorId:guid}")]
    public IActionResult DeleteAuthor([FromRoute] Guid authorId)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No author with guid {authorId} exists");

        db.DeleteAuthor(author);
        db.Save();

        return NoContent();
    }

    private string? CreateAuthorsResourceUri(AuthorsResourceParameters parameters, ResourceUriType type)
    {
        switch (type)
        {
            case ResourceUriType.PreviousPage:
                return Url.Link("GetAuthors",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        mainCategory = parameters.MainCategory,
                        searchQuery = parameters.SearchQuery
                    })!;

            case ResourceUriType.NextPage:
                return Url.Link("GetAuthors",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        mainCategory = parameters.MainCategory,
                        searchQuery = parameters.SearchQuery
                    })!;

            case ResourceUriType.Current:
                return Url.Link("GetAuthors",
                    new
                    {
                        fields = parameters.Fields,
                        orderBy = parameters.OrderBy,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        mainCategory = parameters.MainCategory,
                        searchQuery = parameters.SearchQuery
                    })!;

            default:
                return null;
        }
    }

    private IEnumerable<LinkDTO> CreateLinksForAuthor(Guid authorId, string? fields)
    {
        var links = new List<LinkDTO>();

        if (string.IsNullOrWhiteSpace(fields))
        {
            links.Add(new LinkDTO(Url.ActionLink("GetAuthor", "Authors", new { authorId }), "self", "GET"));
        }
        else
        {
            links.Add(new LinkDTO(Url.ActionLink("GetAuthor", "Authors", new { authorId, fields }), "self", "GET"));
        }

        links.Add(new LinkDTO(Url.ActionLink("GetCoursesByAuthorId", "Courses", new { authorId }), "courses", "GET"));
        links.Add(new LinkDTO(Url.ActionLink("CreateCourseForAuthor", "Courses", new { authorId }), "create_course_for_author", "POST"));
        links.Add(new LinkDTO(Url.ActionLink("DeleteAuthor", "Authors", new { authorId }), "delete_author", "DELETE"));

        return links;
    }

    private IEnumerable<LinkDTO> CreateLinksForAuthors(AuthorsResourceParameters parameters, bool hasNext, bool hasPrevious)
    {
        var links = new List<LinkDTO>();

        links.Add(new LinkDTO(CreateAuthorsResourceUri(parameters, ResourceUriType.Current), "self", "GET"));

        if (hasNext)
            links.Add(new LinkDTO(CreateAuthorsResourceUri(parameters, ResourceUriType.NextPage), "nextPage", "GET"));

        if (hasPrevious)
            links.Add(new LinkDTO(CreateAuthorsResourceUri(parameters, ResourceUriType.PreviousPage), "previousPage", "GET"));

        return links;
    }
}
