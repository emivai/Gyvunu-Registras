using System.Data;
using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.Models;
using Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories
{
	/// <summary>
	/// Database operations related to 'mikroschemos' entity.
	/// </summary>
	public class MikroschemaRepo
	{
		public static List<GydytojasEditVM.MikroschemaM> List(string GydytojasId)
		{
			var mikroschemos = new List<GydytojasEditVM.MikroschemaM>();

			var query =
				$@"SELECT *
				FROM mikroschemos 
				WHERE fk_VETERINARIJOS_GYDYTOJASasmens_kodas = ?GydytojasId
				ORDER BY numeris ASC";

			var dt =
				Sql.Query(query, args => {
					args.Add("?GydytojasId", MySqlDbType.VarChar).Value = GydytojasId;
				});

			var inListId = 0;

			foreach( DataRow item in dt )
			{
				mikroschemos.Add(new GydytojasEditVM.MikroschemaM
				{
					InListId = inListId,
					Numeris = Convert.ToString(item["numeris"]),
                    Data = Convert.ToDateTime(item["iterpimo_data"]),
                    Saskaita = Convert.ToString(item["fk_SASKAITAnumeris"]),
                    Gyvunas = Convert.ToString(item["fk_GYVUNASid_GYVUNAS"])
				});

				//advance in list ID for next entry
				inListId += 1;
			}

			return mikroschemos;
		}

		public static GydytojasEditVM.MikroschemaM Find(string id)
		{
			var mevm = new GydytojasEditVM.MikroschemaM();

			var query = $@"SELECT * FROM mikroschemos WHERE numeris=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.VarChar).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mevm.Numeris = Convert.ToString(item["numeris"]);
                mevm.Data = Convert.ToDateTime(item["iterpimo_data"]);
                mevm.Saskaita = Convert.ToString(item["fk_SASKAITAnumeris"]);
                mevm.Gyvunas = Convert.ToString(item["fk_GYVUNASid_GYVUNAS"]);
			}
			return mevm;
		}

		public static GydytojasEditVM.MikroschemaM FindAnimal(string id)
		{
			var mevm = new GydytojasEditVM.MikroschemaM();

			var query = $@"SELECT * FROM mikroschemos WHERE fk_GYVUNASid_GYVUNAS=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.VarChar).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mevm.Numeris = Convert.ToString(item["numeris"]);
                mevm.Data = Convert.ToDateTime(item["iterpimo_data"]);
                mevm.Saskaita = Convert.ToString(item["fk_SASKAITAnumeris"]);
                mevm.Gyvunas = Convert.ToString(item["fk_GYVUNASid_GYVUNAS"]);
			}
			return mevm;
		}

		public static void Insert(string GydytojasId, GydytojasEditVM.MikroschemaM up)
		{
			var query =
				$@"INSERT INTO mikroschemos
					(
						fk_VETERINARIJOS_GYDYTOJASasmens_kodas,
                        fk_GYVUNASid_GYVUNAS,
						fk_SASKAITAnumeris,
						iterpimo_data,
                        numeris
					)
					VALUES(
						?fk_Gydytojas,
                        ?fk_Gyvunas,
						?saskaita,
						?data,
						?id
					)";

			Sql.Insert(query, args => {
				args.Add("?fk_Gydytojas", MySqlDbType.VarChar).Value = GydytojasId;
                args.Add("?fk_Gyvunas", MySqlDbType.VarChar).Value = up.Gyvunas;
				args.Add("?saskaita", MySqlDbType.VarChar).Value = up.Saskaita;
				args.Add("?data", MySqlDbType.Date).Value = up.Data;
				args.Add("?id", MySqlDbType.VarChar).Value = up.Numeris;
			});
		}

		public static void DeleteForGydytojas(string Gydytojas)
		{
			var query =
				$@"DELETE FROM s
				USING mikroschemos as s
				WHERE s.fk_VETERINARIJOS_GYDYTOJASasmens_kodas=?fkid";

			Sql.Delete(query, args => {
				args.Add("?fkid", MySqlDbType.VarChar).Value = Gydytojas;
			});
		}
	}
}