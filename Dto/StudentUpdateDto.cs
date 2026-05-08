namespace StudentCrudAppWithEFCoreCodeFirst.Dto
{
    public class StudentUpdateDto
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int StudentAge { get; set; }
        public int StudentMarks { get; set; }
    }
}
