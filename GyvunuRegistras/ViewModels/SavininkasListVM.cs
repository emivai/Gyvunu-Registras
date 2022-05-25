using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels
{
	/// <summary>
	/// Model for 'Savininkas' entity.
	/// </summary>
	public class SavininkasListVM
	{
        [DisplayName("Asmens kodas")]
		public string AsmensKodas { get; set; }

        [DisplayName("Vardas")]
		public string Vardas { get; set; }

        [DisplayName("Pavardė")]
		public string Pavarde { get; set; }

		[DisplayName("Adresas")]
		public string Adresas { get; set; }

        [DisplayName("Telefono numeris")]
		public string Numeris { get; set; }

		[DisplayName("El. pašto adresas")]
		public string ElPastas { get; set; }
	}
}