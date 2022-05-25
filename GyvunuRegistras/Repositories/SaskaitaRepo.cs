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
	/// Database operations related to 'Saskaita' entity.
	/// </summary>
	public class SaskaitaRepo
	{
		public static List<Saskaita> List()
		{
			var saskaitos = new List<Saskaita>();
			
			string query = $@"SELECT * FROM saskaitos ORDER BY numeris ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				saskaitos.Add(new Saskaita
				{
                    Numeris = Convert.ToString(item["numeris"]),
                    Data = Convert.ToDateTime(item["data"]),
                    Suma = Convert.ToDecimal(item["suma"])

				});
			}

			return saskaitos;
		}

		public static Saskaita Find(string numr)
		{
			var query = $@"SELECT * FROM saskaitos WHERE numeris=?num";

			var dt = 
				Sql.Query(query, args => {
					args.Add("?num", MySqlDbType.VarChar).Value = numr;
				});

			if( dt.Count > 0 )
			{
				var Saskaita = new Saskaita();

				foreach( DataRow item in dt )
				{
					Saskaita.Numeris = Convert.ToString(item["numeris"]);
                    Saskaita.Data = Convert.ToDateTime(item["data"]);
                    Saskaita.Suma = Convert.ToDecimal(item["suma"]);
				}

				return Saskaita;
			}

			return null;
		}

		public static void Update(Saskaita sask)
		{						
			var query = 
				$@"UPDATE saskaitos
				SET 
					data=?data, 
					suma=?suma 
				WHERE 
					numeris=?num";

			Sql.Update(query, args => {
				args.Add("?data", MySqlDbType.Date).Value = sask.Data;
				args.Add("?suma", MySqlDbType.Decimal).Value = sask.Suma;
				args.Add("?num", MySqlDbType.VarChar).Value = sask.Numeris;
			});				
		}

		public static void Insert(Saskaita sask)
		{							
			var query = 
				$@"INSERT INTO saskaitos
				(
					numeris,
					data,
					suma
				)
				VALUES(
					?numeris,
					?data,
					?suma
				)";

			Sql.Insert(query, args => {
				args.Add("?data", MySqlDbType.Date).Value = sask.Data;
				args.Add("?suma", MySqlDbType.Decimal).Value = sask.Suma;
				args.Add("?numeris", MySqlDbType.VarChar).Value = sask.Numeris;
			});				
		}

		public static void Delete(string id)
		{			
			var query = $@"DELETE FROM saskaitos WHERE numeris=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.VarChar).Value = id;
			});			
		}
	}
}