// Erick Martins
// @erickapenas
// Fila de atendimento de banco utilizando PILHA

using System;
using System.Collections.Generic;

class FilaBanco
{
    private static List<int> clientes = new List<int>();
    private static Stack<int> filaAtendimento = new Stack<int>();

    static void Main(string[] args)
    {
        int opcao;

        do
        {
            ExibirMenu();
            opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    AdicionarCliente();
                    break;
                case 2:
                    RemoverCliente();
                    break;
                case 3:
                    SimularAtendimento();
                    break;
                case 4:
                    ListarClientes();
                    break;
                case 5:
                    Console.WriteLine("Obrigado por usar o sistema de fila!");
                    break;
                default:
                    Console.WriteLine("Opção inválida. Digite um número entre 1 e 5.");
                    break;
            }

        } while (opcao != 5);
    }

    static void ExibirMenu()
    {
        Console.WriteLine("---- Menu Fila Banco ----");
        Console.WriteLine("1 - Adicionar pessoa à fila");
        Console.WriteLine("2 - Remover pessoa da fila");
        Console.WriteLine("3 - Simular atendimento");
        Console.WriteLine("4 - Listar todas as pessoas na fila");
        Console.WriteLine("5 - Sair");
        Console.WriteLine("-------------------------");
        Console.Write("Digite a sua opção: ");
    }

    static void AdicionarCliente()
    {
        Console.Write("Digite o número da pessoa: ");
        int numeroCliente = int.Parse(Console.ReadLine());

        clientes.Add(numeroCliente);
        filaAtendimento.Push(numeroCliente);

        Console.WriteLine("Pessoa adicionada à fila!\n");
    }

    static void RemoverCliente()
    {
        if (filaAtendimento.Count == 0)
        {
            Console.WriteLine("Fila vazia. Não há pessoas para remover.\n");
            return;
        }

        int clienteRemovido = filaAtendimento.Pop();
        clientes.Remove(clienteRemovido);

        Console.WriteLine($"Pessoa {clienteRemovido} removida da fila!\n");
    }

    static void SimularAtendimento()
    {
        if (filaAtendimento.Count == 0)
        {
            Console.WriteLine("Fila vazia. Não há clientes para atender.\n");
            return;
        }

        int clienteAtual = filaAtendimento.Pop();

        Console.WriteLine($"Atendendo cliente {clienteAtual}.\n");

        clientes.Remove(clienteAtual);
    }

    static void ListarClientes()
    {
        if (clientes.Count == 0)
        {
            Console.WriteLine("Fila vazia. Não há pessoas na fila.\n");
            return;
        }

        Console.WriteLine("---- Lista de Clientes na Fila ----");
        foreach (int cliente in clientes)
        {
            Console.WriteLine(cliente);
        }
        Console.WriteLine("-----------------------------------");
    }
}