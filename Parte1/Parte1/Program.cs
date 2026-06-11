using System;
using System.Collections.Generic;
using System.IO;

namespace Parte1
{
    class Program
    {
        // Definição Formal do AFD

        // Q: Os estados
        static List<string> Q = new List<string> { "q0", "q1", "q2" };

        // Sigma (Σ): Alfabeto de entrada
        static List<char> Sig = new List<char> { 'a', 'b' };

        // q0: Estado inicial
        static string q0 = "q0";

        // F: Estado terminal
        static List<string> F = new List<string> { "q2" };

        // Delta (δ): Função de transição
        static Dictionary<(string estado, char simbolo), string> Del = new Dictionary<(string, char), string>();

        static void Main(string[] args)
        {
            // Regras de transição
            // Objetivo: terminar com 'ab'

            Del.Add(("q0", 'a'), "q1");
            Del.Add(("q0", 'b'), "q0");
            Del.Add(("q1", 'a'), "q1");
            Del.Add(("q1", 'b'), "q2");
            Del.Add(("q2", 'a'), "q1");
            Del.Add(("q2", 'b'), "q0");

            ExibirDiagrama();

            // Le o arquivo entradas.txt
            string[] linhas = LerArquivoEntrada();

            if (linhas.Length > 0)
                ProcessarCadeias(linhas); // Passa por cada teste e mostra os resultados
        }

        static void ExibirDiagrama()
        {
            Console.WriteLine("============== TABELA DE TRANSIÇÕES (AFD) =================");
            Console.WriteLine("Estado Atual\t| Símbolo Lido\t| Próximo Estado");
            Console.WriteLine("-----------------------------------------------------------");
            foreach (var transicao in Del)
            {
                Console.WriteLine($"{transicao.Key.estado}\t\t| {transicao.Key.simbolo}\t\t| {transicao.Value}");
            }
            Console.WriteLine("===========================================================");
        }

        static string[] LerArquivoEntrada()
        {
            string arquivo = "entradas.txt";

            if (!File.Exists(arquivo))
            {
                Console.WriteLine($"\n[ERRO] O arquivo '{arquivo}' não foi encontrado.");
                Console.WriteLine("Certifique-se de que ele foi criado e suas Propriedades estão como 'Copiar se for mais novo'.");
                return Array.Empty<string>(); // Retorna array vazio pra nao quebrar
            }

            return File.ReadAllLines(arquivo);
        }

        static void ProcessarCadeias(string[] linhas)
        {
            Console.WriteLine("\n=================== PROCESSANDO CADEIAS ===================");

            foreach (string linha in linhas)
            {
                // Remove espaços em branco
                string cadeia = linha.Trim();

                List<string> historico;

                // Valida o AFD
                bool aceita = Aceitar(cadeia, out historico);

                string rastroFormatado = string.Join(" -> ", historico);
                string resultadoTxt = aceita ? "ACEITA" : "REJEITA";

                // Imprime os dados
                Console.WriteLine($"Cadeia: \"{cadeia}\"");
                Console.WriteLine($"Estados percorridos: {rastroFormatado}");
                Console.WriteLine($"Resultado: {resultadoTxt}");
                Console.WriteLine("-----------------------------------------------------------");
            }
        }

        static bool Aceitar(string cadeia, out List<string> historico)
        {
            historico = new List<string>();
            string estadoAtual = q0; // Inicia com q0

            // Guarda o estado inicial no historico
            historico.Add(estadoAtual);

            foreach (char simbolo in cadeia)
            {
                //Se o símbolo não pertence rejeita
                if (!Sig.Contains(simbolo))
                    return false;

                // Ve se a regra existe
                if (Del.ContainsKey((estadoAtual, simbolo)))
                {
                    // Move o autômato pro proximo estado
                    estadoAtual = Del[(estadoAtual, simbolo)];
                    // Guarda o novo estado
                    historico.Add(estadoAtual);
                }
                else
                    return false;
            }

            // Ve se é o estado final
            return F.Contains(estadoAtual);
        }
    }
}