// Erick Martins
// @erickapenas
// Sistema de RESERVA usando LISTA

using System;
using System.Collections.Generic;

public class Program
{
    static List<string> horariosDisponiveis = new List<string>();
    static Dictionary<string, string> agendamentos = new Dictionary<string, string>();

    public static void Main()
    {
        InicializarHorarios();
        Menu();
    }

    static void InicializarHorarios()
    {
        for (int i = 0; i < 24; i++)
        {
            horariosDisponiveis.Add(i.ToString("D2") + ":00");
        }
    }

    static void Menu()
    {
        int opcao = -1;
        while (opcao != 0)
        {
            Console.WriteLine("\nDigite seu nome:");
            string nome = Console.ReadLine();

            Console.WriteLine($"\nOlá, {nome}. Escolha uma opção:");
            Console.WriteLine("1 - Agendar horário");
            Console.WriteLine("2 - Desmarcar horário");
            Console.WriteLine("3 - Ver todos os horários disponíveis");
            opcao = Convert.ToInt32(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    AgendarHorario(nome);
                    break;
                case 2:
                    DesmarcarHorario(nome);
                    break;
                case 3:
                    MostrarHorarios();
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static void AgendarHorario(string nome)
    {
        Console.WriteLine("Horários disponíveis:");
        foreach (string horario in horariosDisponiveis)
        {
            Console.WriteLine(horario);
        }
        Console.WriteLine("Digite o horário que deseja agendar:");
        string horarioAgendado = Console.ReadLine();
        if (horariosDisponiveis.Contains(horarioAgendado))
        {
            agendamentos[horarioAgendado] = nome;
            horariosDisponiveis.Remove(horarioAgendado);
            Console.WriteLine("Horário agendado com sucesso!");
        }
        else
        {
            Console.WriteLine("Horário não disponível. Tente novamente.");
        }
    }

    static void DesmarcarHorario(string nome)
    {
        Console.WriteLine("Digite o horário que deseja desmarcar:");
        string horarioDesmarcado = Console.ReadLine();
        if (agendamentos.ContainsKey(horarioDesmarcado) && agendamentos[horarioDesmarcado] == nome)
        {
            agendamentos.Remove(horarioDesmarcado);
            horariosDisponiveis.Add(horarioDesmarcado);
            Console.WriteLine("Horário desmarcado com sucesso!");
        }
        else
        {
            Console.WriteLine("Horário não encontrado. Tente novamente.");
        }
    }

    static void MostrarHorarios()
    {
        Console.WriteLine("\nTodos os horários disponíveis:");
        for (int i = 0; i < 24; i++)
        {
            string data = DateTime.Now.ToString("[dd/MM/yyyy]");
            string horario = i.ToString("D2") + ":00";
            string status = agendamentos.ContainsKey(horario) ? $"Agendado por {agendamentos[horario]}" : "Disponível";
            Console.WriteLine($"{data} - {horario} - {status}");
        }
    }
}
