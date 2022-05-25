using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GyvunuRegistras.ViewModels
{
	/// <summary>
	/// Model of 'VeterinarijosKlinika' entity used in creation and editing forms.
	/// </summary>
	public class VeterinarijosKlinikaEditVM
	{
		/// <summary>
		/// Entity data
		/// </summary>
		public class ModelM
		{
			[DisplayName("Id")]
			public int Id { get; set; }

			[DisplayName("Pavadinimas")]
			[MaxLength(20)]
			[Required]
			public string Pavadinimas { get; set; }

            [DisplayName("Adresas")]
			[MaxLength(50)]
			[Required]
			public string Adresas { get; set; }

			[DisplayName("Miestas")]
			[Required]
			public int FkMiestas { get; set; }
		}

		/// <summary>
		/// Select lists for making drop downs for choosing values of entity fields.
		/// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Miestai { get; set; }
		}

		/// <summary>
		/// Entity view.
		/// </summary>
		public ModelM Model { get; set; } = new ModelM();

		/// <summary>
		/// Lists for drop down controls.
		/// </summary>
		public ListsM Lists { get; set; } = new ListsM();
	}
}