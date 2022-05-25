using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GyvunuRegistras.Models
{
	/// <summary>
	/// Model of 'VeterinarijosKlinika' entity.
	/// </summary>
	public class VeterinarijosKlinika
	{
		[DisplayName("Id")]
		public int Id { get; set; }

		[DisplayName("Pavadinimas")]
		public string Pavadinimas { get; set; }

        [DisplayName("Adresas")]
		public string Adresas { get; set; }

		//Miestas
		[DisplayName("Miestas")]
		public int FkMiestas { get; set; }
	}
}