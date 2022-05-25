using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using GyvunuRegistras.Repositories;
using GyvunuRegistras.Models;
using GyvunuRegistras.ViewModels;


namespace GyvunuRegistras.Controllers
{
	/// <summary>
	/// Controller for working with 'Gydytojas' entity.
	/// </summary>
	public class GydytojasController : Controller
	{
		/// <summary>
		/// This is invoked when either 'Index' action is requested or no action is provided.
		/// </summary>
		/// <returns>Entity list view.</returns>
		public ActionResult Index()
		{
			return View(GydytojasRepo.List());
		}

		/// <summary>
		/// This is invoked when creation form is first opened in a browser.
		/// </summary>
		/// <returns>Entity creation form.</returns>
		public ActionResult Create()
		{
			var GydytojasEvm = new GydytojasEditVM();

			PopulateLists(GydytojasEvm);

			return View(GydytojasEvm);
		}


		/// <summary>
		/// This is invoked when buttons are pressed in the creation form.
		/// </summary>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="GydytojasEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns creation from view or redirets back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Create(int? save, int? add, int? remove, GydytojasEditVM GydytojasEvm)
		{
			//do not allow creation of entity with 'asmensKodas' field matching existing one
			var match = GydytojasRepo.Find(GydytojasEvm.Gydytojas.AsmensKodas);

			if( match.Gydytojas.AsmensKodas != null )
			{
				ModelState.AddModelError("Gydytojas.AsmensKodas", "Field value already exists in database.");
				PopulateLists(GydytojasEvm);
				return View(GydytojasEvm);
			}
			//addition of new 'Mikroschemos' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new GydytojasEditVM.MikroschemaM {
						InListId =
							GydytojasEvm.Mikroschemos.Count > 0 ?
							GydytojasEvm.Mikroschemos.Max(it => it.InListId) + 1 :
							0,

						Data = DateTime.Now
					};

				GydytojasEvm.Mikroschemos.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(GydytojasEvm);
				return View(GydytojasEvm);
			}

			//removal of existing 'Mikroschemos' record was requested?
			if( remove != null )
			{
				//filter out 'Mikroschemos' record having in-list-id the same as the given one
				GydytojasEvm.Mikroschemos =
					GydytojasEvm
						.Mikroschemos
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(GydytojasEvm);
				return View(GydytojasEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{
					foreach( var upVm in GydytojasEvm.Mikroschemos )
					{
						var match2 = MikroschemaRepo.Find(upVm.Numeris);
						var match3 = MikroschemaRepo.FindAnimal(upVm.Gyvunas);

						if( match2.Numeris != null)
						{
							ModelState.AddModelError("", $"Mikroschema su numeriu { match2.Numeris} jau yra duombazėje.");
							PopulateLists(GydytojasEvm);
							return View(GydytojasEvm);
						}
						else if (match3.Gyvunas != null)
						{
							ModelState.AddModelError("", $"Gyvūnas {GyvunasRepo.Find(match3.Gyvunas).Vardas} {match3.Gyvunas} jau turi mikroschemą.");
							PopulateLists(GydytojasEvm);
							return View(GydytojasEvm);
						}
					}

					GydytojasRepo.Insert(GydytojasEvm);

					foreach( var upVm in GydytojasEvm.Mikroschemos )
						MikroschemaRepo.Insert(GydytojasEvm.Gydytojas.AsmensKodas, upVm);

					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateLists(GydytojasEvm);
					return View(GydytojasEvm);
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
			var GydytojasEvm = GydytojasRepo.Find(id);
			
			GydytojasEvm.Mikroschemos = MikroschemaRepo.List(id);			
			PopulateLists(GydytojasEvm);

			return View(GydytojasEvm);
		}

		/// <summary>
		/// This is invoked when buttons are pressed in the editing form.
		/// </summary>
		/// <param name="id">ID of the entity being edited</param>
		/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
		/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
		/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
		/// <param name="GydytojasEvm">Entity view model filled with latest data.</param>
		/// <returns>Returns editing from view or redired back to Index if save is successfull.</returns>
		[HttpPost]
		public ActionResult Edit(string id, int? save, int? add, int? remove, GydytojasEditVM GydytojasEvm)
		{
			//addition of new 'Mikroschemos' record was requested?
			if( add != null )
			{
				//add entry for the new record
				var up =
					new GydytojasEditVM.MikroschemaM {
						InListId =
							GydytojasEvm.Mikroschemos.Count > 0 ?
							GydytojasEvm.Mikroschemos.Max(it => it.InListId) + 1 :
							0,
                        Data = DateTime.Now
					};
				GydytojasEvm.Mikroschemos.Add(up);

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(GydytojasEvm);
				return View(GydytojasEvm);
			}

			//removal of existing 'Mikroschemos' record was requested?
			if( remove != null )
			{
				//filter out 'Mikroschemos' record having in-list-id the same as the given one
				GydytojasEvm.Mikroschemos =
					GydytojasEvm
						.Mikroschemos
						.Where(it => it.InListId != remove.Value)
						.ToList();

				//make sure @Html helper is not reusing old model state containing the old list
				ModelState.Clear();

				//go back to the form
				PopulateLists(GydytojasEvm);
				return View(GydytojasEvm);
			}

			//save of the form data was requested?
			if( save != null )
			{
				//form field validation passed?
				if( ModelState.IsValid )
				{
					//update 'Gydytojas'
					GydytojasRepo.Update(GydytojasEvm);

					//delete all old 'Mikroschemos' records
					MikroschemaRepo.DeleteForGydytojas(GydytojasEvm.Gydytojas.AsmensKodas);

					//create new 'Mikroschemos' records
					foreach( var upVm in GydytojasEvm.Mikroschemos )
					{
						try
						{
							MikroschemaRepo.Insert(GydytojasEvm.Gydytojas.AsmensKodas, upVm);
						}
						catch ( MySql.Data.MySqlClient.MySqlException )
						{
							ModelState.AddModelError("", $"Mikroschema su numeriu {upVm.Numeris} jau yra duombazėje arba gyvūnas {GyvunasRepo.Find(upVm.Gyvunas).Vardas} {upVm.Gyvunas} jau turi mikroschemą");
							PopulateLists(GydytojasEvm);
							return View(GydytojasEvm);
						}
					}

					//save success, go back to the entity list
					return RedirectToAction("Index");
				}
				//form field validation failed, go back to the form
				else
				{
					PopulateLists(GydytojasEvm);
					return View(GydytojasEvm);
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
			var GydytojasEvm = GydytojasRepo.Find(id);
			return View(GydytojasEvm);
		}

		/// <summary>
		/// This is invoked when deletion is confirmed in deletion form
		/// </summary>
		/// <param name="id">ID of the entity to delete.</param>
		/// <returns>Deletion form view on error, redirects to Index on success.</returns>
		[HttpPost]
		public ActionResult DeleteConfirm(string id)
		{
			//load 'Gydytojas'
			var GydytojasEvm = GydytojasRepo.Find(id);

			//'Gydytojas' is in the state where deletion is permitted?
			try
			{
				//delete the entity
				//MikroschemaRepo.DeleteForGydytojas(id);
				GydytojasRepo.Delete(id);

				//redired to list form
				return RedirectToAction("Index");
			}
			//'Gydytojas' is in state where deletion is not permitted
			catch
			{
				//enable explanatory message and show delete form
				ViewData["deletionNotPermitted"] = true;
				return View("Delete", GydytojasEvm);
			}
		}

		/// <summary>
		/// Populates select lists used to render drop down controls.
		/// </summary>
		/// <param name="GydytojasEvm">'Gydytojas' view model to append to.</param>
		private void PopulateLists(GydytojasEditVM GydytojasEvm)
		{
			//load entities for the select lists
            var klinikos = VeterinarijosKlinikaRepo.List();
            var saskaitos = SaskaitaRepo.List();
            var gyvunai = GyvunasRepo.List();
			

			//build select lists
			GydytojasEvm.Lists.Klinikos =
				klinikos
					.Select(it =>
					{
						return
							new SelectListItem
							{
								Value = Convert.ToString(it.Id),
								Text = $"{it.Pavadinimas} ({it.Adresas}, {it.Miestas})"
							};
					})
					.ToList();

			//build select lists for 'Mikroschemos'
			{
				//initialize the destination list
				GydytojasEvm.Lists.Saskaitos = new List<SelectListItem>();

				//load 'Saskaitos' to use for item groups
				var Saskaitos = SaskaitaRepo.List();

				//create select list items from 'PaslauguKainos' related to each 'Saskaitos'
				foreach( var Saskaita in Saskaitos )
				{
					//create list item group for current 'saskaitos' entity
					var itemGrp = new SelectListGroup() { Name = Saskaita.Numeris };

					var sle = new SelectListItem
					{
						Value = Saskaita.Numeris,
						Text = $"{Saskaita.Suma} EUR {String.Format("{0:d}", Saskaita.Data)}",
						Group = itemGrp
					};

					GydytojasEvm.Lists.Saskaitos.Add(sle);
				}

                {
				    //initialize the destination list
				    GydytojasEvm.Lists.Gyvunai = new List<SelectListItem>();

				    //load 'Saskaitos' to use for item groups
				    var Gyvunai = GyvunasRepo.List();

				    //create select list items from 'PaslauguKainos' related to each 'Saskaitos'
				    foreach( var gyvunas in Gyvunai )
				    {
					    //create list item group for current 'saskaitos' entity
					    var itemGrp = new SelectListGroup() { Name = $" {gyvunas.Id}, {GyvunuRusysRepo.Find(gyvunas.Rusis).Pavadinimas}, {GyvunuLytysRepo.Find(gyvunas.Lytis).Pavadinimas}" };

				    	var sle = new SelectListItem
				    	{
				    		Value = gyvunas.Id,
				    		Text = $"{gyvunas.Vardas} ({gyvunas.Veisle})",
				    		Group = itemGrp
				    	};

					    GydytojasEvm.Lists.Gyvunai.Add(sle);
					}
				}
			}
		}
	}
}