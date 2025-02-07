namespace pizzeria_web_api.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Ingrediente() { }

        public Ingrediente(int id, string nome)
        {
            this.Id = id;
            this.Nome = nome;
        }
    }
}
