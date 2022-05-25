using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.Models;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories
{
	public class MiestasRepo
	{
		public static List<Miestas> List()
		{
			var miestai = new List<Miestas>();

			string query = $@"SELECT * FROM miestai ORDER BY id_MIESTAS ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				miestai.Add(new Miestas
				{
					id_MIESTAS = Convert.ToInt32(item["id_MIESTAS"]),
					Pavadinimas = Convert.ToString(item["pavadinimas"]),
				});
			}

			return miestai;
		}

		public static Miestas Find(int id_MIESTAS)
		{
			var miestas = new Miestas();

			var query = $@"SELECT * FROM miestai WHERE id_MIESTAS=?id_MIESTAS";
			var dt = 
				Sql.Query(query, args => {
					args.Add("?id_MIESTAS", MySqlDbType.Int32).Value = id_MIESTAS;
				});

			foreach( DataRow item in dt )
			{
				miestas.id_MIESTAS = Convert.ToInt32(item["id_MIESTAS"]);
				miestas.Pavadinimas = Convert.ToString(item["pavadinimas"]);
			}

			return miestas;
		}

		public static void Update(Miestas miestas)
		{			
			var query = 
				$@"UPDATE miestai 
				SET 
					pavadinimas=?pavadinimas 
				WHERE 
					id_MIESTAS=?id_MIESTAS";

			Sql.Update(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = miestas.Pavadinimas;
				args.Add("?id_MIESTAS", MySqlDbType.VarChar).Value = miestas.id_MIESTAS;
			});							
		}

		public static void Insert(Miestas miestas)
		{			
			var query = $@"INSERT INTO miestai ( pavadinimas ) VALUES ( ?pavadinimas )";
			Sql.Insert(query, args => {
				args.Add("?pavadinimas", MySqlDbType.VarChar).Value = miestas.Pavadinimas;
			});
		}

		public static void Delete(int id_MIESTAS)
		{			
			var query = $@"DELETE FROM miestai where id_MIESTAS=?id_MIESTAS";
			Sql.Delete(query, args => {
				args.Add("?id_MIESTAS", MySqlDbType.Int32).Value = id_MIESTAS;
			});			
		}
	}
}