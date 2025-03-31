using FluentValidation;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Validators
{
    public class GeneroValidator : AbstractValidator<Genero>
    {
        public GeneroValidator()
        {
            RuleFor(g => g.Nombre)
                .NotEmpty().WithMessage("El nombre del género es obligatorio")
                .MaximumLength(50).WithMessage("El nombre del género no debe superar los 50 caracteres");
        }
    }
}

