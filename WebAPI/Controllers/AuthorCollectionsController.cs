using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI;


[ApiController]
[Route("api/[controller]")]
public class AuthorCollectionsController : ControllerBase
{
    private readonly IRepository db;
    private readonly IMapper mapper;

    public AuthorCollectionsController(IRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    [HttpPost]
    public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorCreationDTO> authorCreationCollectionDTO)
    {
        var authorCollection = mapper.Map<IEnumerable<Author>>(authorCreationCollectionDTO);

        foreach (var author in authorCollection)
        {
            db.AddAuthor(author);
        }
        db.Save();

        var authorCollectionDTO = mapper.Map<IEnumerable<AuthorDTO>>(authorCollection);

        return StatusCode(StatusCodes.Status201Created, authorCollectionDTO);
    }
}
