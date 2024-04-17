using System.ComponentModel.DataAnnotations;
using ManejoPresupuesto.Models.Validaciones;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta//: IValidatableObject
    {

        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(maximumLength:50, MinimumLength =3, ErrorMessage ="La longitud del campo {0} debe de estar entre {2} y {1} caracteres")]
		[PrimeraLetraMayuscula]
		[Remote(action: "VerificarExisteTipoCuenta", controller:"TiposCuentas")]
		public string Nombre { get; set; }
        public int UsuarioId { get; set; }

        public int Orden {  get; set; }


		//validaciones personalizadas
		//public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		//{
		//	if(Nombre != null && Nombre.Length > 0)
		//	{
		//		var primeraLetra = Nombre[0].ToString();
		//		if(primeraLetra != primeraLetra.ToUpper())
		//		{
		//			yield return new ValidationResult("la primera letra debe de ser mayusucla", new[] { nameof(Nombre) });
		//		}
		//	}
		//}

		/*pruebas de otras validaciones por defecto*/
		//[Required(ErrorMessage = "El campo {0} es requerido")]
		//[EmailAddress(ErrorMessage ="El campo debe de ser un correo electronico valido")]
		//public string Email { get; set; }
		//[Range(minimum:18, maximum:130, ErrorMessage ="El error debe de estar entre {1} y {2}")]
		//public int Edad { get; set; }
		//[Url(ErrorMessage ="Debe ser una URL valida")]
		//public string URL {get; set; }
		//[CreditCard(ErrorMessage ="La tarjeta de credito no es valida")]
		//[Display(Name ="Tarjeta de credito")]
		//public string TarjetaDeCredito {  get; set; }
	}
}
