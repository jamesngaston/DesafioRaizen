using DesafioRaizen.Models;
using System.ComponentModel.DataAnnotations;

namespace DesafioRaizen.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(9, ErrorMessage = "XXXXX-XXX")]
        [MinLength(9, ErrorMessage = "XXXXX-XXX")]
        public string CEP { get; set; } = string.Empty;

        public static implicit operator CustomerDto(Customer customer) => new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            CEP = customer.CEP,
            BirthDate = customer.BirthDate
        };

        public static implicit operator Customer(CustomerDto dto) => new Customer
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            CEP = dto.CEP,
            BirthDate = dto.BirthDate
        };
    }
}