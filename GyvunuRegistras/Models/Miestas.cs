using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GyvunuRegistras.Models
{
	/// <summary>
	/// Model for 'Miestas' entity.
	/// </summary>
	public class Miestas
	{
		[DisplayName("Id")]
		public int id_MIESTAS { get; set; }

		[DisplayName("Pavadinimas")]
		[Required]
		[RegularExpression(@"[a-zA-ZąĄčČęĘėĖįĮšŠųŲūŪžŽ]+", ErrorMessage = "Turi būti sudaryta tik iš raidžių")]
		public string Pavadinimas { get; set; }
	}
}