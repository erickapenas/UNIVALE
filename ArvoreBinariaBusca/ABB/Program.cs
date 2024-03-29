// Erick Martins
// @erickapenas

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Tarvore dados = new Tarvore();
            tdados aux;
            string cont;
            do
            {
                aux = new tdados();
                Console.Write("Codigo: ");
                aux.codigo = int.Parse(Console.ReadLine());
                Console.Write("Nome: ");
                aux.nome = Console.ReadLine();
                dados.insere(aux);
                Console.Write("Continuar? (S/N) \n");
                cont = Console.ReadLine();
            } while (cont == "s");
            Console.WriteLine(dados.listar());
            Console.ReadKey();
        }
    }
}
