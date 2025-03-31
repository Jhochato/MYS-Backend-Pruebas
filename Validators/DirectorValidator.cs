using FluentValidation;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Validators
{
    public class DirectorValidator : AbstractValidator<Director>
    {
        public DirectorValidator()
        {
            RuleFor(d => d.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres");

            RuleFor(d => d.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(100).WithMessage("Los apellidos no deben superar los 100 caracteres");

            RuleFor(d => d.Pais)
                .NotEmpty().WithMessage("El país es obligatorio");
        }
    }
}
