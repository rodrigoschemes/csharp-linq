using AluraTunes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aula6
{
    /// <summary>
    /// Contando e Totalizando valores
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
                var query = from f in contexto.Faixas
                            where f.Album.Artista.Nome == "Led Zeppelin"
                            select f;

                var quantidade = contexto.Faixas
                                .Count(f => f.Album.Artista.Nome == "Led Zeppelin");

                Console.WriteLine("Led Zeppelin tem {0} faixas de música.", quantidade);

                Console.WriteLine();





                var query2 = from inf in contexto.ItensNotaFiscal
                            where inf.Faixa.Album.Artista.Nome == "Led Zeppelin"
                            select new { totalDoItem = inf.Quantidade * inf.PrecoUnitario };

                var totalDoArtista = query2.Sum(q => q.totalDoItem);

                Console.WriteLine("Total do artista: R$ {0}", totalDoArtista);

                Console.WriteLine();






                var query3 = from inf in contexto.ItensNotaFiscal
                             where inf.Faixa.Album.Artista.Nome == "Led Zeppelin"
                            group inf by inf.Faixa.Album into agrupado
                            let vendasPorAlbum = agrupado.Sum(a => a.Quantidade * a.PrecoUnitario)
                            orderby vendasPorAlbum
                                descending
                            select new
                            {
                                TituloDoAlbum = agrupado.Key.Titulo,
                                TotalPorAlbum = vendasPorAlbum
                            };

                foreach (var agrupado in query3)
                {
                    Console.WriteLine("{0}\t{1}",
                        agrupado.TituloDoAlbum.PadRight(40),
                        agrupado.TotalPorAlbum);
                }

                Console.WriteLine();







                contexto.Database.Log = Console.WriteLine;

                
            }
        }
    }
}
