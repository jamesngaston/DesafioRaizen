using DesafioRaizen.Filters.Enums;

namespace DesafioRaizen.Filters
{
    public class CustomerFilter
    {
        public string? Name { get; set; } = null;

        public string? Email { get; set; } = null;

        public DateTime? BirthDate { get; set; } = null;

        public string? CEP { get; set; } = null;

        public OrderBy OrderBy { get; set; } = OrderBy.Id;

        public bool Ascending { get; set; } = false;
    }
}