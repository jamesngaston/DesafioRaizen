using DesafioRaizen.Models;
using Humanizer;
using System.ComponentModel.DataAnnotations;

namespace DesafioRaizen.DTO
{
    public class CustomerDto
    {
        public bool cepValidado = false;
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de nascimento")]
        public DateTime BirthDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:#####-###}")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(9, ErrorMessage = "XXXXX-XXX")]
        [MinLength(9, ErrorMessage = "XXXXX-XXX")]
        [Display(Name = "CEP")]
        public string CEP { get; set; } = string.Empty;

        public static implicit operator CustomerDto(Customer customer) =>
            new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                CEP = customer.CEP.Substring(0, 5) + "-" + customer.CEP.Substring(5, 3),
                BirthDate = customer.BirthDate,
                cepValidado = true,
            };

        public static implicit operator Customer(CustomerDto dto) => new Customer
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            CEP = dto.CEP.Replace("-", ""),
            BirthDate = dto.BirthDate
        };
    }
}