namespace EduHome.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; } /*burada id nin string olması qarşıq hərfli rəqəmli olduğu üçün qoyulmasədər*/
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public bool IsDeactive { get; set; }
    }
}
