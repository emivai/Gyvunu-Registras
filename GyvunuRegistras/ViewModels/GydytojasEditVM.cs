using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels
{
	/// <summary>
    /// View model for editing data of 'Gydytojas' entity.
    /// </summary>
	public class GydytojasEditVM
	{
		/// <summary>
        /// Entity data.
        /// </summary>
        public class GydytojasM
        {
            [DisplayName("Asmens kodas")]
            [Required]
		    public string AsmensKodas { get; set; }

            [DisplayName("Vardas")]
            [Required]
		    public string Vardas { get; set; }

            [DisplayName("Pavardė")]
            [Required]
		    public string Pavarde { get; set; }

            [DisplayName("Telefono numeris")]
            [Required]
		    public string Numeris { get; set; }

		    [DisplayName("El. pašto adresas")]
            [Required]
		    public string ElPastas { get; set; }

            [DisplayName("Klinika")]
            [Required]
            public int Klinika { get; set; }
        
        }

		/// <summary>
        /// Representation of 'Mikroschema' entity in 'Gydytojas' edit form.
        /// </summary>
		public class MikroschemaM
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

            [DisplayName("Gyvunas")]
	    	[Required]
	    	public string Gyvunas { get; set; }

            [DisplayName("Saskaita")]
	    	[Required]
	    	public string Saskaita { get; set; }
        }

		/// <summary>
        /// Select lists for making drop downs for choosing values of entity fields.
        /// </summary>
		public class ListsM
		{
			public IList<SelectListItem> Gyvunai { get; set; }

            public IList<SelectListItem> Klinikos { get; set; }

        	public IList<SelectListItem> Saskaitos {get;set;}
		}


		/// <summary>
        /// Entity view.
        /// </summary>
        public GydytojasM Gydytojas { get; set; } = new GydytojasM();

		/// <summary>
        /// Views of related 'Mikroschemos' records.
        /// </summary>
        public IList<MikroschemaM> Mikroschemos { get; set;  } = new List<MikroschemaM>();

		/// <summary>
        /// Lists for drop down controls.
        /// </summary>
        public ListsM Lists { get; set; } = new ListsM();
	}
}