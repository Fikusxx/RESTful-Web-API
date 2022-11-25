using WebAPI.Models;
using WebAPI.ResourceParameters;
using WebAPI.Utilities;

namespace WebAPI.Services;

public interface IRepository
{
    IEnumerable<Course> GetCourses(Guid authorId);
    Course GetCourse(Guid authorId, Guid courseId);
    void AddCourse(Guid authorId, Course course);
    void UpdateCourse(Course course);
    void DeleteCourse(Course course);
    PagedList<Author> GetAuthors(AuthorsResourceParameters parameters);
    Author GetAuthor(Guid authorId);
    IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds);
    void AddAuthor(Author author);
    void DeleteAuthor(Author author);
    void UpdateAuthor(Author author);
    bool AuthorExists(Guid authorId);
    bool Save();
}
