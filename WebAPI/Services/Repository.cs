using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.ResourceParameters;
using WebAPI.Services.MappingService;
using WebAPI.Utilities;

namespace WebAPI.Services;

public class Repository : IRepository
{
    private readonly ApplicationDbContext db;
    private readonly IPropertyMappingService propertyMappingService;

    public Repository(ApplicationDbContext db, IPropertyMappingService propertyMappingService)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
        this.propertyMappingService = propertyMappingService;
    }

    public void AddCourse(Guid authorId, Course course)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }
        // always set the AuthorId to the passed-in authorId
        course.AuthorId = authorId;
        db.Courses.Add(course);
    }

    public void DeleteCourse(Course course)
    {
        db.Courses.Remove(course);
    }

    public Course GetCourse(Guid authorId, Guid courseId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        return db.Courses
          .Where(c => c.AuthorId == authorId && c.Id == courseId).FirstOrDefault();
    }

    public IEnumerable<Course> GetCourses(Guid authorId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return db.Courses
                    .Where(c => c.AuthorId == authorId)
                    .OrderBy(c => c.Title).ToList();
    }

    public void UpdateCourse(Course course)
    {
        // no code in this implementation
    }

    public void AddAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        // the repository fills the id (instead of using identity columns)
        //author.Id = Guid.NewGuid();

        //if (author.Courses != null)
        //{
        //    foreach (var course in author.Courses)
        //    {
        //        course.Id = Guid.NewGuid();
        //    }
        //}

        db.Authors.Add(author);
    }

    public bool AuthorExists(Guid authorId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return db.Authors.Any(a => a.Id == authorId);
    }

    public void DeleteAuthor(Author author)
    {
        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        db.Authors.Remove(author);
    }

    public Author GetAuthor(Guid authorId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return db.Authors.FirstOrDefault(a => a.Id == authorId);
    }

    public PagedList<Author> GetAuthors(AuthorsResourceParameters parameters)
    {
        var categoryName = parameters.MainCategory;
        var searchQuery = parameters.SearchQuery;
        var orderBy = parameters.OrderBy;

        bool isMainCategoryValid = string.IsNullOrWhiteSpace(categoryName) == false;
        bool isSearchQueryValid = string.IsNullOrWhiteSpace(searchQuery) == false;
        bool isOrderValid = string.IsNullOrWhiteSpace(orderBy) == false;

        var collection = db.Authors.AsQueryable();

        if (isMainCategoryValid)
        {
            categoryName = categoryName!.Trim();
            collection = collection.Where(a => a.MainCategory == categoryName);
        }

        if (isSearchQueryValid)
        {
            searchQuery = searchQuery!.Trim();
            collection = collection.Where(a => a.MainCategory.Contains(searchQuery)
            || a.FirstName.Contains(searchQuery) || a.LastName.Contains(searchQuery));
        }

        if (isOrderValid)
        {
            orderBy = orderBy.Trim();

            var authorPropertyMappingDictionary = propertyMappingService.GetPropertyMapping<AuthorDTO, Author>();

            collection = collection.ApplySort(parameters.OrderBy, authorPropertyMappingDictionary);
        }

        return PagedList<Author>.Create(collection, parameters.PageNumber, parameters.PageSize);
    }

    public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
    {
        if (authorIds == null)
        {
            throw new ArgumentNullException(nameof(authorIds));
        }

        return db.Authors.Where(a => authorIds.Contains(a.Id))
            .OrderBy(a => a.FirstName)
            .OrderBy(a => a.LastName)
            .ToList();
    }

    public void UpdateAuthor(Author author)
    {
        // no code in this implementation
    }

    public bool Save()
    {
        return db.SaveChanges() >= 0;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // dispose resources when needed
        }
    }
}
