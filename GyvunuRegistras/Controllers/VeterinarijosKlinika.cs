using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using GyvunuRegistras.Repositories;
using GyvunuRegistras.Models;
using GyvunuRegistras.ViewModels;


namespace GyvunuRegistras.Controllers
{
	/// <summary>
	/// Controller for working with 'VeterinarijosKlinika' entity.
	/// </summary>
	public class VeterinarijosKlinikaController : Controller
	{

		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			var klinikos = VeterinarijosKlinikaRepo.List();
			return View(klinikos);
		}

		/// <summary>
		/// This is invoked when creation form is first opened in browser.
		/// </summary>
		/// <returns>Creation form view.</returns>
		public ActionResult Create()
		{
			var klinikaEvm = new VeterinarijosKlinikaEditVM();
			PopulateSelections(klinikaEvm);
			return View(klinikaEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="modelisEvm">Entity model filled with latest data.</param>
		/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(VeterinarijosKlinikaEditVM klinikaEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				VeterinarijosKlinikaRepo.Insert(klinikaEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(klinikaEvm);
			return View(klinikaEvm);
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(int id)
		{
			var klinikaEvm = VeterinarijosKlinikaRepo.Find(id);
			PopulateSelections(klinikaEvm);

			return View(klinikaEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="autoEvm">Entity model filled with latest data.</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(int id, VeterinarijosKlinikaEditVM klinikaEvm)
		{
			//form field validation passed?
			if( ModelState.IsValid )
			{
				VeterinarijosKlinikaRepo.Update(klinikaEvm);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			PopulateSelections(klinikaEvm);
			return View(klinikaEvm);
		}

		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(int id)
		{
			var klinikaLvm = VeterinarijosKlinikaRepo.FindForDeletion(id);
			return View(klinikaLvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(int id)
		{
			//try deleting, this will fail if foreign key constraint fails
			try
			{
				VeterinarijosKlinikaRepo.Delete(id);

				//deletion success, redired to list form
				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var klinikaLvm = VeterinarijosKlinikaRepo.FindForDeletion(id);

				return View("Delete", klinikaLvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="modelisEvm">'Automobilis' view model to append to.</param>
		public void PopulateSelections(VeterinarijosKlinikaEditVM klinikaEvm)
		{
			//load entities for the select lists
			var miestai = MiestasRepo.List();

			//build select lists
			klinikaEvm.Lists.Miestai =
				miestai.Select(it => {
					return
						new SelectListItem() {
							Value = Convert.ToString(it.id_MIESTAS),
							Text = it.Pavadinimas
						};
				})
				.ToList();
		}
	}
}
