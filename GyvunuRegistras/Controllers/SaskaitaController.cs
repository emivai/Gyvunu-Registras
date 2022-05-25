using Microsoft.AspNetCore.Mvc;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories;
using Org.Ktu.Isk.P175B602.GyvunuRegistras.Models;
using Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Controllers
{
	/// <summary>
	/// Controller for working with 'Saskaita' entity.
	/// </summary>
	public class SaskaitaController : Controller
	{
		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			//gražinamas darbuotoju sarašo vaizdas
			return View(SaskaitaRepo.List());
		}

		/// <summary>
		/// This is invoked when creation form is first opened in browser.
		/// </summary>
		/// <returns>Creation form view.</returns>
		public ActionResult Create()
		{
			var sask = new Saskaita();
			return View(sask);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="darb">Entity model filled with latest data.</param>
		/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(Saskaita sask)
		{
			//do not allow creation of entity with 'numeris' field matching existing one
			var match = SaskaitaRepo.Find(sask.Numeris);

			if( match != null )
				ModelState.AddModelError("numeris", "Field value already exists in database.");

            if( sask.Numeris.Length != 5 )
				ModelState.AddModelError("numeris", "Field must contain 5 digits");

			//form field validation passed?
			if (ModelState.IsValid)
			{
				SaskaitaRepo.Insert(sask);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			
			//form field validation failed, go back to the form
			return View(sask);
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(string id)
		{
			return View(SaskaitaRepo.Find(id));
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>		
		/// <param name="darb">Entity model filled with latest data.</param>
		/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(string id, Saskaita sask)
		{
			//form field validation passed?
			if (ModelState.IsValid)
			{
				SaskaitaRepo.Update(sask);

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}

			//form field validation failed, go back to the form
			return View(sask);
		}

		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(string id)
		{
			var sask = SaskaitaRepo.Find(id);
			return View(sask);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(string id)
		{
			//try deleting, this will fail if foreign key constraint fails
			try 
			{
				SaskaitaRepo.Delete(id);

				//deletion success, redired to list form
				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;

				var sask = SaskaitaRepo.Find(id);
				return View("Delete", sask);
			}
		}
	}
}
