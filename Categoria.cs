﻿namespace pizzeria_web_api
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Categoria() { }

        public Categoria(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
