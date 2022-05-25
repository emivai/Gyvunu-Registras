using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GyvunuRegistras.Models
{
	/// <summary>
	/// Model for 'GyvunoRusis' entity.
	/// </summary>
	public class GyvunoRusis
	{
		public int Id { get; set; }

		public string Pavadinimas { get; set; }
	}
}