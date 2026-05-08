namespace StudentCrudAppWithEFCoreCodeFirst.Services
{
    public class StudentGradeService :IStudentGradeService //Transient Service
    {
        public string GetGrade(int marks) 
        {
            if (marks >= 90) return "A";
            if (marks >= 75) return "B";
            if (marks >= 60) return "C";
            return "Fail";
        }
    }
}
