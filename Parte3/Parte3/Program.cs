using System;
using System.Collections.Generic;
using System.IO;

namespace Parte3
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] linhas = LerArquivoEntrada();

            Console.WriteLine("====================== PARTE 3: MÁQUINA DE TURING =======================");
            foreach (string linha in linhas)
            {
                string cadeia = linha.Trim();
                if (cadeia == "E") cadeia = "";

                Console.WriteLine($"\nTestando cadeia: \"{cadeia}\"");
                bool aceita = SimularMt(cadeia);
                Console.WriteLine($"Resultado Final: {(aceita ? "ACEITA" : "REJEITA")}");
                Console.WriteLine("--------------------------------------------------------------------------");
            }

            Console.WriteLine("\n======================== DESAFIO: MT COMPUTADORA =========================");
            string[] testes = { "1", "11", "111", "11111" };
            foreach (string entrada in testes)
            {
                Console.WriteLine($"\nEntrada Unária (n={entrada.Length}): {entrada}");
                SimularMT_Computadora(entrada);
                Console.WriteLine("--------------------------------------------------------------------------");
            }
        }

        static string[] LerArquivoEntrada()
        {
            string caminhoArquivo = "entradas_mt.txt";
            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"[ERRO] O arquivo '{caminhoArquivo}' não foi encontrado.");
                return Array.Empty<string>();
            }
            return File.ReadAllLines(caminhoArquivo);
        }

        static bool SimularMt(string cadeia)
        {
            Dictionary<int, char> fita = new Dictionary<int, char>();
            for (int i = 0; i < cadeia.Length; i++) fita[i] = cadeia[i];

            int cabecote = 0;
            string estadoAtual = "q0";
            int passos = 0;
            int limitePassos = 1000;

            if (cadeia.Length == 0)
                return false;

            while (estadoAtual != "qaccept" && estadoAtual != "qreject" && passos < limitePassos)
            {
                char simboloAtual = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';

                ExibirPasso(estadoAtual, cabecote, fita);

                passos++;

                if (estadoAtual == "q0")
                {
                    if (simboloAtual == 'a') { fita[cabecote] = 'X'; cabecote++; estadoAtual = "q1"; }
                    else if (simboloAtual == 'Y') { cabecote++; estadoAtual = "q4"; }
                    else estadoAtual = "qreject";
                }
                else if (estadoAtual == "q1")
                {
                    if (simboloAtual == 'a' || simboloAtual == 'Y') { cabecote++; }
                    else if (simboloAtual == 'b') { fita[cabecote] = 'Y'; cabecote++; estadoAtual = "q2"; }
                    else estadoAtual = "qreject";
                }
                else if (estadoAtual == "q2")
                {
                    if (simboloAtual == 'b' || simboloAtual == 'Z') { cabecote++; }
                    else if (simboloAtual == 'c') { fita[cabecote] = 'Z'; cabecote--; estadoAtual = "q3"; }
                    else estadoAtual = "qreject";
                }
                else if (estadoAtual == "q3")
                {
                    if (simboloAtual == 'a' || simboloAtual == 'b' || simboloAtual == 'Y' || simboloAtual == 'Z') { cabecote--; }
                    else if (simboloAtual == 'X') { cabecote++; estadoAtual = "q0"; }
                }
                else if (estadoAtual == "q4")
                {
                    if (simboloAtual == 'Y' || simboloAtual == 'Z') { cabecote++; }
                    else if (simboloAtual == '_' || simboloAtual == ' ') { estadoAtual = "qaccept"; }
                    else estadoAtual = "qreject";
                }
            }

            if (passos >= limitePassos) Console.WriteLine("[AVISO] Atingiu o limite máximo de passos!");
            return estadoAtual == "qaccept";
        }

        static void SimularMT_Computadora(string entrada)
        {
            Dictionary<int, char> fita = new Dictionary<int, char>();
            for (int i = 0; i < entrada.Length; i++) fita[i] = entrada[i];

            int cabecote = 0;
            string estadoAtual = "q_inicio";
            int passos = 0;

            while (estadoAtual != "q_fim" && passos < 500)
            {
                char simboloAtual = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';
                ExibirPasso(estadoAtual, cabecote, fita);
                passos++;

                if (estadoAtual == "q_inicio")
                {
                    if (simboloAtual == '1') { cabecote++; }
                    else if (simboloAtual == '_' || simboloAtual == ' ')
                    {
                        fita[cabecote] = '1';
                        estadoAtual = "q_fim";
                    }
                }
            }
            ExibirPasso(estadoAtual, cabecote, fita);

            int contagem = 0;
            foreach (var v in fita.Values) if (v == '1') contagem++;
            Console.WriteLine($"Fita de saída contem: {contagem} símbolos '1'.");
        }

        static void ExibirPasso(string estado, int cabecote, Dictionary<int, char> fita)
        {
            int min = 0, max = 5;
            foreach (var k in fita.Keys)
            {
                if (k < min) min = k;
                if (k > max) max = k;
            }
            max += 2; // Formatar pra ver melhor

            string representacaoFita = "";
            for (int i = min; i <= max; i++)
            {
                char c = fita.ContainsKey(i) ? fita[i] : '_';
                if (i == cabecote)
                    representacaoFita += $"[{c}]";
                else
                    representacaoFita += $" {c} ";
            }
            Console.WriteLine($"Estado: {estado,-9} | Posição Cabeçote: {cabecote,-2} | Fita: {representacaoFita}");
        }
    }
}