using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

using Org.Ktu.Isk.P175B602.GyvunuRegistras.ViewModels;


namespace Org.Ktu.Isk.P175B602.GyvunuRegistras.Repositories
{
	/// <summary>
	/// Database operations related to 'Savininkas' entity.
	/// </summary>
	public class SavininkasRepo
	{
		public static List<SavininkasListVM> List()
		{
			var savininkai = new List<SavininkasListVM>();

			string query = $@"SELECT * FROM savininkai ORDER BY pavarde ASC, vardas ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				savininkai.Add(new SavininkasListVM
				{
                    AsmensKodas = Convert.ToString(item["asmens_kodas"]),
                    Vardas = Convert.ToString(item["vardas"]),
                    Pavarde = Convert.ToString(item["pavarde"]),
                    Adresas = Convert.ToString(item["adresas"]),
                    Numeris = Convert.ToString(item["telefono_numeris"]),
                    ElPastas = Convert.ToString(item["elektroninio_pasto_adresas"]) 
				});
			}

			return savininkai;
		}

		public static SavininkasEditVM Find(string id)
		{
			var result = new SavininkasEditVM();

			string query = 
				$@"SELECT 
					asmens_kodas,
                    vardas,
                    pavarde,
                    adresas,
                    telefono_numeris,
                    elektroninio_pasto_adresas
				FROM savininkai 
				WHERE asmens_kodas=?id";

			var dt = 
				Sql.Query(query, args => {
					args.Add("?id", MySqlDbType.VarChar).Value = id;
				});

			var sav = result.Savininkas;

			foreach( DataRow item in dt )
			{
				sav.AsmensKodas = Convert.ToString(item["asmens_kodas"]);
                sav.Vardas = Convert.ToString(item["vardas"]);
                sav.Pavarde = Convert.ToString(item["pavarde"]);
                sav.Adresas = Convert.ToString(item["adresas"]);
                sav.Numeris = Convert.ToString(item["telefono_numeris"]);
                sav.ElPastas = Convert.ToString(item["elektroninio_pasto_adresas"]);
			}

			return result;
		}

		public static void Update(SavininkasEditVM evm)
		{
			var query = 
				$@"UPDATE savininkai s 
				SET s.vardas=?vardas, s.pavarde=?pavarde, s.adresas=?adresas, 
                s.telefono_numeris=?telefono_numeris, s.elektroninio_pasto_adresas=?elektroninio_pasto_adresas
				WHERE s.asmens_kodas=?asmens_kodas";

			Sql.Update(query, args => {
				args.Add("?asmens_kodas", MySqlDbType.VarChar).Value = evm.Savininkas.AsmensKodas;
                args.Add("?vardas", MySqlDbType.VarChar).Value = evm.Savininkas.Vardas;
                args.Add("?pavarde", MySqlDbType.VarChar).Value = evm.Savininkas.Pavarde;
                args.Add("?adresas", MySqlDbType.VarChar).Value = evm.Savininkas.Adresas;
                args.Add("?telefono_numeris", MySqlDbType.VarChar).Value = evm.Savininkas.Numeris;
                args.Add("?elektroninio_pasto_adresas", MySqlDbType.VarChar).Value = evm.Savininkas.ElPastas;
			});
		}

		public static void Insert(SavininkasEditVM evm)
		{							
			var query = 
				$@"INSERT INTO savininkai
				(
					asmens_kodas,
                    vardas,
                    pavarde,
                    adresas,
                    telefono_numeris,
                    elektroninio_pasto_adresas
				)
				VALUES(
					?asmens_kodas,
                    ?vardas,
                    ?pavarde,
                    ?adresas,
                    ?telefono_numeris,
                    ?elektroninio_pasto_adresas
				)";

			Sql.Insert(query, args => {
				args.Add("?asmens_kodas", MySqlDbType.VarChar).Value = evm.Savininkas.AsmensKodas;
                args.Add("?vardas", MySqlDbType.VarChar).Value = evm.Savininkas.Vardas;
                args.Add("?pavarde", MySqlDbType.VarChar).Value = evm.Savininkas.Pavarde;
                args.Add("?adresas", MySqlDbType.VarChar).Value = evm.Savininkas.Adresas;
                args.Add("?telefono_numeris", MySqlDbType.VarChar).Value = evm.Savininkas.Numeris;
                args.Add("?elektroninio_pasto_adresas", MySqlDbType.VarChar).Value = evm.Savininkas.ElPastas;
			});				
		}

		public static void Delete(string id)
		{
			var query = $@"DELETE FROM savininkai WHERE asmens_kodas=?id";
			Sql.Delete(query, args => {
				args.Add("?id", MySqlDbType.VarChar).Value = id;
			});
		}
	}
}