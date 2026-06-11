using System;
using System.Collections.Generic;
using System.IO;

namespace Parte2
{
    class Program
    {
        static List<char> Sig = new List<char> { 'a', 'b' };
        static char Z0 = 'Z';

        static void Main(string[] args)
        {
            string[] linhas = LerArquivoEntrada();

            if (linhas.Length > 0)
                ProcessarCadeias(linhas);
        }

        static string[] LerArquivoEntrada()
        {
            string nomeArquivo = "entradas_ap.txt";

            if (!File.Exists(nomeArquivo))
            {
                Console.WriteLine($"[ERRO] O arquivo '{nomeArquivo}' não foi encontrado.");
                return Array.Empty<string>();
            }
            return File.ReadAllLines(nomeArquivo);
        }

        static void ProcessarCadeias(string[] linhas)
        {
            Console.WriteLine("================ SIMULAÇÃO AUTÔMATO DE PILHA ==================");
            foreach (string linha in linhas)
            {
                string cadeia = linha.Trim();
                if (cadeia == "E")
                    cadeia = "";

                Console.WriteLine($"\nTestando cadeia: \"{cadeia}\"");
                bool aceita = SimularAP(cadeia);

                Console.WriteLine($"Resultado Final: {(aceita ? "ACEITA" : "REJEITA")}");
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        static bool SimularAP(string cadeia)
        {
            Stack<char> pilha = new Stack<char>();
            pilha.Push(Z0);

            string estadoAtual = "q0";

            // Exibe a config inicial
            ExibirConfiguracao(estadoAtual, cadeia, pilha);

            for (int i = 0; i < cadeia.Length; i++)
            {
                char simbolo = cadeia[i];
                string entradaRestante = cadeia.Substring(i + 1);

                if (!Sig.Contains(simbolo))
                    return false;

                if (estadoAtual == "q0" && simbolo == 'a')
                    pilha.Push('X');
                else if (estadoAtual == "q0" && simbolo == 'b')
                {
                    estadoAtual = "q1";

                    if (pilha.Count > 0 && pilha.Peek() == 'X')
                        pilha.Pop();
                    else
                        return false;
                }
                else if (estadoAtual == "q1" && simbolo == 'b')
                {
                    if (pilha.Count > 0 && pilha.Peek() == 'X')
                        pilha.Pop();
                    else
                        return false;
                }
                else
                    return false;

                ExibirConfiguracao(estadoAtual, entradaRestante, pilha);
            }

            // Transição Vazia
            if (estadoAtual == "q1" && pilha.Count == 1 && pilha.Peek() == Z0)
            {
                pilha.Pop();
                ExibirConfiguracao("q_fim", "ε", pilha);
            }

            return pilha.Count == 0;
        }

        static void ExibirConfiguracao(string estado, string restante, Stack<char> pilha)
        {
            char[] arr = pilha.ToArray();
            string conteudoPilha = arr.Length > 0 ? string.Join("", arr) : "[VAZIA]";
            string restanteFormatado = string.IsNullOrEmpty(restante) ? "ε" : restante;

            Console.WriteLine($"Estado: {estado} | Entrada Restante: {restanteFormatado,-8} | Pilha: {conteudoPilha}");
        }
    }
}