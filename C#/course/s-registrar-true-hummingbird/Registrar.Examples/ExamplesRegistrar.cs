using System.Reflection;
using TesterLib;

namespace Registrar;

#pragma warning disable CA1822
class ExamplesRegistrar
{
    public bool TestEmptyListOperations(Tester t)
    {
        IList<int> empty = new EmptyList<int>();
        int sum = 0;
        empty.ForEach(item => sum += item);

        var pushed = empty.PushFront(42);

        return t.CheckExpect(empty.Length(), 0)
            && t.CheckExpect(empty.Any(_ => true), false)
            && t.CheckExpect(empty.Contains(42), false)
            && t.CheckExpect(empty.Equals(new EmptyList<int>()), true)
            && t.CheckExpect(empty.EqualsEmpty(new EmptyList<int>()), true)
            && t.CheckExpect(empty.EqualsLink(new LinkList<int>(1, new EmptyList<int>())), false)
            && t.CheckExpect(sum, 0)
            && t.CheckExpect(empty.Fold(5, (acc, item) => acc + item), 5)
            && t.CheckExpect(pushed.Length(), 1)
            && t.CheckExpect(pushed.Contains(42), true);
    }

    public bool TestLinkListOperations(Tester t)
    {
        IList<int> list = new LinkList<int>(1, new LinkList<int>(2, new EmptyList<int>()));
        var sameList = new LinkList<int>(1, new LinkList<int>(2, new EmptyList<int>()));
        var differentList = new LinkList<int>(1, new EmptyList<int>());

        string order = "";
        list.ForEach(item => order += item.ToString());

        var pushed = list.PushFront(0);
        var foldResult = list.Fold("", (acc, item) => acc + item);

        return t.CheckExpect(list.Length(), 2)
            && t.CheckExpect(list.Any(item => item == 2), true)
            && t.CheckExpect(list.Any(item => item == 3), false)
            && t.CheckExpect(list.Contains(1), true)
            && t.CheckExpect(list.Contains(3), false)
            && t.CheckExpect(list.Equals(sameList), true)
            && t.CheckExpect(list.Equals(differentList), false)
            && t.CheckExpect(list.EqualsEmpty(new EmptyList<int>()), false)
            && t.CheckExpect(list.EqualsLink(sameList), true)
            && t.CheckExpect(list.EqualsLink(differentList), false)
            && t.CheckExpect(order, "12")
            && t.CheckExpect(foldResult, "12")
            && t.CheckExpect(pushed.Length(), 3)
            && t.CheckExpect(pushed.Contains(0), true);
    }

    public bool TestStudentEquals(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);
        var student3 = new Student("Student3", 1);

        return t.CheckExpect(student1.SameStudent(student2), false)
            && t.CheckExpect(student1.SameStudent(student3), true);
    }

    public bool TestCourseConstructor(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);

        var instructor = new Instructor("Instructor1");
        var course1 = new Course("Course1", instructor, FromArray([student1, student2]));

        IList<Student> students = GetField<Course, IList<Student>>("students", course1);

        return t.CheckExpect(students?.Length(), 2)
            && t.CheckExpect(students, FromArray([student1, student2]))
            && t.CheckExpect(student1.HasCourse(course1), true)
            && t.CheckExpect(student2.HasCourse(course1), true)
            && t.CheckExpect(instructor.HasCourse(course1), true);
    }

    public bool TestEnroll(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);

        var instructor = new Instructor("Instructor1");
        var course1 = new Course("Course1", instructor, new EmptyList<Student>());

        student1.Enroll(course1);
        student2.Enroll(course1);

        IList<Student> students = GetField<Course, IList<Student>>("students", course1);

        return t.CheckExpect(students?.Length(), 2)
            && t.CheckExpect(students, FromArray([student1, student2]))
            && t.CheckExpect(student1.HasCourse(course1), true)
            && t.CheckExpect(student2.HasCourse(course1), true)
            && t.CheckExpect(instructor.HasCourse(course1), true);
    }

    public bool TestRegisterEnrollCycle(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);

        var instructor = new Instructor("Instructor1");
        var course1 = new Course("Course1", instructor, new EmptyList<Student>());

        course1.Register(student1);
        course1.Register(student1);
        student1.Enroll(course1);

        IList<Student> students = GetField<Course, IList<Student>>("students", course1);
        IList<Course> courses = GetField<Student, IList<Course>>("courses", student1);

        return t.CheckExpect(students.Length(), 1)
            && t.CheckExpect(courses.Length(), 1)
            && t.CheckExpect(course1.HasStudent(student1), true)
            && t.CheckExpect(student1.HasCourse(course1), true)
            && t.CheckExpect(course1.HasStudent(student2), false)
            && t.CheckExpect(student2.HasCourse(course1), false);
    }

    public bool TestDejaVu(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);
        var student3 = new Student("Student3", 3);

        var instructor1 = new Instructor("Instructor1");

        var course1 = new Course("Course1", instructor1, FromArray([student1, student2]));
        var course2 = new Course("Course2", instructor1, FromArray([student2, student3]));

        return t.CheckExpect(instructor1.DejaVu(student1), false)
            && t.CheckExpect(instructor1.DejaVu(student2), true)
            && t.CheckExpect(instructor1.DejaVu(student3), false);
    }

    public bool TestDejaVuSingleCourse(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);

        var instructor1 = new Instructor("Instructor1");
        var course1 = new Course("Course1", instructor1, FromArray([student1]));

        return t.CheckExpect(instructor1.DejaVu(student1), false)
            && t.CheckExpect(instructor1.DejaVu(student2), false)
            && t.CheckExpect(instructor1.HasCourse(course1), true)
            && t.CheckExpect(student1.HasCourse(course1), true);
    }

    public bool TestInstructorAssignCourse(Tester t)
    {
        var instructor1 = new Instructor("Instructor1");
        var instructor2 = new Instructor("Instructor2");

        var course1 = new Course("Course1", instructor1, new EmptyList<Student>());
        var course2 = new Course("Course2", instructor2, new EmptyList<Student>());

        instructor1.AssignCourse(course1);
        instructor1.AssignCourse(course2);

        IList<Course> courses = GetField<Instructor, IList<Course>>("courses", instructor1);

        return t.CheckExpect(courses.Length(), 2)
            && t.CheckExpect(instructor1.HasCourse(course1), true)
            && t.CheckExpect(instructor1.HasCourse(course2), true)
            && t.CheckExpect(new Instructor("Instructor3").HasCourse(course1), false);
    }

    public bool TestClassmates(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);
        var student3 = new Student("Student3", 3);

        var instructor1 = new Instructor("Instructor1");

        var course1 = new Course("Course1", instructor1, FromArray([student1, student2]));
        var course2 = new Course("Course2", instructor1, FromArray([student2, student3]));

        return t.CheckExpect(student1.Classmates(student2), true)
            && t.CheckExpect(student1.Classmates(student3), false)
            && t.CheckExpect(student2.Classmates(student1), true)
            && t.CheckExpect(student2.Classmates(student3), true)
            && t.CheckExpect(student1.Classmates(student1), true)
            && t.CheckExpect(student3.Classmates(student2), true);
    }

    public bool TestClassmatesNoCourses(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);

        return t.CheckExpect(student1.Classmates(student2), false);
    }

    public bool TestMassiveCourses(Tester t)
    {
        var student1 = new Student("Student1", 1);
        var student2 = new Student("Student2", 2);
        var student3 = new Student("Student3", 3);
        var student4 = new Student("Student4", 4);
        var student5 = new Student("Student5", 5);
        var student6 = new Student("Student6", 6);
        var student7 = new Student("Student7", 7);
        var student8 = new Student("Student8", 8);

        var instructor1 = new Instructor("Instructor1");
        var instructor2 = new Instructor("Instructor2");
        var instructor3 = new Instructor("Instructor3");

        var course1 = new Course("Course1", instructor1, FromArray([student1, student2, student3]));
        var course2 = new Course("Course2", instructor1, FromArray([student2]));
        var course3 = new Course("Course3", instructor1, FromArray([student3, student5]));
        var course4 = new Course("Course4", instructor2, FromArray([student1, student4]));
        var course5 = new Course("Course5", instructor2, FromArray([student2, student5, student5]));
        var course6 = new Course("Course6", instructor3, FromArray([student6, student7, student8]));

        IList<Course> student5Courses = GetField<Student, IList<Course>>("courses", student5);

        return t.CheckExpect(instructor1.DejaVu(student2), true)
            && t.CheckExpect(instructor1.DejaVu(student3), true)
            && t.CheckExpect(instructor1.DejaVu(student5), false)
            && t.CheckExpect(instructor2.DejaVu(student2), false)
            && t.CheckExpect(instructor3.DejaVu(student6), false)
            && t.CheckExpect(instructor1.HasCourse(course1), true)
            && t.CheckExpect(instructor1.HasCourse(course2), true)
            && t.CheckExpect(instructor2.HasCourse(course4), true)
            && t.CheckExpect(instructor3.HasCourse(course6), true)
            && t.CheckExpect(student1.Classmates(student2), true)
            && t.CheckExpect(student1.Classmates(student4), true)
            && t.CheckExpect(student1.Classmates(student5), false)
            && t.CheckExpect(student5.Classmates(student2), true)
            && t.CheckExpect(student2.Classmates(student6), false)
            && t.CheckExpect(student5.HasCourse(course3), true)
            && t.CheckExpect(student5.HasCourse(course5), true)
            && t.CheckExpect(course5.HasStudent(student5), true)
            && t.CheckExpect(course5.HasStudent(student1), false)
            && t.CheckExpect(student5Courses.Length(), 2);
    }

    private static IList<T> FromArray<T>(T[] array)
    {
        IList<T> list = new EmptyList<T>();
        foreach (var item in array)
        {
            list = list.PushFront(item);
        }

        return list;
    }

    private static U GetField<T, U>(string fieldName, T instance) =>
        (U)
            typeof(T)
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(instance)!;
}

