using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

//Aluno: Erick Martins

namespace PadariaPaoPrecioso
{
    [Serializable]
    public class DadosProdutosContainer
    {
        public float Caixa { get; set; }
        public dadosProduto[] Produtos { get; set; }
        public List<dadosVenda> Vendas { get; set; }
    }
    [Serializable]
    public struct dadosProduto
    {
        public string descricaoProduto;
        public float valorUnitario;
        public int qtdEstoque;
        public string dataFabricacao;
        public int prazoValidade; // em dias
    }
    [Serializable]
    public struct dadosVenda
    {
        public string descricaoProduto;
        public float valorUnitario;
        public int quantidade;
        public float valorTotal;
        public string formaPagamento;
        public DateTime dataHoraVenda; // Adicionado campo para a data e hora da venda
    }

    class Program
    {
        static float caixa = 0; // Variável para armazenar o valor do caixa
        static string arquivoProdutos = "produtosSalvos.bin"; // Nome do arquivo para armazenar os dados serializados
        static string arquivoVendas = "vendasRealizadas.bin"; // Nome do arquivo para armazenar as vendas realizadas

        static void Main(string[] args)
        {
            DadosProdutosContainer dados = CarregarDados();
            float caixa = dados.Caixa;
            dadosProduto[] produtos = CarregarProdutos();
            List<dadosVenda> vendas = CarregarVendas(); // Carregar vendas realizadas

            int opcao;

            while (true)
            {
                Console.WriteLine("--------------------------------------------\n");
                Console.WriteLine("SISTEMA DE GESTÃO PANIFICAÇÃO - PAO PRECIOSO\n");
                Console.WriteLine("--------------------------------------------\n");
                Console.WriteLine("1 - Cadastrar Produto");
                Console.WriteLine("2 - Listar Produtos");
                Console.WriteLine("3 - Realizar Venda");
                Console.WriteLine("4 - Visualizar Caixa");
                Console.WriteLine("5 - Listar Vencidos");
                Console.WriteLine("6 - Excluir Produto");
                Console.WriteLine("7 - Gerar Relatório de Fluxo de Caixa");
                Console.WriteLine("8 - Gerar Relatório de Inventário");
                Console.WriteLine("0 - Sair do Sistema\n");
                Console.WriteLine("--------------------------------------------\n");
                Console.Write("Digite a opção desejada: ");
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.Clear();
                    Console.WriteLine("Opção inválida! Por favor, digite um número válido.\n");
                    continue; // Volte para o início do loop
                }

                if (opcao == 0)
                {
                    Console.WriteLine("Salvando e Saindo...");
                    SalvarProdutos(produtos);
                    SalvarVendas(vendas); // Salvar vendas realizadas
                    GerarRelatorioFluxoCaixa(caixa); // Adicionar chamada para gerar relatório de fluxo de caixa
                    GerarRelatorioInventario(produtos); // Adicionar chamada para gerar relatório de inventário
                    break;
                }
                Console.Clear();
                switch (opcao)
                {
                    case 1:
                        CadastrarProduto(produtos);
                        break;
                    case 2:
                        ListarProdutos(produtos);
                        break;
                    case 3:
                        RealizarVenda(produtos, vendas);
                        break;
                    case 4:
                        VisualizarCaixa(vendas);
                        break;
                    case 5:
                        ListarProdutosVencidos(produtos);
                        break;
                    case 6:
                        ExcluirProduto(produtos);
                        break;
                    case 7:
                        GerarRelatorioFluxoCaixa(caixa); // Adicionar chamada para gerar relatório de fluxo de caixa
                        break;
                    case 8:
                        GerarRelatorioInventario(produtos); // Adicionar chamada para gerar relatório de inventário
                        break;
                    default:
                        Console.WriteLine("Opção Inválida!");
                        break;
                }
            }
        }

        static public void CadastrarProduto(dadosProduto[] produtos)
        {
            Console.WriteLine("--------------------------------------------\n");
            Console.WriteLine("            CADASTRO DE PRODUTOS            \n");
            Console.WriteLine("--------------------------------------------\n");
            int indice = ObterIndice(produtos);
            Console.Write("Produto: ");
            string descricaoProduto = Console.ReadLine();

            // Verifica se o nome do produto não é nulo ou vazio
            while (string.IsNullOrWhiteSpace(descricaoProduto))
            {
                Console.WriteLine("Nome do produto não pode ser nulo ou vazio. Digite novamente:");
                descricaoProduto = Console.ReadLine();
            }

            produtos[indice].descricaoProduto = descricaoProduto;

            // Tratamento para a entrada do valor unitário
            bool valorUnitarioValido = false;
            do
            {
                Console.Write("Valor Unitário do Produto: ");
                string inputValorUnitario = Console.ReadLine();

                if (float.TryParse(inputValorUnitario, out float valorUnitario))
                {
                    produtos[indice].valorUnitario = valorUnitario;
                    valorUnitarioValido = true;
                }
                else
                {
                    Console.WriteLine("Valor unitário inválido. Digite um número válido.");
                }
            } while (!valorUnitarioValido);

            bool quantidadeValida = false;
            do
            {
                Console.Write("Quantidade Inicial do Estoque: ");
                if (int.TryParse(Console.ReadLine(), out int quantidadeEstoque))
                {
                    produtos[indice].qtdEstoque = quantidadeEstoque;
                    quantidadeValida = true;
                }
                else
                {
                    Console.WriteLine("Quantidade inválida. Digite um número válido para a quantidade em estoque.");
                }
            } while (!quantidadeValida);

            Console.Write("Data de Fabricação ex.: (23/11/2023): ");
            string dataFabricacao = Console.ReadLine();

            // Verifica se a data de fabricação não é nula ou vazia
            while (string.IsNullOrWhiteSpace(dataFabricacao))
            {
                Console.WriteLine("Data de fabricação não pode ser nula ou vazia. Digite novamente:");
                dataFabricacao = Console.ReadLine();
            }

            produtos[indice].dataFabricacao = dataFabricacao;

            Console.WriteLine("Prazo de Validade em dias: ");
            string inputPrazoValidade = Console.ReadLine();

            // Verifica se o prazo de validade não é nulo ou vazio
            while (string.IsNullOrWhiteSpace(inputPrazoValidade))
            {
                Console.WriteLine("Prazo de validade não pode ser nulo ou vazio. Digite novamente:");
                inputPrazoValidade = Console.ReadLine();
            }

            if (int.TryParse(inputPrazoValidade, out int prazoValidade))
            {
                produtos[indice].prazoValidade = prazoValidade;
                SalvarProdutos(produtos);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Por favor, digite um número válido para o prazo de validade.");
            }
        }

        static public void ListarProdutos(dadosProduto[] produtos)
        {
            Console.WriteLine("---------------------------------------\n");
            Console.WriteLine("           LISTA DE PRODUTOS           \n");
            Console.WriteLine("---------------------------------------\n");

            // Ordenar os produtos por ordem alfabética
            Array.Sort(produtos, 0, produtos.Length, Comparer<dadosProduto>.Create((p1, p2) => string.Compare(p1.descricaoProduto, p2.descricaoProduto)));

            bool produtosEncontrados = false;

            for (int i = 0; i < produtos.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(produtos[i].descricaoProduto) && produtos[i].qtdEstoque > 0)
                {
                    Console.WriteLine($"Produto: {produtos[i].descricaoProduto}");
                    Console.WriteLine($"Valor Unitário: R${produtos[i].valorUnitario}");
                    Console.WriteLine($"Quantidade Em Estoque: {produtos[i].qtdEstoque}");
                    Console.WriteLine($"Data de Fabricação: {produtos[i].dataFabricacao}");
                    Console.WriteLine($"Validade: {produtos[i].prazoValidade} dias.\n");
                    Console.WriteLine("---------------------------------------\n");

                    produtosEncontrados = true;
                }
            }

            if (!produtosEncontrados)
            {
                Console.WriteLine("Nenhum Produto Disponível!");
            }

            Console.WriteLine("\n");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Console.Clear();
        }

        static public void ListarProdutosVencidos(dadosProduto[] produtos)
        {
            Console.WriteLine("---------------------------------------\n");
            Console.WriteLine("       LISTA DE PRODUTOS VENCIDOS      \n");
            Console.WriteLine("---------------------------------------\n");

            bool produtosVencidosEncontrados = false;

            for (int i = 0; i < produtos.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(produtos[i].descricaoProduto) && produtos[i].qtdEstoque > 0 && ProdutoEstaVencido(produtos[i]))
                {
                    Console.WriteLine($"Produto: {produtos[i].descricaoProduto}");
                    Console.WriteLine($"Valor Unitário: R${produtos[i].valorUnitario}");
                    Console.WriteLine($"Quantidade Em Estoque: {produtos[i].qtdEstoque}");
                    Console.WriteLine($"Data de Fabricação: {produtos[i].dataFabricacao}");
                    Console.WriteLine($"Validade: {produtos[i].prazoValidade} dias.\n");
                    Console.WriteLine("---------------------------------------\n");

                    produtosVencidosEncontrados = true;
                }
            }

            if (!produtosVencidosEncontrados)
            {
                Console.WriteLine("Nenhum Produto Vencido Encontrado!");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Console.Clear();
        }

        static private bool ProdutoEstaVencido(dadosProduto produto)
        {
            DateTime dataFabricacao = DateTime.Parse(produto.dataFabricacao);
            DateTime dataValidade = dataFabricacao.AddDays(produto.prazoValidade);
            return DateTime.Now > dataValidade;
        }

        static public int ObterIndice(dadosProduto[] produtos)
        {
            int indice;
            for (indice = 0; indice < produtos.Length; indice++)
            {
                if (string.IsNullOrEmpty(produtos[indice].descricaoProduto))
                {
                    break;
                }
            }
            return indice;
        }

        private static int EncontrarProduto(dadosProduto[] produtos, string nomeProduto)
        {
            for (int i = 0; i < produtos.Length; i++)
            {
                if (produtos[i].descricaoProduto == nomeProduto)
                {
                    return i;
                }
            }
            return -1;
        }

        private static void VisualizarCaixa(List<dadosVenda> vendas)
        {
            Console.WriteLine("---------------------------------------\n");
            Console.WriteLine("            VISUALIZAR CAIXA           \n");
            Console.WriteLine("---------------------------------------\n");
            Console.WriteLine($"Valor no Caixa: R${caixa}\n");

            if (vendas.Count > 0)
            {
                Console.WriteLine("Produtos Vendidos:");
                Console.WriteLine("------------------");

                foreach (var venda in vendas)
                {
                    Console.WriteLine($"Produto: {venda.descricaoProduto}");
                    Console.WriteLine($"Quantidade: {venda.quantidade}");
                    Console.WriteLine($"Valor Total: R${venda.valorTotal}");
                    Console.WriteLine($"Forma de Pagamento: {venda.formaPagamento}");
                    Console.WriteLine($"Data e Hora da Venda: {venda.dataHoraVenda}");
                    Console.WriteLine("------------------");
                }
            }
            else
            {
                Console.WriteLine("Nenhum produto foi vendido ainda.");
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void RealizarVenda(dadosProduto[] produtos, List<dadosVenda> vendas)
        {
            Console.WriteLine("---------------------------------------\n");
            Console.WriteLine("             REALIZAR VENDA            \n");
            Console.WriteLine("---------------------------------------\n");

            // Ordenar os produtos por ordem alfabética
            Array.Sort(produtos, (p1, p2) => string.Compare(p1.descricaoProduto, p2.descricaoProduto));

            // Mostrar produtos disponíveis
            Console.WriteLine("Produtos Disponíveis:");
            Console.WriteLine("---------------------");

            bool produtosDisponiveis = false;

            for (int i = 0; i < produtos.Length; i++)
            {
                if (!string.IsNullOrEmpty(produtos[i].descricaoProduto) && produtos[i].qtdEstoque > 0)
                {
                    Console.WriteLine($"{i + 1}. {produtos[i].descricaoProduto} | Estoque: {produtos[i].qtdEstoque} | Preço: {produtos[i].valorUnitario}");
                    produtosDisponiveis = true;
                }
            }

            if (!produtosDisponiveis)
            {
                Console.WriteLine("Nenhum produto disponível para venda.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.WriteLine("\nDigite o número correspondente ao produto que deseja vender (ou 0 para cancelar): ");
            string inputEscolhaProduto = Console.ReadLine();

            if (!int.TryParse(inputEscolhaProduto, out int escolhaProduto))
            {
                Console.WriteLine("Opção inválida. Certifique-se de digitar um número válido.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            int indiceProduto = escolhaProduto - 1;

            if (indiceProduto < 0 || indiceProduto >= produtos.Length || string.IsNullOrEmpty(produtos[indiceProduto].descricaoProduto) || produtos[indiceProduto].qtdEstoque <= 0)
            {
                Console.WriteLine("Opção de produto inválida ou sem estoque disponível.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.WriteLine($"\nProduto: {produtos[indiceProduto].descricaoProduto}");
            Console.WriteLine($"Produtos em estoque: {produtos[indiceProduto].qtdEstoque}");
            Console.WriteLine($"Valor Unitário: R${produtos[indiceProduto].valorUnitario}\n");

            Console.WriteLine("Informe a quantidade a ser vendida: ");
            int quantidadeVenda = int.Parse(Console.ReadLine());

            if (quantidadeVenda > produtos[indiceProduto].qtdEstoque)
            {
                Console.WriteLine("Quantidade insuficiente em estoque!");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            float valorTotal = quantidadeVenda * produtos[indiceProduto].valorUnitario;

            Console.WriteLine($"\nValor Total: R${valorTotal}\n");
            Console.WriteLine("Informe a forma de pagamento: ");
            Console.WriteLine("1 - Cartão de Crédito");
            Console.WriteLine("2 - PIX");
            Console.WriteLine("3 - Dinheiro\n");

            int escolhaPagamento = int.Parse(Console.ReadLine());

            string formaPagamento = "";

            switch (escolhaPagamento)
            {
                case 1:
                    Console.WriteLine("Pagamento realizado com cartão.");
                    formaPagamento = "Cartão de Crédito";
                    // implementação do pagamento com cartão aqui
                    break;
                case 2:
                    Console.WriteLine("Pagamento realizado com PIX.");
                    formaPagamento = "PIX";
                    // implementação do pagamento com PIX aqui
                    break;
                case 3:
                    Console.WriteLine("Pagamento realizado com dinheiro");
                    formaPagamento = "Dinheiro";
                    // implementação do pagamento com dinheiro aqui
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }

            // Atualizar estoque
            produtos[indiceProduto].qtdEstoque -= quantidadeVenda;

            int indiceVenda = ObterIndice(vendas);
            vendas.Add(new dadosVenda
            {
                descricaoProduto = produtos[indiceProduto].descricaoProduto,
                valorUnitario = produtos[indiceProduto].valorUnitario,
                quantidade = quantidadeVenda,
                valorTotal = valorTotal,
                formaPagamento = formaPagamento,
                dataHoraVenda = DateTime.Now // Captura a data e hora atual da venda
            });

            // Atualizar caixa
            caixa += valorTotal;

            Console.Clear();
        }

        private static int ObterIndice(List<dadosVenda> vendas)
        {
            int indice;
            for (indice = 0; indice < vendas.Count; indice++)
            {
                if (vendas[indice].descricaoProduto == null)
                {
                    break;
                }
            }
            return indice;
        }

        static void SalvarVendas(List<dadosVenda> vendas)
        {
            try
            {
                using (FileStream stream = new FileStream(arquivoVendas, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, vendas);
                }

                Console.WriteLine("Vendas realizadas salvas com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar as vendas realizadas: {ex.Message}");
            }
        }

        static List<dadosVenda> CarregarVendas()
        {
            try
            {
                if (File.Exists(arquivoVendas))
                {
                    using (FileStream stream = new FileStream(arquivoVendas, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return (List<dadosVenda>)formatter.Deserialize(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar as vendas realizadas: {ex.Message}");
            }

            return new List<dadosVenda>();
        }

        private static int ObterIndice(dadosVenda[] vendas)
        {
            int indice;
            for (indice = 0; indice < vendas.Length; indice++)
            {
                if (vendas[indice].descricaoProduto == null)
                {
                    break;
                }
            }
            return indice;
        }

        static void SalvarProdutos(dadosProduto[] produtos)
        {
            try
            {
                DadosProdutosContainer container = new DadosProdutosContainer
                {
                    Produtos = produtos
                };

                using (FileStream stream = new FileStream(arquivoProdutos, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, container);
                }

                Console.WriteLine("Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar os dados: {ex.Message}");
            }
        }

        static public void ExcluirProduto(dadosProduto[] produtos)
        {
            Console.WriteLine("---------------------------------------\n");
            Console.WriteLine("          EXCLUSÃO DE PRODUTO          \n");
            Console.WriteLine("---------------------------------------\n");

            for (int i = 0; i < produtos.Length; i++)
            {
                if (!string.IsNullOrEmpty(produtos[i].descricaoProduto) && produtos[i].qtdEstoque > 0)
                {
                    Console.WriteLine($"{i + 1}. {produtos[i].descricaoProduto} - Estoque: {produtos[i].qtdEstoque} - Preço: {produtos[i].valorUnitario}");
                }
            } // Mostra a lista de produtos antes da exclusão

            Console.WriteLine("\nDigite o número correspondente ao produto que deseja excluir (ou 0 para cancelar): ");
            string inputEscolhaProduto = Console.ReadLine();

            if (!int.TryParse(inputEscolhaProduto, out int escolhaProduto))
            {
                Console.WriteLine("Opção inválida. Certifique-se de digitar um número válido.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            int indiceProduto = escolhaProduto - 1;

            if (indiceProduto < 0 || indiceProduto >= produtos.Length || string.IsNullOrEmpty(produtos[indiceProduto].descricaoProduto) || produtos[indiceProduto].qtdEstoque <= 0)
            {
                Console.WriteLine("Opção de produto inválida ou sem estoque disponível.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.WriteLine($"\nVocê está prestes a excluir o seguinte produto:\n");
            Console.WriteLine($"Produto: {produtos[indiceProduto].descricaoProduto}");
            Console.WriteLine($"Valor Unitário: R${produtos[indiceProduto].valorUnitario}");
            Console.WriteLine($"Quantidade Em Estoque: {produtos[indiceProduto].qtdEstoque}");
            Console.WriteLine($"Data de Fabricação: {produtos[indiceProduto].dataFabricacao}");
            Console.WriteLine($"Validade: {produtos[indiceProduto].prazoValidade} dias.\n");

            Console.WriteLine("Deseja realmente excluir o produto? (S/N): ");
            string confirmacao = Console.ReadLine();

            if (confirmacao.ToUpper() == "S")
            {
                // Remover o produto da lista
                produtos[indiceProduto] = new dadosProduto();
                SalvarProdutos(produtos); // Salvar as alterações
                Console.Clear();
                Console.WriteLine("Produto excluído com sucesso!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Exclusão cancelada pelo usuário.");
            }

            Console.ReadKey();
            Console.Clear();
        }

        static dadosProduto[] CarregarProdutos()
        {
            try
            {
                if (File.Exists(arquivoProdutos))
                {
                    using (FileStream stream = new FileStream(arquivoProdutos, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        DadosProdutosContainer container = (DadosProdutosContainer)formatter.Deserialize(stream);
                        return container.Produtos;
                    }
                }
                else
                {
                    // Se o arquivo não existir, cria uma nova matriz de produtos
                    return new dadosProduto[100];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar os dados: {ex.Message}");
            }

            return new dadosProduto[100];
        }

        private static void GerarRelatorioFluxoCaixa(float caixa)
        {
            string arquivoFluxoCaixa = "relatorioFluxoCaixa.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(arquivoFluxoCaixa))
                {
                    sw.WriteLine("RELATÓRIO DE FLUXO DE CAIXA");
                    sw.WriteLine($"Data e Hora: {DateTime.Now}");
                    sw.WriteLine($"Valor no Caixa: R${caixa}");
                }

                Console.WriteLine($"Relatório de Fluxo de Caixa gerado com sucesso em {arquivoFluxoCaixa}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar o relatório de fluxo de caixa: {ex.Message}");
            }
        }

        private static void GerarRelatorioInventario(dadosProduto[] produtos)
        {
            string arquivoInventario = "relatorioInventario.txt";

            try
            {
                // Filtra os produtos não nulos ou vazios
                var produtosValidos = produtos.Where(p => !string.IsNullOrEmpty(p.descricaoProduto)).ToArray();

                // Ordena os produtos alfabeticamente por descrição
                Array.Sort(produtosValidos, (p1, p2) => string.Compare(p1.descricaoProduto, p2.descricaoProduto));

                using (StreamWriter sw = new StreamWriter(arquivoInventario))
                {
                    sw.WriteLine("RELATÓRIO DE INVENTÁRIO");
                    sw.WriteLine($"Data e Hora: {DateTime.Now}\n");

                    foreach (var produto in produtosValidos)
                    {
                        sw.WriteLine($"Produto: {produto.descricaoProduto}");
                        sw.WriteLine($"Valor Unitário: R${produto.valorUnitario}");
                        sw.WriteLine($"Quantidade em Estoque: {produto.qtdEstoque}");
                        sw.WriteLine($"Data de Fabricação: {produto.dataFabricacao}");
                        sw.WriteLine($"Prazo de Validade: {produto.prazoValidade} dias");
                        sw.WriteLine();
                    }
                }

                Console.WriteLine($"Relatório de Inventário gerado com sucesso em {arquivoInventario}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar o relatório de inventário: {ex.Message}");
            }
        }

        static DadosProdutosContainer CarregarDados()
        {
            try
            {
                if (File.Exists(arquivoProdutos))
                {
                    using (FileStream stream = new FileStream(arquivoProdutos, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return (DadosProdutosContainer)formatter.Deserialize(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar os dados: {ex.Message}");
            }

            return new DadosProdutosContainer();
        }

        static void SalvarDados(DadosProdutosContainer dados)
        {
            try
            {
                using (FileStream stream = new FileStream(arquivoProdutos, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, dados);
                }

                Console.WriteLine("Dados salvos com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar os dados: {ex.Message}");
            }
        }
    }
}
