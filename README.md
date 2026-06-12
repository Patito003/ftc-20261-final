# Trabalho Final - Teoria dos Computadores e Linguagens Formais

Repositório destinado ao desenvolvimento do trabalho prático da disciplina de Teoria dos Computadores na Faculdade COTEMIG. O projeto consiste na implementação prática e simulação dos três principais modelos da Hierarquia de Chomsky: Autômatos Finitos, Autômatos de Pilha e Máquinas de Turing.

---

## Vídeo de Explicação do projeto
* *Link do Vídeo:* [Clique aqui para assistir o video de explicação do Projeto](https://drive.google.com/file/d/1-cn5KHUDp9yKhMod2AK4btVa6B33QdFZ/view)
O vídeo contém a explicação teórica e demonstração prática.

---

## Integrantes do Grupo
* *Gabriel Fonseca de Araujo* - Matrícula: 72400439
* *Paulo Vitor Martins de Castro* - Matrícula: 72400218

---

## Estrutura do Projeto

O projeto foi desenvolvido em *C# (.NET 8.0)* utilizando uma arquitetura modular dividida em três subprojetos dentro da mesma Solução (.sln):

1. *Parte1 (AFD):* Simulador de um Autômato Finito Determinístico para reconhecer a linguagem L1 (cadeias que terminam em "ab").
2. *Parte2 (AP):* Simulador de um Autômato de Pilha com aceitação por pilha vazia para a linguagem L2 (cadeias do tipo a^n b^n).
3. *Parte3 (MT):* Simulador de uma Máquina de Turing com fita dinâmica para a linguagem L4 (cadeias do tipo a^n b^n c^n) e uma MT Computadora para a função unária f(n) = n + 1.

---

## Como Executar o Projeto

### Pré-requisitos
* SDK do .NET 8.0 instalado.
* IDE Visual Studio 2022 (ou superior) ou VS Code.

### Passo a Passo

1. *Clonar o repositório:*
   Cole o link abaixo no seu terminal para clonar:
   git clone https://github.com/Patito003/ftc-20261-final.git
   cd ftc-20261-final

2. *Abrir a Solução:*
   * Abra o arquivo de solução (.sln) na pasta raiz para carregar os três projetos simultaneamente no Visual Studio.

3. *Configurar os Arquivos de Entrada:*
   * Certifique-se de que os arquivos de teste (entradas.txt, entradas_ap.txt e entradas_mt.txt) estão presentes nas respectivas pastas de cada projeto com a propriedade "Copiar se for mais novo" ativa.

4. *Executar uma Parte Específica:*
   * No painel Gerenciador de Soluções, clique com o botão direito sobre o projeto desejado (Parte1, Parte2 ou Parte3).
   * Selecione "Definir como Projeto de Inicialização".
   * Pressione F5 ou o botão de Play para rodar a simulação no console.

---

## Casos de Teste Oficiais Cobertos

### Parte 1 (AFD)
* ab, aab, bab, ababab -> *ACEITAS*
* ba, b, (cadeia vazia) -> *REJEITADAS*

### Parte 2 (AP)
* ab, aabb, aaabbb -> *ACEITAS*
* aab, abb, ba, abab, (cadeia vazia) -> *REJEITADAS*

### Parte 3 (MT)
* abc, aabbcc, aaabbbccc Momento -> *ACEITAS*
* aabbc, ab, abcabc, (cadeia vazia) -> *REJEITADAS*
* *Desafio Computadora:* Entradas unárias como "111" (n=3) são processadas e resultam em "1111" (n=4), computando com sucesso f(n) = n + 1.
, refactor(afd):).
* *Clean Code:* Lógica de manipulação de arquivos e loops extraída da função Main para métodos especialistas de responsabilidade única.
