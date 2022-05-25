using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Models
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