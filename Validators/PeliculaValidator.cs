using FluentValidation;
using AdmSeriesAnimadasAPI.Models;

namespace AdmSeriesAnimadasAPI.Validators
{
    public class PeliculaValidator : AbstractValidator<Pelicula>
    {
        public PeliculaValidator()
        {
            RuleFor(p => p.Titulo)
                .NotEmpty().WithMessage("El título de la película es obligatorio")
                .MaximumLength(200).WithMessage("El título no debe superar los 200 caracteres");

            RuleFor(p => p.Reseña)
                .NotEmpty().WithMessage("La reseña es obligatoria")
                .MaximumLength(500).WithMessage("La reseña no debe superar los 500 caracteres");

            RuleFor(p => p.CodigoTrailer)
                .NotEmpty().WithMessage("La URL del tráiler es obligatoria")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Debe ser una URL válida para el trailer");

            RuleFor(p => p.ImagenPortada)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Debe ser una URL válida para la imagen de portada");
        }
    }
}