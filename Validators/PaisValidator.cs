using FluentValidation;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Validators
{
    public class PaisValidator : AbstractValidator<Pais>
    {
        public PaisValidator()
        {
            RuleFor(p => p.Nombre)
                .NotEmpty().WithMessage("El nombre del país es obligatorio")
                .MaximumLength(100).WithMessage("El nombre del país no debe superar los 50 caracteres");
        }
    }
}

