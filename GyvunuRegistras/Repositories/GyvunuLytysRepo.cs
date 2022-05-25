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
	/// Database operations related to 'GyvunoLytis' entity.
	/// </summary>
	public class GyvunuLytysRepo
	{
		public static List<GyvunoLytis> List()
		{
			var lytys = new List<GyvunoLytis>();
			
			var query = $@"SELECT * FROM lytys ORDER BY id_lytys ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				lytys.Add(new GyvunoLytis
				{
					Id = Convert.ToInt32(item["id_lytys"]),
					Pavadinimas = Convert.ToString(item["name"])
				});
			}
			return lytys;
		}

		public static GyvunoLytis Find(int id)
		{
			var lytis = new GyvunoLytis();

			var query = $@"SELECT * FROM lytys WHERE id_lytys=?id";
			var dt = 
				Sql.Query(query, args => {
				args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				lytis.Id = Convert.ToInt32(item["id_lytys"]);
				lytis.Pavadinimas = Convert.ToString(item["name"]);
			}

			return lytis;
		}
	}
}