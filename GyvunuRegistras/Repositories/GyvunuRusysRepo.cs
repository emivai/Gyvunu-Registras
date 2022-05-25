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
	/// Database operations related to 'GyvunoRusis' entity.
	/// </summary>
	public class GyvunuRusysRepo
	{
		public static List<GyvunoRusis> List()
		{
			var rusys = new List<GyvunoRusis>();
			
			var query = $@"SELECT * FROM gyvunu_rusys ORDER BY id_gyvunu_rusys ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				rusys.Add(new GyvunoRusis
				{
					Id = Convert.ToInt32(item["id_gyvunu_rusys"]),
					Pavadinimas = Convert.ToString(item["name"])
				});
			}
			return rusys;
		}

		public static GyvunoRusis Find(int id)
		{
			var rusis = new GyvunoRusis();

			var query = $@"SELECT * FROM gyvunu_rusys WHERE id_gyvunu_rusys=?id";
			var dt = 
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.Int32).Value = id;
				});

			foreach( DataRow item in dt )
			{
				rusis.Id = Convert.ToInt32(item["id_gyvunu_rusys"]);
				rusis.Pavadinimas = Convert.ToString(item["name"]);
			}

			return rusis;
		}


	}

	
}