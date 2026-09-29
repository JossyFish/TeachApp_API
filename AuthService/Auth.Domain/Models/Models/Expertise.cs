namespace Auth.Domain.Models.Models
{
    public class Expertise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;


        public Expertise() { }

        public Expertise(int id, string name)
        {
            Id = id;
            Name = name;
        }

    }
}
