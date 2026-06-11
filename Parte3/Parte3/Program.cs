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

                // Trata a letra 'E' como vazio
                if (cadeia == "E") cadeia = "";

                Console.WriteLine($"\nTestando cadeia: \"{cadeia}\"");

                bool aceita = SimularMt(cadeia);
                Console.WriteLine($"Resultado Final: {(aceita ? "ACEITA" : "REJEITA")}");
                Console.WriteLine("--------------------------------------------------------------------------");
            }

            // Desafio
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
            // Definição Formal
            Dictionary<int, char> fita = new Dictionary<int, char>();
            for (int i = 0; i < cadeia.Length; i++) fita[i] = cadeia[i];

            // Primeira célula da fita
            int cabecote = 0;

            // Estado inicial
            string estadoAtual = "q0";

            int passos = 0;

            // Limite de Segurança
            int limitePassos = 1000;

            // String vazia rejeitada
            if (cadeia.Length == 0)
                return false;

            while (estadoAtual != "qaccept" && estadoAtual != "qreject" && passos < limitePassos)
            {
                // Lê o caractere atual da fita. Se o cabeçote estiver fora dos limites vira '_'
                char simboloAtual = fita.ContainsKey(cabecote) ? fita[cabecote] : '_';

                ExibirPasso(estadoAtual, cabecote, fita);

                passos++;

                // TABELA DE TRANSIÇÕES

                // Estado q0: Estado de Varredura/Início. Busca o primeiro 'a' livre para marcar.
                if (estadoAtual == "q0")
                {
                    if (simboloAtual == 'a') { fita[cabecote] = 'X'; cabecote++; estadoAtual = "q1"; } // Marca 'a' com 'X', move Direita e vai procurar 'b'
                    else if (simboloAtual == 'Y') { cabecote++; estadoAtual = "q4"; } // Acabaram os 'a's, vai para o estado de checagem final
                    else estadoAtual = "qreject"; // Símbolo fora de ordem ou inesperado
                }
                // Estado q1: Procura o primeiro caractere 'b' correspondente para casar
                else if (estadoAtual == "q1")
                {
                    if (simboloAtual == 'a' || simboloAtual == 'Y') { cabecote++; } // Ignora outros 'a's ou 'Y's já marcados e avança para a Direita
                    else if (simboloAtual == 'b') { fita[cabecote] = 'Y'; cabecote++; estadoAtual = "q2"; } // Marca 'b' com 'Y', move Direita e vai procurar 'c'
                    else estadoAtual = "qreject";
                }
                // Estado q2: Procura o primeiro caractere 'c' correspondente para casar
                else if (estadoAtual == "q2")
                {
                    if (simboloAtual == 'b' || simboloAtual == 'Z') { cabecote++; } // Ignora outros 'b's ou 'Z's já marcados e avança para a Direita
                    else if (simboloAtual == 'c') { fita[cabecote] = 'Z'; cabecote--; estadoAtual = "q3"; } // Marca 'c' com 'Z', muda direção para Esquerda
                    else estadoAtual = "qreject";
                }
                // Estado q3: Retorna o cabeçote para o início da fita até encontrar a última marcação 'X' feita
                else if (estadoAtual == "q3")
                {
                    if (simboloAtual == 'a' || simboloAtual == 'b' || simboloAtual == 'Y' || simboloAtual == 'Z') { cabecote--; } // Move Esquerda
                    else if (simboloAtual == 'X') { cabecote++; estadoAtual = "q0"; } // Encontrou a barreira 'X', avança uma célula e reinicia o ciclo
                }
                // Estado q4: Estado de Verificação de Consistência Final (Garante que não sobraram símbolos órfãos)
                else if (estadoAtual == "q4")
                {
                    if (simboloAtual == 'Y' || simboloAtual == 'Z') { cabecote++; } // Avança checando se o restante da fita está limpo
                    else if (simboloAtual == '_' || simboloAtual == ' ') { estadoAtual = "qaccept"; } // Encontrou o final limpo da fita: Palavra Válida!
                    else estadoAtual = "qreject"; // Encontrou um símbolo incorreto, quebrando o balanceamento
                }
            }


            if (passos >= limitePassos)
                Console.WriteLine("[AVISO] Atingiu o limite máximo de passos!");

            return estadoAtual == "qaccept";
        }

        static void SimularMT_Computadora(string entrada)
        {
            Dictionary<int, char> fita = new Dictionary<int, char>();
            for (int i = 0; i < entrada.Length; i++) fita[i] = entrada[i];

            int cabecote = 0;
            string estadoAtual = "q_inicio";
            int passos = 0;

            // Processamento da fita em unário
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
                        // Espaço branco vira '1'
                        fita[cabecote] = '1';
                        estadoAtual = "q_fim"; // Parar
                    }
                }
            }
            ExibirPasso(estadoAtual, cabecote, fita);

            //Conta oque sobrou
            int contagem = 0;
            foreach (var v in fita.Values) if (v == '1') contagem++;
            Console.WriteLine($"Fita de saída contem: {contagem} símbolos '1'.");
        }

        static void ExibirPasso(string estado, int cabecote, Dictionary<int, char> fita)
        {
            int min = 0, max = 5;
            // Define dinamicamente o tamanho do desenho baseado nos limites atuais do dicionário
            foreach (var k in fita.Keys)
            {
                if (k < min) min = k;
                if (k > max) max = k;
            }
            max += 2; // Adiciona margem visual para simular o infinito em branco à direita

            string representacaoFita = "";
            for (int i = min; i <= max; i++)
            {
                char c = fita.ContainsKey(i) ? fita[i] : '_';

                // Exigência Visual: Isola o caractere sob o cabeçote atual com delimitadores [ ]
                if (i == cabecote)
                    representacaoFita += $"[{c}]";
                else
                    representacaoFita += $" {c} ";
            }
            Console.WriteLine($"Estado: {estado,-9} | Posição Cabeçote: {cabecote,-2} | Fita: {representacaoFita}");
        }
    }
}