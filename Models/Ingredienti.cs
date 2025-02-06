namespace pizzeria_web_api.Models
{
    public class Ingredienti
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Ingredienti() { }

        public Ingredienti(int id, string nome)
        {
            this.Id = id;
            this.Nome = nome;
        }
    }
}
