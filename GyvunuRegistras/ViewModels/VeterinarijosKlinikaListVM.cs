using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels
{
	/// <summary>
	/// Model of 'VeterinarijosKlinika' entity used in lists.
	/// </summary>
	public class VeterinarijosKlinikaListVM
	{
		[DisplayName("Id")]
		public int Id { get; set; }

		[DisplayName("Pavadinimas")]
		public string Pavadinimas { get; set; }		

        [DisplayName("Adresas")]
		public string Adresas { get; set; }	

		[DisplayName("Miestas")]
		public string Miestas { get; set; }
	}
}