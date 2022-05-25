using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels
{
	/// <summary>
    /// View model for editing data of 'Savininkas' entity.
    /// </summary>
	public class SavininkasEditVM
	{
		/// <summary>
        /// Entity data.
        /// </summary>
        public class SavininkasM
        {
		[DisplayName("Asmens kodas")]
		[Required]
		public string AsmensKodas { get; set; }

        [DisplayName("Vardas")]
		[Required]
		[RegularExpression(@"[a-zA-ZąĄčČęĘėĖįĮšŠųŲūŪžŽ]+", ErrorMessage = "Turi būti sudaryta tik iš raidžių")]
		public string Vardas { get; set; }

        [DisplayName("Pavardė")]
		[Required]
		[RegularExpression(@"[a-zA-ZąĄčČęĘėĖįĮšŠųŲūŪžŽ]+", ErrorMessage = "Turi būti sudaryta tik iš raidžių")]
		public string Pavarde { get; set; }

        [DisplayName("Adresas")]
		[Required]
		public string Adresas { get; set; }

        [DisplayName("Telefono numeris")]
		[Required]
		public string Numeris { get; set; }

		[DisplayName("El. pašto adresas")]
		[Required]
		public string ElPastas { get; set; }
        }

		/// <summary>
        /// Representation of 'Mokejimas' entity in 'Savininkas' edit form.
        /// </summary>
		public class MokejimasM
		{
		/// <summary>
        /// ID of the record in the form. Is used when adding and removing records.
        /// </summary>
        public int InListId { get; set; }

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

		[DisplayName("Sąskaita")]
		public string FkSaskaita { get; set; }
        }

		/// <summary>
        /// Select lists for making drop downs for choosing values of entity fields.
        /// </summary>
		public class ListsM
		{
        	public IList<SelectListItem> Saskaitos {get;set;}
		}

		/// <summary>
        /// Entity view.
        /// </summary>
        public SavininkasM Savininkas { get; set; } = new SavininkasM();

		/// <summary>
        /// Views of related 'Mokejimas' records.
        /// </summary>
        public IList<MokejimasM> Mokejimai { get; set;  } = new List<MokejimasM>();

		/// <summary>
        /// Lists for drop down controls.
        /// </summary>
        public ListsM Lists { get; set; } = new ListsM();
	}
}