using TesterLib;

namespace Registrar;

public interface IList<T>
{
    /// <summary>
    /// Returns a new list with the item added to the front.
    /// </summary>
    IList<T> PushFront(T item);

    /// <summary>
    /// Returns true if any element in the list satisfies the predicate.
    /// </summary>
    bool Any(Func<T, bool> predicate);

    /// <summary>
    /// Returns true if the list contains the item.
    /// </summary>
    bool Contains(T item) => Any(x => x is not null && x.Equals(item));

    /// <summary>
    /// Returns the number of items in the list.
    /// </summary>
    int Length();

    /// <summary>
    /// Returns true if this list is equal to the other list.
    /// </summary>
    bool Equals(IList<T> otherList);
    bool EqualsLink(LinkList<T> otherList);
    bool EqualsEmpty(EmptyList<T> otherList);

    /// <summary>
    /// Loops through the list and calls the action on each item.
    /// </summary>
    void ForEach(Action<T> action);

    /// <summary>
    /// Folds every item in the list into a single value.
    /// </summary>
    U Fold<U>(U initial, Func<U, T, U> fn);
}

public class EmptyList<T> : IList<T>
{
    public IList<T> PushFront(T item) => new LinkList<T>(item, this);

    public bool Any(Func<T, bool> predicate) => false;

    public int Length() => 0;

    public bool Equals(IList<T> otherList) => otherList.EqualsEmpty(this);

    public bool EqualsLink(LinkList<T> otherList) => false;

    public bool EqualsEmpty(EmptyList<T> otherList) => true;

    public void ForEach(Action<T> action) { }

    public U Fold<U>(U initial, Func<U, T, U> fn) => initial;
}

public class LinkList<T>(T first, IList<T> rest) : IList<T>
{
    private readonly T first = first;
    private readonly IList<T> rest = rest;

    public IList<T> PushFront(T item) => new LinkList<T>(item, this);

    public bool Any(Func<T, bool> predicate) => predicate(first) || rest.Any(predicate);

    public int Length() => 1 + rest.Length();

    public bool Equals(IList<T> otherList) => otherList.EqualsLink(this);

    public bool EqualsLink(LinkList<T> otherList) =>
        first is not null && first.Equals(otherList.first) && rest.Equals(otherList.rest);

    public bool EqualsEmpty(EmptyList<T> otherList) => false;

    public void ForEach(Action<T> action)
    {
        action(first);
        rest.ForEach(action);
    }

    public U Fold<U>(U initial, Func<U, T, U> fn) => rest.Fold(fn(initial, first), fn);
}

public class Course
{
    private readonly string name;
    private readonly Instructor prof;
    private IList<Student> students;

    public Course(string name, Instructor prof, IList<Student> students)
    {
        this.name = name;
        this.prof = prof;
        this.students = students;

        this.prof.AssignCourse(this);
        this.students.ForEach(student => student.Enroll(this));
    }

    /// <summary>
    /// Register the given student to this course
    /// </summary>
    public void Register(Student student)
    {
        if (!HasStudent(student))
        {
            students = students.PushFront(student);
            student.Enroll(this);
        }
    }

    /// <summary>
    /// Checks if this course has the given student
    /// </summary>
    public bool HasStudent(Student student) => students.Any(s => s.SameStudent(student));
}

public class Instructor
{
    private readonly string name;
    private IList<Course> courses;

    public Instructor(string name)
    {
        this.name = name;
        courses = new EmptyList<Course>();
    }

    public void AssignCourse(Course course)
    {
        if (!HasCourse(course))
        {
            courses = courses.PushFront(course);
        }
    }

    /// <summary>
    /// Determines whether the given Student is in more than one of this Instructor’s Courses.
    /// </summary>
    public bool DejaVu(Student student) =>
        courses.Fold(0, (acc, course) => (course.HasStudent(student) ? 1 : 0) + acc) > 1;

    /// <summary>
    /// Checks if this instructor is enrolled in the given course
    /// </summary>
    public bool HasCourse(Course course) => courses.Contains(course);
}

public class Student
{
    private readonly string name;
    private readonly int id;
    private IList<Course> courses;

    public Student(string name, int id)
    {
        this.name = name;
        this.id = id;
        courses = new EmptyList<Course>();
    }

    /// <summary>
    /// Enroll this student to the given course
    /// </summary>
    public void Enroll(Course course)
    {
        if (!HasCourse(course))
        {
            courses = courses.PushFront(course);
            course.Register(this);
        }
    }

    /// <summary>
    /// Determines whether the given Student is in any of the same classes as this Student.
    /// </summary>
    public bool Classmates(Student student) => courses.Any(course => course.HasStudent(student));

    /// <summary>
    /// Checks if this student is enrolled in the given course
    /// </summary>
    public bool HasCourse(Course course) => courses.Contains(course);

    /// <summary>
    /// Checks if this student is the same as the given student. Student id must be unique per distinct student
    /// </summary>
    public bool SameStudent(Student otherStudent) => id == otherStudent.id;
}

