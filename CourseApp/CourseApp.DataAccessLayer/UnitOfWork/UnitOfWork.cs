using CourseApp.DataAccessLayer.Abstract;
using CourseApp.DataAccessLayer.Concrete;

namespace CourseApp.DataAccessLayer.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private Lazy<IStudentRepository> _studentRepository;
    private Lazy<ILessonRepository> _lessonRepository;
    private Lazy<ICourseRepository> _courseRepository;
    private Lazy<IRegistrationRepository> _registrationRepository;
    private Lazy<IExamRepository> _examRepository;
    private Lazy<IExamResultRepository> _examResultRepository;
    private Lazy<IInstructorRepository> _instructorRepository;

    public UnitOfWork(AppDbContext context, Lazy<IStudentRepository> studentRepository, Lazy<ILessonRepository> lessonRepository, Lazy<ICourseRepository> courseRepository, Lazy<IRegistrationRepository> registrationRepository, Lazy<IExamRepository> examRepository, Lazy<IExamResultRepository> examResultRepository, Lazy<IInstructorRepository> instructorRepository)
    {
        _context = context;
        _studentRepository = studentRepository;
        _lessonRepository = lessonRepository;
        _courseRepository = courseRepository;
        _registrationRepository = registrationRepository;
        _examRepository = examRepository;
        _examResultRepository = examResultRepository;
        _instructorRepository = instructorRepository;
    }

    // TAMAMLANDI-ZOR SEVİYE: Thread-safe değil - Lifetime işlemleri artık Autofac tarafından yönetiliyor.
    public IStudentRepository Students => _studentRepository.Value; 

    public ILessonRepository Lessons => _lessonRepository.Value;

    public ICourseRepository Courses => _courseRepository.Value;

    public IExamRepository Exams => _examRepository.Value;

    public IExamResultRepository ExamResults => _examResultRepository.Value;

    public IInstructorRepository Instructors => _instructorRepository.Value;

    public IRegistrationRepository Registrations => _registrationRepository.Value;

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}