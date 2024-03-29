// Erick Martins
// @erickapenas

using System;
using System.Collections.Generic;

public class Restaurante
{
    private Queue<Pedido> filaPedidos = new Queue<Pedido>();
    private Stack<Prato> pilhaPratos = new Stack<Prato>();
    private List<Ingrediente> estoqueIngredientes = new List<Ingrediente>();

    public void AdicionarPedido(Pedido pedido)
    {
        filaPedidos.Enqueue(pedido);
        Console.WriteLine("Pedido adicionado com sucesso!");
    }

    public void PrepararProximoPedido()
    {
        if (filaPedidos.Count > 0)
        {
            Pedido pedido = filaPedidos.Dequeue();
            foreach (Prato prato in pedido.Pratos)
            {
                pilhaPratos.Push(prato);
            }
            Console.WriteLine("Pedido preparado com sucesso!");
        }
        else
        {
            Console.WriteLine("Não há pedidos na fila.");
        }
    }

    public void FinalizarProximoPrato()
    {
        if (pilhaPratos.Count > 0)
        {
            Prato prato = pilhaPratos.Pop();
            foreach (Ingrediente ingrediente in prato.Ingredientes)
            {
                AtualizarEstoque(ingrediente);
            }
            Console.WriteLine($"Prato {prato.Nome} finalizado!");
        }
        else
        {
            Console.WriteLine("Não há pratos para finalizar.");
        }
    }

    private void AtualizarEstoque(Ingrediente ingrediente)
    {
        Ingrediente ingredienteEstoque = estoqueIngredientes.Find(i => i.Nome == ingrediente.Nome);

        if (ingredienteEstoque == null)
        {
            estoqueIngredientes.Add(new Ingrediente
            {
                Nome = ingrediente.Nome,
                Quantidade = -ingrediente.Quantidade,
                DataValidade = ingrediente.DataValidade
            });
        }
        else
        {
            ingredienteEstoque.Quantidade -= ingrediente.Quantidade;

            if (ingredienteEstoque.Quantidade < 0)
            {
                ReordenarIngrediente(ingredienteEstoque);
            }
        }
    }

    private void ReordenarIngrediente(Ingrediente ingrediente)
    {
        Console.WriteLine($"Ingrediente {ingrediente.Nome} precisa ser reordenado!");
    }

    public void OrganizarEstoque()
    {
        DateTime dataAtual = DateTime.Now;
        estoqueIngredientes.RemoveAll(i => i.DataValidade < dataAtual);

        foreach (Ingrediente ingrediente in estoqueIngredientes)
        {
            if (ingrediente.Quantidade < 10)
            {
                ReordenarIngrediente(ingrediente);
            }
        }
        Console.WriteLine("Estoque organizado com sucesso!");
    }

    public void AdicionarAoEstoque(Ingrediente novoIngrediente)
    {
        Ingrediente ingredienteEstoque = estoqueIngredientes.Find(i => i.Nome == novoIngrediente.Nome);

        if (ingredienteEstoque == null)
        {
            estoqueIngredientes.Add(new Ingrediente
            {
                Nome = novoIngrediente.Nome,
                Quantidade = novoIngrediente.Quantidade,
                DataValidade = novoIngrediente.DataValidade
            });
        }
        else
        {
            ingredienteEstoque.Quantidade += novoIngrediente.Quantidade;
        }
        Console.WriteLine($"Ingrediente {novoIngrediente.Nome} adicionado ao estoque com sucesso!");
    }

    public void ExibirEstoque()
    {
        Console.WriteLine("Estoque de ingredientes:");
        foreach (Ingrediente ingrediente in estoqueIngredientes)
        {
            Console.WriteLine($"{ingrediente.Nome}: {ingrediente.Quantidade} unidades, Validade: {ingrediente.DataValidade.ToShortDateString()}");
        }
    }
}

public class Pedido
{
    public List<Prato> Pratos { get; set; }
}

public class Prato
{
    public List<Ingrediente> Ingredientes { get; set; }
    public string Nome { get; set; }
}

public class Ingrediente
{
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataValidade { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        Restaurante restaurante = new Restaurante();

        do
        {
            Console.WriteLine("\n=== Menu ===");
            Console.WriteLine("1. Adicionar Pedido");
            Console.WriteLine("2. Preparar Próximo Pedido");
            Console.WriteLine("3. Finalizar Próximo Prato");
            Console.WriteLine("4. Organizar Estoque");
            Console.WriteLine("5. Adicionar ao Estoque");
            Console.WriteLine("6. Exibir Estoque");
            Console.WriteLine("7. Sair\n");
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out int escolha))
            {
                switch (escolha)
                {
                    case 1:
                        // Adicionar Pedido
                        Pedido novoPedido = CriarNovoPedido();
                        restaurante.AdicionarPedido(novoPedido);
                        break;
                    case 2:
                        // Preparar Próximo Pedido
                        restaurante.PrepararProximoPedido();
                        break;
                    case 3:
                        // Finalizar Próximo Prato
                        restaurante.FinalizarProximoPrato();
                        break;
                    case 4:
                        // Organizar Estoque
                        restaurante.OrganizarEstoque();
                        break;
                    case 5:
                        // Adicionar ao Estoque
                        Ingrediente novoIngrediente = CriarNovoIngrediente();
                        restaurante.AdicionarAoEstoque(novoIngrediente);
                        break;
                    case 6:
                        // Exibir Estoque
                        restaurante.ExibirEstoque();
                        break;
                    case 7:
                        // Sair
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }

        } while (true);
    }

    private static Pedido CriarNovoPedido()
    {
        Pedido novoPedido = new Pedido();
        Console.Write("Quantos pratos no pedido? ");
        if (int.TryParse(Console.ReadLine(), out int numPratos) && numPratos > 0)
        {
            novoPedido.Pratos = new List<Prato>();
            for (int i = 0; i < numPratos; i++)
            {
                Prato novoPrato = new Prato();
                Console.Write($"Nome do Prato {i + 1}: ");
                novoPrato.Nome = Console.ReadLine();

                Console.Write($"Quantos ingredientes no Prato {i + 1}? ");
                if (int.TryParse(Console.ReadLine(), out int numIngredientes) && numIngredientes > 0)
                {
                    novoPrato.Ingredientes = new List<Ingrediente>();
                    for (int j = 0; j < numIngredientes; j++)
                    {
                        Ingrediente novoIngrediente = new Ingrediente();
                        Console.Write($"Nome do Ingrediente {j + 1}: ");
                        novoIngrediente.Nome = Console.ReadLine();

                        Console.Write($"Quantidade do Ingrediente {j + 1}: ");
                        if (int.TryParse(Console.ReadLine(), out int quantidade) && quantidade > 0)
                        {
                            novoIngrediente.Quantidade = quantidade;
                        }
                        else
                        {
                            Console.WriteLine("Quantidade inválida. O ingrediente não será adicionado.");
                            continue;
                        }

                        Console.Write($"Data de Validade do Ingrediente {j + 1} (formato: dd/mm/yyyy): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dataValidade))
                        {
                            novoIngrediente.DataValidade = dataValidade;
                        }
                        else
                        {
                            Console.WriteLine("Data de validade inválida. O ingrediente não será adicionado.");
                            continue;
                        }

                        novoPrato.Ingredientes.Add(novoIngrediente);
                    }
                }
                else
                {
                    Console.WriteLine("Número inválido de ingredientes. O prato não será adicionado.");
                    continue;
                }

                novoPedido.Pratos.Add(novoPrato);
            }
        }
        else
        {
            Console.WriteLine("Número inválido de pratos. O pedido não será adicionado.");
        }

        return novoPedido;
    }

    private static Ingrediente CriarNovoIngrediente()
    {
        Ingrediente novoIngrediente = new Ingrediente();
        Console.Write("Nome do Ingrediente: ");
        novoIngrediente.Nome = Console.ReadLine();

        Console.Write("Quantidade do Ingrediente: ");
        if (int.TryParse(Console.ReadLine(), out int quantidade) && quantidade > 0)
        {
            novoIngrediente.Quantidade = quantidade;
        }
        else
        {
            Console.WriteLine("Quantidade inválida. O ingrediente não será adicionado.");
            return null;
        }

        Console.Write("Data de Validade do Ingrediente (formato: dd/mm/yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime dataValidade))
        {
            novoIngrediente.DataValidade = dataValidade;
        }
        else
        {
            Console.WriteLine("Data de validade inválida. O ingrediente não será adicionado.");
            return null;
        }

        return novoIngrediente;
    }
}
