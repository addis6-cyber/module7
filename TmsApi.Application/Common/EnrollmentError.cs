namespace TmsApi.Application.Common;

public sealed record EnrollmentError(
    string Code,
    string Message)
{
    public static EnrollmentError CourseNotFound(string code) =>
        new(
            "course_not_found",
            $"Course '{code}' was not found.");

    public static EnrollmentError StudentNotFound(int id) =>
        new(
            "student_not_found",
            $"Student '{id}' was not found.");

    public static EnrollmentError AlreadyEnrolled(int studentId, string courseCode) =>
        new(
            "already_enrolled",
            $"Student '{studentId}' is already enrolled in '{courseCode}'.");
}