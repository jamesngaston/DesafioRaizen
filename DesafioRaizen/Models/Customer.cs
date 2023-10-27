using System.Runtime.CompilerServices;

namespace DesafioRaizen.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string CEP { get; set; } = string.Empty;
    }
}