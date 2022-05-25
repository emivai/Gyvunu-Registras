using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.Models;
using Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories
{
	public class VeterinarijosKlinikaRepo
	{
		public static List<VeterinarijosKlinikaListVM> List()
		{
			var result = new List<VeterinarijosKlinikaListVM>();

			var query =
				$@"SELECT
					kl.id_VETERINARIJOS_KLINIKA,
					kl.pavadinimas,
                    kl.adresas,
					mies.pavadinimas AS miestas
				FROM
					veterinarijos_klinikos kl
					LEFT JOIN miestai mies ON mies.id_MIESTAS=kl.fk_MIESTASid_MIESTAS 
				ORDER BY mies.pavadinimas ASC, kl.id_VETERINARIJOS_KLINIKA ASC";

			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new VeterinarijosKlinikaListVM
				{
					Id = Convert.ToInt32(item["id_VETERINARIJOS_KLINIKA"]),
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
                    Adresas = Convert.ToString(item["adresas"]),
					Miestas = Convert.ToString(item["miestas"])
				});
			}

			return result;
		}

		public static List<VeterinarijosKlinika> ListForMiestas(int miestasId)
		{
			var result = new List<VeterinarijosKlinika>();

			var query = $@"SELECT * FROM veterinarijos_klinikos WHERE fk_MIESTASid_MIESTAS=?miestasId ORDER BY id_VETERINARIJOS_KLINIKA ASC";

			var dt =
				Sql.Query(query, args => {
					args.Add("?miestasId", MySqlDbType.Int32).Value = miestasId;
				});

			foreach( DataRow item in dt )
			{
				result.Add(new VeterinarijosKlinika
				{
					Id = Convert.ToInt32(item["id_VETERINARIJOS_KLINIKA"]),
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
                    Adresas = Convert.ToString(item["adresas"]),
					FkMiestas = Convert.ToInt32(item["fk_MIESTASid_MIESTAS"])
				});
			}

			return result;
		}

		public static VeterinarijosKlinikaEditVM Find(int id)
		{
			var mevm = new VeterinarijosKlinikaEditVM();

			var query = $@"SELECT * FROM veterinarijos_klinikos WHERE id_VETERINARIJOS_KLINIKA=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mevm.Model.Id = Convert.ToInt32(item["id_VETERINARIJOS_KLINIKA"]);
				mevm.Model.Pavadinimas = Convert.ToString(item["pavadinimas"]);
                mevm.Model.Adresas = Convert.ToString(item["adresas"]);
				mevm.Model.FkMiestas = Convert.ToInt32(item["fk_MIESTASid_MIESTAS"]);
			}

			return mevm;
		}

		public static VeterinarijosKlinikaListVM FindForDeletion(int id)
		{
			var mlvm = new VeterinarijosKlinikaListVM();

			var query =
				$@"SELECT
					kl.id_VETERINARIJOS_KLINIKA,
					kl.pavadinimas,
                    kl.adresas,
					mies.pavadinimas AS miestas
				FROM
					veterinarijos_klinikos kl
					LEFT JOIN miestai mies ON mies.id_MIESTAS=fk_MIESTASid_MIESTAS
				WHERE
					kl.id_VETERINARIJOS_KLINIKA = ?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mlvm.Id = Convert.ToInt32(item["id_VETERINARIJOS_KLINIKA"]);
				mlvm.Pavadinimas = Convert.ToString(item["pavadinimas"]);
                mlvm.Adresas = Convert.ToString(item["adresas"]);
				mlvm.Miestas = Convert.ToString(item["miestas"]);
			}

			return mlvm;
		}

		public static void Update(VeterinarijosKlinikaEditVM klinikaEvm)
		{
			var query =
				$@"UPDATE veterinarijos_klinikos
				SET
					pavadinimas=?pavadinimas,
                    adresas=?adresas,
					fk_MIESTASid_MIESTAS=?miestas
				WHERE
					id_VETERINARIJOS_KLINIKA=?id";

			Sql.Update(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = klinikaEvm.Model.Pavadinimas;
                args.Add("?adresas", MySqlDbType.VarChar).Value = klinikaEvm.Model.Adresas;
				args.Add("?miestas", MySqlDbType.VarChar).Value = klinikaEvm.Model.FkMiestas;
				args.Add("?id", MySqlDbType.Int32).Value = klinikaEvm.Model.Id;
			});
		}

		public static void Insert(VeterinarijosKlinikaEditVM klinikaEvm)
		{
			var query =
				$@"INSERT INTO veterinarijos_klinikos
				(
					pavadinimas,
                    adresas,
					fk_MIESTASid_MIESTAS
				)
				VALUES(
					?pavadinimas,
                    ?adresas,
					?fk_MIESTASid_MIESTAS
				)";

			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = klinikaEvm.Model.Pavadinimas;
                args.Add("?adresas", MySqlDbType.VarChar).Value = klinikaEvm.Model.Adresas;
				args.Add("?fk_MIESTASid_MIESTAS", MySqlDbType.Int32).Value = klinikaEvm.Model.FkMiestas;
			});
		}

		public static void Delete(int id)
		{
			var query = $@"DELETE FROM veterinarijos_klinikos WHERE id_VETERINARIJOS_KLINIKA=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.Int32).Value = id;
			});
		}
	}
}