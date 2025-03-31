using FluentValidation;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Validators
{
    public class ActorValidator : AbstractValidator<Actor>
    {
        public ActorValidator()
        {
            RuleFor(a => a.Nombre)
                .NotEmpty().WithMessage("El nombre del actor es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres");

            RuleFor(a => a.Apellidos)
                .NotEmpty().WithMessage("Los apellidos del actor son obligatorios")
                .MaximumLength(100).WithMessage("Los apellidos no deben superar los 100 caracteres");

            RuleFor(a => a.Pais)
                .NotEmpty().WithMessage("El país es obligatorio");
        }
    }
}