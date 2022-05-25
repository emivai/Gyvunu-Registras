using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Models
{
	/// <summary>
	/// Model for 'Gyvunas' entity.
	/// </summary>
	public class Gyvunas
	{
		[DisplayName("Id")]
		[Required]
		public string Id { get; set; }

		[DisplayName("Vardas")]
		[Required]
		public string Vardas { get; set; }

		[DisplayName("Gimimo spalva")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required]
		public DateTime? GimimoData { get; set; }

        [DisplayName("Veisle")]
		[Required]
		public string Veisle { get; set; }

        [DisplayName("Kailio spalva")]
		[Required]
		public string Kailis { get; set; }

        [DisplayName("Rusis")]
		[Required]
		public int Rusis { get; set; }

        [DisplayName("Lytis")]
		[Required]
		public int Lytis { get; set; }

        [DisplayName("Savininkas")]
		[Required]
		public string FkSavininkas { get; set; }
	}
}