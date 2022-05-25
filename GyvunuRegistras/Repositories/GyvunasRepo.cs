using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

using GyvunuRegistras.Models;


namespace GyvunuRegistras.Repositories
{
	/// <summary>
	/// Database operations related to 'Gyvunas' entity.
	/// </summary>
	public class GyvunasRepo
	{
		public static List<Gyvunas> List()
		{
			var gyvunai = new List<Gyvunas>();
			
			string query = 
			$@"SELECT *
			 FROM gyvunai g
			 LEFT JOIN lytys l ON g.lytis=l.id_lytys
			 LEFT JOIN gyvunu_rusys r ON g.rusis=r.id_gyvunu_rusys
			 ORDER BY g.id_GYVUNAS ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				gyvunai.Add(new Gyvunas
				{
                    Id = Convert.ToString(item["id_GYVUNAS"]),
                    Vardas = Convert.ToString(item["vardas"]),
                    GimimoData = Convert.ToDateTime(item["gimimo_data"]),
                    Veisle = Convert.ToString(item["veisle"]),
                    Kailis = Convert.ToString(item["kailio_spalva"]),
                    Rusis = Convert.ToInt32(item["rusis"]),
                    Lytis = Convert.ToInt32(item["lytis"]),
                    FkSavininkas = Convert.ToString(item["fk_SAVININKASasmens_kodas"])
				});
			}

			return gyvunai;
		}

		public static Gyvunas Find(string id)
		{
			var query = $@"SELECT * FROM gyvunai WHERE id_GYVUNAS=?id";

			var dt = 
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.VarChar).Value = id;
				});

			if( dt.Count > 0 )
			{
				var Gyvunas = new Gyvunas();

				foreach( DataRow item in dt )
				{
					Gyvunas.Id = Convert.ToString(item["id_GYVUNAS"]);
                    Gyvunas.Vardas = Convert.ToString(item["vardas"]);
                    Gyvunas.GimimoData = Convert.ToDateTime(item["gimimo_data"]);
                    Gyvunas.Veisle = Convert.ToString(item["veisle"]);
                    Gyvunas.Kailis = Convert.ToString(item["kailio_spalva"]);
                    Gyvunas.Rusis = Convert.ToInt32(item["rusis"]);
                    Gyvunas.Lytis = Convert.ToInt32(item["lytis"]);
                    Gyvunas.FkSavininkas = Convert.ToString(item["fk_SAVININKASasmens_kodas"]);
				}

				return Gyvunas;
			}

			return null;
		}

		public static void Update(Gyvunas gyv)
		{						
			var query = 
				$@"UPDATE gyvunai
				SET 
					vardas=?vardas,
                    gimimo_data=?data,
                    veisle=?veisle,
                    kailio_spalva=?kailis,
                    rusis=?rusis,
                    lytis=?lytis,
                    fk_SAVININKASasmens_kodas=?fk
				WHERE 
					id_GYVUNAS=?id";

			Sql.Update(query, args => {
				args.Add("?vardas", MySqlDbType.VarChar).Value = gyv.Vardas;
                args.Add("?data", MySqlDbType.Date).Value = gyv.GimimoData;
                args.Add("?veisle", MySqlDbType.Date).Value = gyv.Veisle;
                args.Add("?kailis", MySqlDbType.Date).Value = gyv.Kailis;
                args.Add("?rusis", MySqlDbType.Date).Value = gyv.Rusis;
                args.Add("?lytis", MySqlDbType.Date).Value = gyv.Lytis;
                args.Add("?fk", MySqlDbType.Date).Value = gyv.FkSavininkas;
			});				
		}

		public static void Insert(Gyvunas gyv)
		{							
			var query = 
				$@"INSERT INTO gyvunai
				(
                    id_GYVUNAS,
					vardas,
                    gimimo_data,
                    veisle,
                    kailio_spalva,
                    rusis,
                    lytis,
                    fk_SAVININKASasmens_kodas
				)
				VALUES(
					?id,
                    ?vardas,
                    ?data,
                    ?veisle,
                    ?kailis,
                    ?rusis,
                    ?lytis,
                    ?fk
				)";

			Sql.Insert(query, args => {
                args.Add("?id", MySqlDbType.VarChar).Value = gyv.Id;
				args.Add("?vardas", MySqlDbType.VarChar).Value = gyv.Vardas;
                args.Add("?data", MySqlDbType.Date).Value = gyv.GimimoData;
                args.Add("?veisle", MySqlDbType.Date).Value = gyv.Veisle;
                args.Add("?kailis", MySqlDbType.Date).Value = gyv.Kailis;
                args.Add("?rusis", MySqlDbType.Date).Value = gyv.Rusis;
                args.Add("?lytis", MySqlDbType.Date).Value = gyv.Lytis;
                args.Add("?fk", MySqlDbType.Date).Value = gyv.FkSavininkas;
			});				
		}

		public static void Delete(string id)
		{			
			var query = $@"DELETE FROM gyvunai WHERE id_GYVUNAS=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.VarChar).Value = id;
			});			
		}
	}
}