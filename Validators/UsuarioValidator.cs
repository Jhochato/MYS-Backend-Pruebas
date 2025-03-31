using FluentValidation;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio")
                .MinimumLength(5).WithMessage("El nombre de usuario debe tener al menos 5 caracteres");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres");

            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres");

            RuleFor(u => u.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(100).WithMessage("Los apellidos no deben superar los 100 caracteres");
        }
    }
}
