using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI;


[ApiController]
[Route("api/authors/{authorId:guid}/[controller]")]
//[ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
public class CoursesController : ControllerBase
{
    private readonly IRepository db;
    private readonly IMapper mapper;

    public CoursesController(IRepository db, IMapper mapper)
    {
        this.db = db;
        this.mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetCoursesByAuthorId(Guid authorId)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No courses exist for author with authorId {authorId}");

        var courses = db.GetCourses(authorId);
        var coursesDTO = mapper.Map<List<CourseDTO>>(courses);

        return Ok(coursesDTO);
    }

    [HttpGet]
    [Route("{courseId:guid}")]
    //[ResponseCache(Duration = 120)]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 100)]
    [HttpCacheValidation(MustRevalidate = true)]
    public IActionResult GetCourseById(Guid authorId, Guid courseId)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No courses exist for author with authorId {authorId}");

        var course = db.GetCourse(authorId, courseId);

        if (course == null)
            return NotFound($"No courses exist for courseId {courseId}");

        var courseDTO = mapper.Map<CourseDTO>(course);

        return Ok(courseDTO);
    }

    [HttpPost]
    public IActionResult CreateCourseForAuthor(Guid authorId, [FromBody] CourseCreationDTO courseCreationDTO)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No author with id {authorId} found");

        var course = mapper.Map<Course>(courseCreationDTO);

        db.AddCourse(authorId, course);
        db.Save();

        var courseDTO = mapper.Map<CourseDTO>(course);

        return StatusCode(StatusCodes.Status201Created, courseDTO);
    }

    [HttpPut]
    [Route("{courseId:guid}")]
    public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, [FromBody] CourseUpdateDTO courseUpdateDTO)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No author with id {authorId} found");

        var course = db.GetCourse(authorId, courseId);

        if (course == null)
        {
            course = new Course() { Id = courseId };
            mapper.Map(courseUpdateDTO, course);
            db.AddCourse(authorId, course);
        }
        else
        {
            mapper.Map(courseUpdateDTO, course);
            db.UpdateCourse(course);
        }

        db.Save();
        var courseDTO = mapper.Map<CourseDTO>(course);

        return Ok(courseDTO);
    }

    [HttpPatch]
    [Route("{courseId:guid}")]
    public IActionResult Patch(Guid authorId, Guid courseId, [FromBody] JsonPatchDocument<CourseUpdateDTO> patchDocument)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No author with id {authorId} found");

        var course = db.GetCourse(authorId, courseId);

        if (course == null)
            return NotFound($"No course with id {courseId} exists");

        var coursePatch = mapper.Map<CourseUpdateDTO>(course);
        patchDocument.ApplyTo(coursePatch, ModelState);

        if (TryValidateModel(coursePatch) == false)
        {
            return ValidationProblem(ModelState);
        }

        mapper.Map(coursePatch, course);
        db.UpdateCourse(course);
        db.Save();
        return Ok(course);
    }

    [HttpDelete]
    [Route("{courseId:guid}")]
    public IActionResult Delete(Guid authorId, Guid courseId)
    {
        var author = db.GetAuthor(authorId);

        if (author == null)
            return NotFound($"No author with id {authorId} found");

        var course = db.GetCourse(authorId, courseId);

        if (course == null)
            return NotFound($"No course with id {courseId} exists");

        db.DeleteCourse(course);
        db.Save();

        return NoContent();
    }

    public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
    {
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>().Value;
        return options.InvalidModelStateResponseFactory(ControllerContext) as ActionResult;
    }
}