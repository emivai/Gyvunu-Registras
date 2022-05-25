using System.Data;
using MySql.Data.MySqlClient;

using Newtonsoft.Json;

using GyvunuRegistras.Models;
using GyvunuRegistras.ViewModels;


namespace GyvunuRegistras.Repositories
{
	/// <summary>
	/// Database operations related to 'Mokejimai' entity.
	/// </summary>
	public class MokejimasRepo
	{
		public static List<SavininkasEditVM.MokejimasM> List(string SavininkasId)
		{
			var mokejimai = new List<SavininkasEditVM.MokejimasM>();

			var query =
				$@"SELECT *
				FROM mokejimai
				WHERE fk_SAVININKASasmens_kodas2 = ?SavininkasId
				ORDER BY fk_SASKAITAnumeris1 ASC, data ASC";

			var dt =
				Sql.Query(query, args => {
					args.Add("?SavininkasId", MySqlDbType.VarChar).Value = SavininkasId;
				});

			var inListId = 0;

			foreach( DataRow item in dt )
			{
				mokejimai.Add(new SavininkasEditVM.MokejimasM
				{
					InListId = inListId,
					Numeris = Convert.ToString(item["id_MOKEJIMAS"]),
                    Data = Convert.ToDateTime(item["data"]),
                    Suma = Convert.ToDecimal(item["suma"]),
                    FkSaskaita = Convert.ToString(item["fk_SASKAITAnumeris1"])
				});

				//advance in list ID for next entry
				inListId += 1;
			}

			return mokejimai;
		}

		public static SavininkasEditVM.MokejimasM Find(string id)
		{
			var mevm = new SavininkasEditVM.MokejimasM();

			var query = $@"SELECT * FROM mokejimai WHERE id_MOKEJIMAS=?id";

			var dt =
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.VarChar).Value = id;
				});

			foreach( DataRow item in dt )
			{
				mevm.Numeris = Convert.ToString(item["id_MOKEJIMAS"]);
                mevm.Data = Convert.ToDateTime(item["data"]);
                mevm.Suma = Convert.ToDecimal(item["suma"]);
                mevm.FkSaskaita = Convert.ToString(item["fk_SASKAITAnumeris1"]);
			}
			return mevm;
		}

		public static void Insert(string SavininkasId, SavininkasEditVM.MokejimasM up)
		{
			var query =
				$@"INSERT INTO mokejimai
					(
						fk_SAVININKASasmens_kodas2,
						fk_SASKAITAnumeris1,
						data,
                        suma, 
                        id_MOKEJIMAS
					)
					VALUES(
						?fk_Savininkas,
						?saskaita,
						?data,
						?suma,
						?id
					)";

			Sql.Insert(query, args => {
				args.Add("?fk_Savininkas", MySqlDbType.VarChar).Value = SavininkasId;
				args.Add("?saskaita", MySqlDbType.VarChar).Value = up.FkSaskaita;
				args.Add("?data", MySqlDbType.Date).Value = up.Data;
				args.Add("?suma", MySqlDbType.Decimal).Value = up.Suma;
				args.Add("?id", MySqlDbType.VarChar).Value = up.Numeris;
			});
		}

		public static void DeleteForSavininkas(string Savininkas)
		{
			var query =
				$@"DELETE FROM s
				USING mokejimai as s
				WHERE fk_SAVININKASasmens_kodas2=?fkid";

			Sql.Delete(query, args => {
				args.Add("?fkid", MySqlDbType.VarChar).Value = Savininkas;
			});
		}
	}
}