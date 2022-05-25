using MySql.Data.MySqlClient;
using System.Data;

using GyvunuRegistras.ViewModels;


namespace GyvunuRegistras.Repositories
{
	/// <summary>
    /// Database operations related to 'Gydytojas' entity.
    /// </summary>
	public class GydytojasRepo
	{
		public static List<GydytojasListVM> List()
		{
			var result = new List<GydytojasListVM>();
			
			var query = 
				$@"SELECT 
					g.asmens_kodas, 
					g.vardas,
                    g.pavarde,
                    g.telefono_numeris,
                    g.elektroninio_pasto_adresas,
					k.pavadinimas as klinika
				FROM 
					veterinarijos_gydytojai g
					LEFT JOIN veterinarijos_klinikos k ON g.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA=k.id_VETERINARIJOS_KLINIKA			
				ORDER BY g.asmens_kodas ASC";
			var dt = Sql.Query(query);

			foreach( DataRow item in dt )
			{
				result.Add(new GydytojasListVM {
                    AsmensKodas = Convert.ToString(item["asmens_kodas"]),
                    Vardas = Convert.ToString(item["vardas"]),
                    Pavarde = Convert.ToString(item["pavarde"]),
                    Numeris = Convert.ToString(item["telefono_numeris"]),
                    ElPastas = Convert.ToString(item["elektroninio_pasto_adresas"]),
                    Klinika = Convert.ToString(item["klinika"])
				});
			}

			return result;
		}

		public static GydytojasEditVM Find(string nr)
		{
			var result = new GydytojasEditVM();
			
			string qlquery = $@"SELECT * FROM veterinarijos_gydytojai WHERE asmens_kodas=?nr";
			var dt = 
				Sql.Query(qlquery, args => {
                    args.Add("?nr", MySqlDbType.VarChar).Value = nr;
                });

			var gyd = result.Gydytojas;

			foreach( DataRow item in dt )
			{
                gyd.AsmensKodas = Convert.ToString(item["asmens_kodas"]);
                gyd.Vardas = Convert.ToString(item["vardas"]);
                gyd.Pavarde = Convert.ToString(item["pavarde"]);
                gyd.Numeris = Convert.ToString(item["telefono_numeris"]);
                gyd.ElPastas = Convert.ToString(item["elektroninio_pasto_adresas"]);
                gyd.Klinika = Convert.ToInt32(item["fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA"]);
			}

			return result;
		}

		public static void Update(GydytojasEditVM evm)
		{
            var query = 
				$@"UPDATE veterinarijos_gydytojai
				SET
					vardas=?vardas,
                    pavarde=?pavarde,
                    telefono_numeris=?numeris,
                    elektroninio_pasto_adresas=?pastas,
					fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA=?klinika
				WHERE asmens_kodas=?kodas";

            Sql.Update(query, args => {
                args.Add("?kodas", MySqlDbType.VarChar).Value = evm.Gydytojas.AsmensKodas;
                args.Add("?vardas", MySqlDbType.VarChar).Value = evm.Gydytojas.Vardas;
                args.Add("?pavarde", MySqlDbType.VarChar).Value = evm.Gydytojas.Pavarde;
                args.Add("?numeris", MySqlDbType.VarChar).Value = evm.Gydytojas.Numeris;
                args.Add("?pastas", MySqlDbType.VarChar).Value = evm.Gydytojas.ElPastas;
                args.Add("?klinika", MySqlDbType.Int32).Value = evm.Gydytojas.Klinika;
            });
		}

		public static void Insert(GydytojasEditVM evm)
		{			
			var query = 
				$@"INSERT INTO veterinarijos_gydytojai
				(
                    asmens_kodas,
					vardas,
                    pavarde,
                    telefono_numeris,
                    elektroninio_pasto_adresas,
					fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
				)
				VALUES(
                    ?kodas,
					?vardas,
                    ?pavarde,
                    ?numeris,
                    ?pastas,
					?klinika
				)";

				Sql.Insert(query, args => {
                args.Add("?kodas", MySqlDbType.VarChar).Value = evm.Gydytojas.AsmensKodas;
                args.Add("?vardas", MySqlDbType.VarChar).Value = evm.Gydytojas.Vardas;
                args.Add("?pavarde", MySqlDbType.VarChar).Value = evm.Gydytojas.Pavarde;
                args.Add("?numeris", MySqlDbType.VarChar).Value = evm.Gydytojas.Numeris;
                args.Add("?pastas", MySqlDbType.VarChar).Value = evm.Gydytojas.ElPastas;
                args.Add("?klinika", MySqlDbType.Int32).Value = evm.Gydytojas.Klinika;
				});
		}

		public static void Delete(string nr)
		{			
			var query = $@"DELETE FROM veterinarijos_gydytojai where asmens_kodas=?nr";
			Sql.Delete(query, args => {
				args.Add("?nr", MySqlDbType.VarChar).Value = nr;
			});
		}
	}
}