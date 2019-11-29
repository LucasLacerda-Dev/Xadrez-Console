using System;
using System.Collections.Generic;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoClaro = ConsoleColor.Cyan;
            ConsoleColor fundoEscuro = ConsoleColor.DarkCyan;

            Console.WriteLine("  ┌──────────────────┐");
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " │ ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    // Alterna as cores do fundo
                    if ((i + j) % 2 == 0)
                        Console.BackgroundColor = fundoClaro;
                    else
                        Console.BackgroundColor = fundoEscuro;
                    ImprimirPeca(tab.Peca(i, j));
                }
                Console.BackgroundColor = fundoOriginal;
                Console.WriteLine(" │ ");
            }
            Console.WriteLine("  └──────────────────┘");
            Console.WriteLine("    a b c d e f g h");
        }

        public static void ImprimirPartida(PartidaDeXadrez partida)
        {
            Tela.ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine($"Turno {partida.Turno}");
            if (!partida.Terminada)
            {
                Console.WriteLine($"Aguardando jogada: {partida.JogadorAtual}");
                if (partida.Xeque)
                    Console.WriteLine("XEQUE!");
            }
            else
            {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine($"Vencedor: {partida.JogadorAtual}");
            }
        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            ConsoleColor aux;

            Console.WriteLine("Peças capturadas: ");
            Console.Write("Brancas:");
            aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Branca));
            Console.ForegroundColor = aux;
            Console.Write("\nPretas:");
            aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            ImprimirConjunto(partida.PecasCapturadas(Cor.Preta));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }

        public static void ImprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach (Peca x in conjunto)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;
            ConsoleColor fundoClaro = ConsoleColor.Cyan;
            ConsoleColor fundoEscuro = ConsoleColor.DarkCyan;

            Console.WriteLine("  ┌──────────────────┐");
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " │ ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                        Console.BackgroundColor = fundoAlterado;
                    else if ((i + j) % 2 == 0)
                        Console.BackgroundColor = fundoClaro;
                    else
                        Console.BackgroundColor = fundoEscuro;
                    ImprimirPeca(tab.Peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine(" │ ");
            }
            Console.WriteLine("  └──────────────────┘");
            Console.WriteLine("    a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez LerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");

            return new PosicaoXadrez(coluna, linha);
        }

        public static void ImprimirPeca(Peca peca)
        {
            ConsoleColor aux;

            if (peca == null)
                Console.Write("- ");
            else
            {
                if (peca.Cor == Cor.Branca)
                {
                    aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                else
                {
                    aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }
    }
}
