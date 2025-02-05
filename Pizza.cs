using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http.HttpResults;

namespace pizzeria_web_api
{
    public class Pizza
    {
        public int Id { get;  set; }
        public string Nome { get;  set; }
        public string Descrizione { get;  set; }
        public decimal Prezzo { get;  set; }
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public Pizza() { }

        public Pizza(int id, string nome, string descrizione, decimal prezzo)
        {
            Id = id;
            Nome = nome;
            Descrizione = descrizione;
            Prezzo = prezzo;
        }
    }
}
