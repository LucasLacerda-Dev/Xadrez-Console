using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using tabuleiro;
using xadrez;
using dados;

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
            Console.ForegroundColor = ConsoleColor.White;
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
            ConsoleColor fundoAlterado = ConsoleColor.Blue;
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
                Console.Write("  ");
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

        public static char Menu()
        {
            Console.WriteLine("---------- Xadrez ----------");
            Console.WriteLine("  (1) Novo Jogo");
            Console.WriteLine("  (2) Ranking");
            Console.WriteLine("  (3) Continuar Partida");

            Console.CursorVisible = false;
            return Console.ReadKey(true).KeyChar;
        }

        public static List<Rank> CarregarRanking()
        {
            string path = @"Dados\Ranking.txt";

            if (!File.Exists(path))
                return null;

            string json = File.ReadAllText(path);
            List<Rank> ranking = JsonConvert.DeserializeObject<List<Rank>>(json);

            return ranking;
        }

        public static void SalvarRanking(PartidaDeXadrez partida)
        {
            Console.Write("Informe seu nome:");
            string vencedor = Console.ReadLine();

            int jogadas;
            List<Rank> ranking = new List<Rank>();
            string path = @"Dados\Ranking.txt";

            if (partida.JogadorAtual == Cor.Branca)
                jogadas = partida.Turno / 2 + 1;
            else
                jogadas = partida.Turno / 2;

            if (File.Exists(path))
            {
                ranking = CarregarRanking();
            }

            ranking.Add(new Rank(vencedor, jogadas));
            JsonSerializer serializer = new JsonSerializer();

            List<Rank> theRanking;

            // LINQ
            theRanking = (from rank in ranking orderby rank.Jogadas ascending, rank.Momento ascending select rank).ToList();
            // LAMBDA
            // theRanking = ranking.OrderBy(rank => rank.Jogadas).ThenBy(rank => rank.Momento).ToList();

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, theRanking);
            }
        }

        public static void ImprimirRanking()
        {

            List<Rank> ranking = CarregarRanking();

            Console.Clear();
            if (ranking != null)
            {
                Console.WriteLine("Ranking: ");
                foreach (Rank r in ranking)
                {
                    Console.WriteLine("{0} - {1} Jogadas", r.Vencedor, r.Jogadas);
                }
            }
            else
                Console.WriteLine("Ranking vazio!");

            Console.WriteLine("\nPressione qualquer tecla para voltar...");
            Console.CursorVisible = false;
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
