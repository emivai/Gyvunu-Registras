using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories;
using Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Controllers
{
	/// <summary>
	/// Controller for working with 'Savininkas' entity.
	/// </summary>
	public class SavininkasController : Controller
	{
		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			return View(SavininkasRepo.List());
		}

		/// <summary>
		/// This is invoked when creation form is first opened in a browser.
		/// </summary>
		/// <returns>Entity creation form.</returns>
		public ActionResult Create()
		{
			var SavininkasEvm = new SavininkasEditVM();
			
			PopulateLists(SavininkasEvm);

			return View(SavininkasEvm);
		}


		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="SavininkasEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns creation from view or redirets back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(int? save, int? add, int? remove, SavininkasEditVM SavininkasEvm)
		{
			//do not allow creation of entity with 'asmensKodas' field matching existing one
			var match = SavininkasRepo.Find(SavininkasEvm.Savininkas.AsmensKodas);

			if( match.Savininkas.AsmensKodas != null )
			{
				ModelState.AddModelError("Savininkas.AsmensKodas", "Field value already exists in database.");
				PopulateLists(SavininkasEvm);
				return View(SavininkasEvm);
			}

			//addition of new 'Mokejimai' record was requested?
			if( add != null )
			{				
				//add entry for the new record
				var up =
					new SavininkasEditVM.MokejimasM {
						InListId =
							SavininkasEvm.Mokejimai.Count > 0 ?
							SavininkasEvm.Mokejimai.Max(it => it.InListId) + 1 :
							0,

                        Data = DateTime.Now,
						Suma = 0
					};
				SavininkasEvm.Mokejimai.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(SavininkasEvm);
				return View(SavininkasEvm);
			}

			//removal of existing 'Mokejimai' record was requested?
			if( remove != null )
			{
				//filter out 'Mokejimai' record having in-list-id the same as the given one
				SavininkasEvm.Mokejimai =
					SavininkasEvm
						.Mokejimai
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(SavininkasEvm);
				return View(SavininkasEvm);
			}

			//save of the form data was requested?
			if( save != null)
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{					
					foreach( var upVm in SavininkasEvm.Mokejimai )
					{
						var match2 = MokejimasRepo.Find(upVm.Numeris);

						if( match2.Numeris != null )
						{
							ModelState.AddModelError("", $"Mokėjimas su numeriu { match2.Numeris} jau yra duombazėje.");
							PopulateLists(SavininkasEvm);
							return View(SavininkasEvm);
						}
					}

					SavininkasRepo.Insert(SavininkasEvm);

					foreach( var upVm in SavininkasEvm.Mokejimai )
						MokejimasRepo.Insert(SavininkasEvm.Savininkas.AsmensKodas, upVm);

					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateLists(SavininkasEvm);
					return View(SavininkasEvm);
				}
			}

			//should not reach here
			throw new Exception("Should not reach here.");
		}

		/// <summary>
		/// This is invoked when editing form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to edit.</param>
		/// <returns>Editing form view.</returns>
		public ActionResult Edit(string id)
		{
			var SavininkasEvm = SavininkasRepo.Find(id);
			
			SavininkasEvm.Mokejimai = MokejimasRepo.List(id);			
			PopulateLists(SavininkasEvm);

			return View(SavininkasEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="SavininkasEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns editing from view or redired back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(string id, int? save, int? add, int? remove, SavininkasEditVM SavininkasEvm)
		{
			//addition of new 'Mokejimai' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new SavininkasEditVM.MokejimasM {
						InListId =
							SavininkasEvm.Mokejimai.Count > 0 ?
							SavininkasEvm.Mokejimai.Max(it => it.InListId) + 1 :
							0,

                        Data = DateTime.Now,
						Suma = 0
					};
				SavininkasEvm.Mokejimai.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(SavininkasEvm);
				return View(SavininkasEvm);
			}

			//removal of existing 'Mokejimai' record was requested?
			if( remove != null )
			{
				//filter out 'Mokejimai' record having in-list-id the same as the given one
				SavininkasEvm.Mokejimai =
					SavininkasEvm
						.Mokejimai
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(SavininkasEvm);
				return View(SavininkasEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{
					//update 'Savininkas'
					SavininkasRepo.Update(SavininkasEvm);

					//delete all old 'Mokejimai' records
					MokejimasRepo.DeleteForSavininkas(SavininkasEvm.Savininkas.AsmensKodas);

					//create new 'Mokejimai' records
					foreach( var upVm in SavininkasEvm.Mokejimai )
					{
						try
						{
							MokejimasRepo.Insert(SavininkasEvm.Savininkas.AsmensKodas, upVm);
						}
						catch ( MySql.Data.MySqlClient.MySqlException )
						{
							ModelState.AddModelError("", $"Mokėjimas su numeriu { upVm.Numeris} jau yra duombazėje.");
							PopulateLists(SavininkasEvm);
							return View(SavininkasEvm);
						}
					}

					//save success, go back to the entity list
					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateLists(SavininkasEvm);
					return View(SavininkasEvm);
				}
			}

			//should not reach here
			throw new Exception("Should not reach here.");
		}

		/// <summary>
		/// This is invoked when deletion form is first opened in browser.
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view.</returns>
		public ActionResult Delete(string id)
		{
			var SavininkasEvm = SavininkasRepo.Find(id);
			return View(SavininkasEvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(string id)
		{
            
			//load 'Savininkas'
			var SavininkasEvm = SavininkasRepo.Find(id);

            try
			{
				//delete the entity
				//MokejimasRepo.DeleteForSavininkas(id);
				SavininkasRepo.Delete(id);

				//redired to list form
				return RedirectToAction("Index");
			}
			//entity in use, deletion not permitted
			catch( MySql.Data.MySqlClient.MySqlException )
			{
				ViewData["deletionNotPermitted"] = true;
				return View("Delete", SavininkasEvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="SavininkasEvm">'Savininkas' view model to append to.</param>
		private void PopulateLists(SavininkasEditVM SavininkasEvm)
		{

			//build select list for 'Mokejimai'
			{
				//initialize the destination list
				SavininkasEvm.Lists.Saskaitos = new List<SelectListItem>();

				//load 'saskaitos' to use for item groups
				var saskaitos = SaskaitaRepo.List();

				//create select list items from 'PaslauguKainos' related to each 'saskaitos'
				foreach( var saskaita in saskaitos )
				{
					//create list item group for current 'saskaitos' entity
					var itemGrp = new SelectListGroup() { Name = saskaita.Numeris };

					var sle = new SelectListItem
					{
						Value = saskaita.Numeris,
						Text = $"{saskaita.Suma} EUR {String.Format("{0:d}", saskaita.Data)}",
						Group = itemGrp
					};

					SavininkasEvm.Lists.Saskaitos.Add(sle);
				}
			}
		}
	}
}