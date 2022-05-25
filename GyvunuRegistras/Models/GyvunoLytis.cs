using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GyvunuRegistras.Models
{
	/// <summary>
	/// Model for 'GyvunoLytis' entity.
	/// </summary>
	public class GyvunoLytis
	{
		public int Id { get; set; }

		public string Pavadinimas { get; set; }
	}
}