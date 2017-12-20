using AluraTunes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aula7
{
    /// <summary>
    /// Max, Min, Média e Funções Customizadas
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new AluraTunesEntities())
            {
                var maiorVenda = contexto.NotasFiscais.Max(nf => nf.Total);
                var menorVenda = contexto.NotasFiscais.Min(nf => nf.Total);
                var vendaMedia = contexto.NotasFiscais.Average(nf => nf.Total);

                Console.WriteLine("A maior venda é de R$ {0}", maiorVenda);
                Console.WriteLine("A menor venda é de R$ {0}", menorVenda);
                Console.WriteLine("A venda média é de R$ {0}", vendaMedia);

                var vendas = (from nf in contexto.NotasFiscais
                              group nf by 1 into agrupado
                              select new
                              {
                                  maiorVenda = agrupado.Max(nf => nf.Total),
                                  menorVenda = agrupado.Min(nf => nf.Total),
                                  vendaMedia = agrupado.Average(nf => nf.Total)
                              }).Single();

                Console.WriteLine("A maior venda é de R$ {0}", vendas.maiorVenda);
                Console.WriteLine("A menor venda é de R$ {0}", vendas.menorVenda);
                Console.WriteLine("A venda média é de R$ {0}", vendas.vendaMedia);



                var vendaMediana = Mediana(contexto.NotasFiscais.Select(ag => ag.Total));
                Console.WriteLine("Venda Mediana: R$ {0}", vendaMediana);

                vendaMediana = contexto.NotasFiscais.Mediana(ag => ag.Total);
                Console.WriteLine("Venda Mediana: R$ {0}", vendaMediana);
            }

            Console.ReadKey();
        }

        public static decimal Mediana(IQueryable<decimal> origem)
        {
            int contagem = origem.Count();
            var ordenado = origem.OrderBy(p => p);

            var elementoCentral_1 = ordenado.Skip((contagem - 1) / 2).First();
            var elementoCentral_2 = ordenado.Skip(contagem / 2).First();

            decimal mediana = (elementoCentral_1 + elementoCentral_2) / 2;

            return mediana;
        }

    }

    public static class Extensions
    {
        public static decimal Mediana<TSource>(this IQueryable<TSource> origem, Expression<Func<TSource, decimal>> selector)
        {
            int contagem = origem.Count();

            var funcSeletor = selector.Compile();
            var ordenado = origem
                .Select(selector)
                .OrderBy(x => x);

            var elementoCentral_1 = ordenado.Skip((contagem - 1) / 2).First();
            var elementoCentral_2 = ordenado.Skip(contagem / 2).First();

            decimal mediana = (elementoCentral_1 + elementoCentral_2) / 2;

            return mediana;
        }
    }
}
