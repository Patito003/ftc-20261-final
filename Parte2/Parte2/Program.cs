using System;
using System.Collections.Generic;
using System.IO;

namespace Parte2
{
    class Program
    {
        // Sigma (Σ): Alfabeto de entrada
        static List<char> Sig = new List<char> { 'a', 'b' };

        // Z0: Símbolo inicial da pilha
        static char Z0 = 'Z';

        static void Main(string[] args)
        {
            // Le o arquivio de entradas
            string[] lines = LerArquivoEntrada();

            if (lines.Length > 0)
                ProcessarCadeias(lines); // Passa por cada teste e mostra os resultados
        }

        static string[] LerArquivoEntrada()
        {
            string nomeArquivo = "entradas_ap.txt";

            if (!File.Exists(nomeArquivo))
            {
                Console.WriteLine($"[ERRO] O arquivo '{nomeArquivo}' não foi encontrado.");
                return Array.Empty<string>(); // Retorna array vazio
            }
            return File.ReadAllLines(nomeArquivo);
        }

        
        static void ProcessarCadeias(string[] lines)
        {
            Console.WriteLine("================ SIMULAÇÃO AUTÔMATO DE PILHA ==================");
            foreach (string linha in lines)
            {
                string cadeia = linha.Trim();

                // Trata a letra 'E' como vazia (Epsilon)
                if (cadeia == "E")
                    cadeia = "";

                Console.WriteLine($"\nTestando cadeia: \"{cadeia}\"");

                // Simula o Autômato de Pilha
                bool aceita = SimularAP(cadeia);

                Console.WriteLine($"Resultado Final: {(aceita ? "ACEITA" : "REJEITA")}");
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        static bool SimularAP(string cadeia)
        {
            // Cria a pilha
            Stack<char> pilha = new Stack<char>();

            // Definição formal: começando com símbolo inicial (Z0)
            pilha.Push(Z0);

            // Inicia no estado q0
            string estadoAtual = "q0";

            ExibirConfiguracao(estadoAtual, cadeia, pilha);

            // Processar simbolo por simbolo
            for (int i = 0; i < cadeia.Length; i++)
            {
                char simbolo = cadeia[i];
                string entradaRestante = cadeia.Substring(i + 1);

                // Valida o alfabeto
                if (!Sig.Contains(simbolo))
                    return false;

                // FUNÇÃO DE TRANSIÇÃO

                // Regra 1: Se está em q0 e lê 'a', empilha um 'X'
                if (estadoAtual == "q0" && simbolo == 'a')
                {
                    pilha.Push('X');
                }
                // Regra 2: Se está em q0 e lê 'b', muda para o estado q1 e desempilha o 'X'
                else if (estadoAtual == "q0" && simbolo == 'b')
                {
                    estadoAtual = "q1";

                    // Valida se tem um 'X' para desempilhar.
                    if (pilha.Count > 0 && pilha.Peek() == 'X')
                        pilha.Pop();
                    else
                        return false;
                }
                // Regra 3: Se já está em q1 e continua lendo 'b', apenas desempilha os 'X'
                else if (estadoAtual == "q1" && simbolo == 'b')
                {
                    if (pilha.Count > 0 && pilha.Peek() == 'X')
                        pilha.Pop();
                    else
                        return false;
                }
                // Regra de Exceção: Qualquer outra combinação quebra a regra da linguagem
                else
                    return false;

                // Exibe o passo a passo
                ExibirConfiguracao(estadoAtual, entradaRestante, pilha);
            }

            /* Se a fita acabou, o estado é q1 e na pilha restou apenas o marcador Z0,
             realizamos uma transição vazia para desempilhar o Z0 e esvaziar a pilha por completo. */
            if (estadoAtual == "q1" && pilha.Count == 1 && pilha.Peek() == Z0)
            {
                pilha.Pop();
                ExibirConfiguracao("q_fim", "ε", pilha); // Exibi o estado
            }

            // O autômato aceita se a pilha for = 0
            return pilha.Count == 0;
        }

        static void ExibirConfiguracao(string estado, string restante, Stack<char> pilha)
        {
            char[] arr = pilha.ToArray();

            // Formata o array
            string conteudoPilha = arr.Length > 0 ? string.Join("", arr) : "[VAZIA]";

            // Se for nulo ou vazio exibe Epsilon
            string restanteFormatado = string.IsNullOrEmpty(restante) ? "ε" : restante;

            Console.WriteLine($"Estado: {estado} | Entrada Restante: {restanteFormatado,-8} | Pilha: {conteudoPilha}");
        }
    }
}