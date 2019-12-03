using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace dados
{
    class Rank
    {
        public string Vencedor { get; set; }
        public int Jogadas { get; set; }
        public DateTime Momento { get; set; }

        public Rank(string vencedor, int jogadas)
        {
            Vencedor = vencedor;
            Jogadas = jogadas;
            Momento = DateTime.Now;
        }
    }
}
