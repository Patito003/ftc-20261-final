using System;
using System.Collections.Generic;
using System.IO;

namespace Parte1
{
    class Program
    {
        // Definição Formal
        static List<string> Q = new List<string> { "q0", "q1", "q2" };
        static List<char> Sig = new List<char> { 'a', 'b' };
        static string q0 = "q0";
        static List<string> F = new List<string> { "q2" };

        // Função de transição
        static Dictionary<(string estado, char simbolo), string> Del = new Dictionary<(string, char), string>();

        static void Main(string[] args)
        {
            Del.Add(("q0", 'a'), "q1");
            Del.Add(("q0", 'b'), "q0");
            Del.Add(("q1", 'a'), "q1");
            Del.Add(("q1", 'b'), "q2");
            Del.Add(("q2", 'a'), "q1");
            Del.Add(("q2", 'b'), "q0");

            ExibirDiagrama();

            string[] linhas = LerArquivoEntrada();
            if (linhas.Length > 0)
                ProcessarCadeias(linhas);
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
                return Array.Empty<string>();
            }

            return File.ReadAllLines(arquivo);
        }

        static void ProcessarCadeias(string[] linhas)
        {
            Console.WriteLine("\n=================== PROCESSANDO CADEIAS ===================");

            foreach (string linha in linhas)
            {
                // Tratando linha vazia
                string cadeia = linha.Trim();

                List<string> historico;
                bool aceita = Aceitar(cadeia, out historico);

                string rastroFormatado = string.Join(" -> ", historico);
                string resultadoTxt = aceita ? "ACEITA" : "REJEITA";

                Console.WriteLine($"Cadeia: \"{cadeia}\"");
                Console.WriteLine($"Estados percorridos: {rastroFormatado}");
                Console.WriteLine($"Resultado: {resultadoTxt}");
                Console.WriteLine("-----------------------------------------------------------");
            }
        }

        static bool Aceitar(string cadeia, out List<string> historico)
        {
            historico = new List<string>();
            string estadoAtual = q0;
            historico.Add(estadoAtual);

            foreach (char simbolo in cadeia)
            {
                if (!Sig.Contains(simbolo))
                    return false;

                if (Del.ContainsKey((estadoAtual, simbolo)))
                {
                    estadoAtual = Del[(estadoAtual, simbolo)];
                    historico.Add(estadoAtual);
                }
                else
                    return false;
            }

            return F.Contains(estadoAtual);
        }
    }
}