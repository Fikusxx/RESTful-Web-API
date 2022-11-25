using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI;

[ApiController]
[Route("api")]
public class RootController : ControllerBase
{

    [HttpGet]
    public IActionResult GetRoot()
    {
        var links = new List<LinkDTO>();

        links.Add(new LinkDTO(Url.ActionLink("GetRoot", "Root", new { }), "self", "GET"));
        links.Add(new LinkDTO(Url.ActionLink("GetAuthors", "Authors", new { }), "authors", "GET"));
        links.Add(new LinkDTO(Url.ActionLink("CreateAuthor", "Authors", new { }), "create_author", "POST"));

        return Ok(links);
    }
}
