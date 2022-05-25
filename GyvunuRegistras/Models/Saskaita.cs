using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Models
{
	/// <summary>
	/// Model for 'Saskaita' entity.
	/// </summary>
	public class Saskaita
	{
		[DisplayName("Numeris")]
		[Required]
		public string Numeris { get; set; }

		[DisplayName("Data")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required]
		public DateTime? Data { get; set; }

		[DisplayName("Suma")]
		[Required]
		[Range(0.01, 10000)]
		public decimal Suma { get; set; }
	}
}