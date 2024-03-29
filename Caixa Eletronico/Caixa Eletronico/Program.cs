// Aluno : Erick Martins
// Funções: Saque, Depósito, Transferência e Histórico.

using System.Runtime.Intrinsics.X86;

namespace Caixa_Eletronico
{
    class Program
    {
        private const string FormatoData = "dd/MM/yyyy HH:mm:ss";
        static float saldo;
        static readonly List<string> historico = [];
        static void Main(string[] args)
        {
            saldo = 1000;
            int opc;
            do
            {
                opc = Menu();
                switch (opc)
                {
                    case 1: //saque
                        Sacar();
                        break;
                    case 2: //deposito
                        Depositar();
                        break;
                    case 3: //transferência
                        Transferir();
                        break;
                    case 4: //histórico
                        ExibirHistorico();
                        break;
                }
            } while (opc != 0);
        }
        static int Menu()
        {
            Console.Clear();
            Console.WriteLine("Caixa Eletrônico");
            Console.WriteLine("\nSeu saldo é: " + saldo);
            Console.WriteLine("\n1 - Sacar");
            Console.WriteLine("2 - Depositar");
            Console.WriteLine("3 - Transferir");
            Console.WriteLine("4 - Histórico");
            Console.WriteLine("0 - Sair\n");
            Console.Write("Selecione uma opção: ");
            try
            {
                int r = int.Parse(Console.ReadLine());
                if (r < 0 || r > 4)
                {
                    Console.WriteLine("\nOpção inválida! Tente novamente.");
                    Console.ReadKey();
                    return -1;
                }
                return r;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("\nErro: Valor inválido. Digite um número inteiro.");
                Console.WriteLine(ex.Message);
                return -1; // Retorna -1 para operação inválida.
            }
        }
        static void Sacar()
        {
            float aux = saldo;
            Console.Clear();
            Console.WriteLine("Saldo atual: " + saldo);
            Console.Write("\nInforme o valor a ser sacado: ");
            int valor = int.Parse(Console.ReadLine());
            if (saldo >= valor)
            {
                saldo -= valor;
                string linha = "\nComprovante de saque\n";
                linha += "=============================\n";
                linha += "Data/Hora: " + DateTime.Now.ToString(FormatoData) + "\n";
                linha += "Saldo anterior: " + aux + "\n";
                linha += "Saque: " + valor + "\n";
                linha += "Saldo atual: " + saldo + "\n";
                linha += "=============================";
                Console.WriteLine(linha);
                historico.Add(linha); // Adiciona a operação na lista de histórico
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            }
            else
            {
                Console.WriteLine("Você não possui saldo suficiente!!!");
            }
            Console.ReadKey();
        }
        static void Depositar()
        {
            float aux = saldo;
            Console.Clear();
            Console.WriteLine("Saldo atual: " + saldo);
            Console.Write("\nInforme o valor a ser depositado: ");
            float valor = float.Parse(Console.ReadLine());
            saldo += valor;
            string linha = "\nComprovante de Depósito\n";
            linha += "=============================\n";
            linha += "Data/Hora: " + DateTime.Now.ToString(FormatoData) + "\n";
            linha += "Saldo anterior: " + aux + "\n";
            linha += "Saque: " + valor + "\n";
            linha += "Saldo atual: " + saldo + "\n";
            linha += "=============================";
            Console.WriteLine(linha);
            historico.Add(linha); // Adiciona a operação na lista de histórico
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
        }
        static void Transferir()
        {
            Console.Clear();
            Console.WriteLine("Saldo atual: " + saldo);
            float aux = saldo;
            Console.Write("\nBanco: ");
            string banco = Console.ReadLine();
            Console.Write("Agência: ");
            string agencia = Console.ReadLine();
            Console.Write("Conta: ");
            string conta = Console.ReadLine();
            Console.Write("Valor: ");
            float valor = float.Parse(Console.ReadLine());
            if (saldo >= valor)
            {
                saldo -= valor;
                string linha = "\nComprovante de transferência\n";
                linha += "=============================\n";
                linha += "Data/Hora: " + DateTime.Now.ToString(FormatoData) + "\n";
                linha += "Saldo anterior: " + aux + "\n";
                linha += "Transferência: " + valor + "\n";
                linha += "Saldo atual: " + saldo + "\n";
                linha += "=============================";
                Console.WriteLine(linha);
                historico.Add(linha); // Adiciona a operação na lista de histórico
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            }
            else
            {
                Console.WriteLine("Você não possui saldo suficiente!!!");
            }
        }
        static void ExibirHistorico()
        {
            Console.Clear();
            Console.WriteLine("Histórico de Operações");
            Console.WriteLine("=============================");
            foreach (string operacao in historico)
            {
                Console.WriteLine(operacao);
                Console.WriteLine();
            }
            Console.WriteLine("=============================");
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }
    }
}