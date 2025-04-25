namespace WebApplication1.Models
{
    public class CreatePostDto
    {
        public string userId { get; set; }
        public string Name { get; set; }
        public string Gener { get; set; }
        public int Age { get; set; }
        public int Cong { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }

    }
}
