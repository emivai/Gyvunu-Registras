using System.Data;
using MySql.Data.MySqlClient;

using Ataskaita = GyvunuRegistras.ViewModels.Ataskaita;


namespace GyvunuRegistras.Repositories
{
	/// <summary>
	/// Database operations related to reports.
	/// </summary>
	public class AtaskaitaRepo
	{		
		public static List<Ataskaita.Gydytojas> GetDoctors(DateTime? dateFrom, DateTime? dateTo, string city)
		{
			var result = new List<Ataskaita.Gydytojas>();	

            var query =
				$@"SELECT
					UCASE(km.miestas) as miestas,
					km.klinika,
					gyd.vardas,
					gyd.pavarde,
					IFNULL(jn1.cnt,0) as mikro_suma,
					IFNULL(jn2.cnt,0) as pras_suma,
					IFNULL(jn3.suma1,0) as mikroschemos,
					CASE WHEN jn4.suma2 IS NOT NULL THEN jn4.suma2 ELSE 0 END as prasymai
				FROM veterinarijos_gydytojai gyd
				INNER JOIN
					(SELECT
					ms.pavadinimas as miestas,
					kl.pavadinimas as klinika,
					kl.id_VETERINARIJOS_KLINIKA as id
					FROM veterinarijos_klinikos kl
					INNER JOIN miestai ms ON kl.fk_MIESTASid_MIESTAS = ms.id_MIESTAS)AS km 
				ON km.id = gyd.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
				LEFT JOIN 
					(SELECT
					mk.numeris,
 					COUNT(mk.numeris) as cnt,
 					mk.fk_VETERINARIJOS_GYDYTOJASasmens_kodas as id
					FROM mikroschemos mk
					WHERE mk.iterpimo_data >= ?nuo AND mk.iterpimo_data <= ?iki
 					GROUP BY mk.fk_VETERINARIJOS_GYDYTOJASasmens_kodas) AS jn1 
				ON jn1.id = gyd.asmens_kodas
				LEFT JOIN 
					(SELECT
					pr.numeris,
 					COUNT(pr.numeris) as cnt,
    				pr.pateikimo_data as data2,
 					pr.fk_VETERINARIJOS_GYDYTOJASasmens_kodas1 as id
 					FROM registravimo_prasymai pr
					WHERE pr.pateikimo_data >= ?nuo AND pr.pateikimo_data <= ?iki
 					GROUP BY pr.fk_VETERINARIJOS_GYDYTOJASasmens_kodas1) AS jn2 
				ON jn2.id = gyd.asmens_kodas
				LEFT JOIN
					(SELECT 
					COUNT(m.numeris) as suma1,
 					kli.id_VETERINARIJOS_KLINIKA as id
 					FROM mikroschemos m
 					INNER JOIN veterinarijos_gydytojai gyd1 ON gyd1.asmens_kodas = m.fk_VETERINARIJOS_GYDYTOJASasmens_kodas
 					INNER JOIN veterinarijos_klinikos kli ON kli.id_VETERINARIJOS_KLINIKA = gyd1.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
 					WHERE m.iterpimo_data >= ?nuo AND m.iterpimo_data <= ?iki
					GROUP BY kli.id_VETERINARIJOS_KLINIKA) AS jn3 
				ON jn3.id = gyd.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
				LEFT JOIN
					(SELECT 
					COUNT(r.numeris) as suma2,
 					kli.id_VETERINARIJOS_KLINIKA as id
 					FROM registravimo_prasymai r
 					INNER JOIN veterinarijos_gydytojai gyd1 ON gyd1.asmens_kodas = r.fk_VETERINARIJOS_GYDYTOJASasmens_kodas1
 					INNER JOIN veterinarijos_klinikos kli ON kli.id_VETERINARIJOS_KLINIKA = gyd1.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
 					WHERE r.pateikimo_data >= ?nuo AND r.pateikimo_data <= ?iki
					GROUP BY kli.id_VETERINARIJOS_KLINIKA) AS jn4 
				ON jn4.id = gyd.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
				WHERE km.miestas = ?city
				ORDER BY km.klinika, gyd.vardas, gyd.pavarde DESC";			

			var dt =
				Sql.Query(query, args => {
					args.Add("?nuo", MySqlDbType.DateTime).Value = dateFrom;
					args.Add("?iki", MySqlDbType.DateTime).Value = dateTo;
                    args.Add("?city", MySqlDbType.VarChar).Value = city;
				});

			foreach( DataRow item in dt )
			{
				result.Add(new Ataskaita.Gydytojas
				{
					Klinika = Convert.ToString(item["klinika"]),
					Miestas = Convert.ToString(item["miestas"]),
					Vardas = Convert.ToString(item["vardas"]),
					Pavarde = Convert.ToString(item["pavarde"]),
					Mikroschemos = Convert.ToInt32(item["mikro_suma"]),
					Prasymai = Convert.ToInt32(item["pras_suma"]),
					MikroschemuSuma = Convert.ToInt32(item["mikroschemos"]),
					PrasymuSuma = Convert.ToInt32(item["prasymai"])
				});
			}

			return result;
		}

		public static void GetTotal(Ataskaita.Report result, DateTime? dateFrom, DateTime? dateTo, string city)
		{
			var query =
				$@"SELECT
				IFNULL(a1.suma, 0) as mik_suma,
				IFNULL(a2.suma, 0) as pras_suma
				FROM miestai m
				LEFT JOIN
					(SELECT 
 					ms.id_MIESTAS as id,
					COUNT(m.numeris) as suma
					FROM mikroschemos m
					INNER JOIN veterinarijos_gydytojai gyd1 ON gyd1.asmens_kodas = m.fk_VETERINARIJOS_GYDYTOJASasmens_kodas
					INNER JOIN veterinarijos_klinikos kli ON kli.id_VETERINARIJOS_KLINIKA = gyd1.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
					INNER JOIN miestai ms ON ms.id_MIESTAS = kli.fk_MIESTASid_MIESTAS
					WHERE m.iterpimo_data >= ?nuo AND m.iterpimo_data <= ?iki
					GROUP BY ms.id_MIESTAS) AS a1 
				ON a1.id = m.id_MIESTAS
				LEFT JOIN
					(SELECT 
  					ms.id_MIESTAS as id,
					COUNT(r.numeris) as suma
					FROM registravimo_prasymai r
					INNER JOIN veterinarijos_gydytojai gyd1 ON gyd1.asmens_kodas = r.fk_VETERINARIJOS_GYDYTOJASasmens_kodas1
					INNER JOIN veterinarijos_klinikos kli ON kli.id_VETERINARIJOS_KLINIKA = gyd1.fk_VETERINARIJOS_KLINIKAid_VETERINARIJOS_KLINIKA
					INNER JOIN miestai ms ON ms.id_MIESTAS = kli.fk_MIESTASid_MIESTAS
					WHERE r.pateikimo_data >= ?nuo AND r.pateikimo_data <= ?iki
					GROUP BY ms.id_MIESTAS) AS a2 
				ON a2.id = m.id_MIESTAS
				WHERE m.pavadinimas = ?city";

			var dt =
				Sql.Query(query, args => {
					args.Add("?nuo", MySqlDbType.DateTime).Value = dateFrom;
					args.Add("?iki", MySqlDbType.DateTime).Value = dateTo;
					args.Add("?city", MySqlDbType.VarChar).Value = city;
				});

			foreach( DataRow item in dt )
			{
				result.VisoSumaMikroschemu = Convert.ToInt32(item["mik_suma"]);
				result.VisoSumaPrasymu = Convert.ToInt32(item["pras_suma"]);
			}

		}
	}


}