using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories;
using Org.Ktu.Isk.P175B602.GyvunuRegistras.Models;
using Ataskaita = Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels.Ataskaita;

namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Controllers
{
	/// <summary>
	/// Controller for producing reports.
	/// </summary>
	public class AtaskaitaController : Controller
	{
		/// <summary>
		/// Produces contracts report.
		/// </summary>
		/// <param name="dateFrom">Starting date. Can be null.</param>
		/// <param name="dateTo">Ending date. Can be null.</param>
		/// <returns>Report view.</returns>
		public ActionResult Index(DateTime? dateFrom, DateTime? dateTo, string City)
		{
			var report = new Ataskaita.Report();
			report.DateFrom = dateFrom;
			report.DateTo = dateTo?.AddHours(23).AddMinutes(59).AddSeconds(59);
			PopulateLists(report);

            report.City = City;

			report.Gydytojai = AtaskaitaRepo.GetDoctors(report.DateFrom, report.DateTo, report.City);

			AtaskaitaRepo.GetTotal(report, report.DateFrom, report.DateTo, report.City);

			return View(report);
		}

		private void PopulateLists(Ataskaita.Report rep)
		{
			var miestai = MiestasRepo.List();

			rep.Miestai =
				miestai
					.Select(it =>
					{
						return
							new SelectListItem
							{
								Value = it.Pavadinimas,
								Text = $"{it.Pavadinimas}"
							};
					})
					.ToList();

		}
	}
}
