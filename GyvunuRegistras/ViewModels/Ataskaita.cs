using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels.Ataskaita
{
	/// <summary>
	/// View model for single contract in a report.
	/// </summary>
	public class Gydytojas
	{
        [DisplayName("Vet. Klinika")]
		public string Klinika { get; set; }

        [DisplayName("Miestas")]
		public string Miestas { get; set; }

		[DisplayName("Vardas")]
		public string Vardas { get; set; }

		[DisplayName("Pavardė")]
		public string Pavarde { get; set; }

		[DisplayName("Įterptos mikroschemos")]
		public int Mikroschemos { get; set; }

		[DisplayName("Pasirašyti prašymai")]
		public int Prasymai { get; set; }

        public int MikroschemuSuma { get; set; }

		public int PrasymuSuma { get; set; }
	}

	/// <summary>
	/// View model for whole report.
	/// </summary>
	public class Report
	{
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? DateFrom { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime? DateTo { get; set; }

		public List<Gydytojas> Gydytojai { get; set; }

		public IList<SelectListItem> Miestai { get; set; }

		public int VisoSumaMikroschemu { get; set; }

		public string City {get;set;}

		public int VisoSumaPrasymu { get; set; }
	}
}