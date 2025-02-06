using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http.HttpResults;

namespace pizzeria_web_api.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il titolo non può avere più di 50 caratteri")]
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        [Range(0.1, 1000)]
        public decimal Prezzo { get; set; }
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public List<int> IngredientiId { get; set; } = new List<int>();
        public List<Ingredienti> ingredienti { get; set; } = new List<Ingredienti>();


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
