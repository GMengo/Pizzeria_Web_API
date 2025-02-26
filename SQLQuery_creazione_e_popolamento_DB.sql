        CREATE TABLE Pizza
    (
        [Id] INT NOT NULL identity(1,1) PRIMARY KEY,
        [nome] VARCHAR(50) NOT NULL,
        [descrizione] VARCHAR(255) NOT NULL,
        [prezzo] FLOAT NOT NULL
    )
        CREATE TABLE Categoria
    (
        [Id] INT NOT NULL identity(1,1) PRIMARY KEY,
        [nome] VARCHAR(100) NOT NULL
    )

    alter table pizza
    add categoriaId int null

    alter table pizza
    add constraint fk_Pizza_Categoria foreign key(categoriaId) references Categoria(Id);
    -- ON DELETE SET NULL;  -- Se si elimina una categoria, le pizze rimangono ma senza categoria (sarebbe un alternativa al codice scritto con gestione direttamente da DB e non da codice)

        CREATE TABLE Ingrediente
    (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [nome] VARCHAR(50) not null
    )

        CREATE TABLE PizzaIngrediente
    (
        pizzaId INT,
        ingredienteId INT,
        PRIMARY KEY (PizzaId, IngredienteId),
        FOREIGN KEY (PizzaId) REFERENCES Pizza(Id),
        -- ON DELETE CASCADE,  -- Se si elimina una pizza, le sue associazioni vengono rimosse (sarebbe un alternativa al codice scritto con gestione direttamente da DB e non da codice)
        FOREIGN KEY (IngredienteId) REFERENCES Ingrediente(Id)
        -- ON DELETE CASCADE  -- Se si elimina un ingrediente, l'associazione viene rimossa (sarebbe un alternativa al codice scritto con gestione direttamente da DB e non da codice)
    )

    insert into Categoria(nome) values('bianca'),('rossa'),('margherita');

    INSERT INTO Pizza(nome, descrizione, prezzo) VALUES
    ('Diavola', 'base margherita con salame piccante', 9.5),
    ('Quattro Formaggi', 'mozzarella, gorgonzola, fontina e parmigiano', 10.0),
    ('Capricciosa', 'pomodoro, mozzarella, prosciutto cotto, funghi, carciofi e olive', 10.5),
    ('Boscaiola', 'mozzarella, salsiccia e funghi', 9.0),
    ('Marinara', 'pomodoro, aglio, origano e olio EVO', 6.5),
    ('Tonno e Cipolla', 'pomodoro, mozzarella, tonno e cipolla rossa', 9.0),
    ('Wurstel e Patatine', 'mozzarella, wurstel e patatine fritte', 8.5),
    ('Bufalina', 'pomodoro, mozzarella di bufala e basilico', 11.0),
    ('Frutti di Mare', 'pomodoro, frutti di mare misti e prezzemolo', 12.5),
    ('Ortolana', 'mozzarella, zucchine, melanzane e peperoni grigliati', 9.5);

    CREATE TABLE Utente
    (
        Id int primary key identity (1,1) not null,
        Email nvarchar(50) not null unique,
        PasswordHash nvarchar(255) not null
    );


    CREATE TABLE Ruolo
    (
        Id INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
        Nome NVARCHAR(50) NOT NULL UNIQUE
    );

    CREATE TABLE UtenteRuolo
    (
        UtenteId INT NOT NULL,
        RuoloId INT NOT NULL,
        PRIMARY KEY (UtenteId, RuoloId),
        FOREIGN KEY (UtenteId) REFERENCES Utente(Id),
        FOREIGN KEY (RuoloId) REFERENCES Ruolo(Id)
    );

    insert into utente(email,passwordhash) values ('admin@prova.com','AQAAAAIAAYagAAAAEEOoenBKf+Hd6FfY57xO9/Ik08TsH5Vi7H7+cbhDkyqyyoiWpx6sLnFC8WLiJ3ys6g=='); -- per testare il login la password è l' hash di "prova"
    -- questo è un modo manuale di dare un ruolo superiore a un utente come l' admin, senza fare una chiamata api ma un interazione diretta da DB
    insert into ruolo (nome) values ('admin');
    insert into utenteruolo(utenteid,ruoloid) values (1,1); -- diamo il ruolo di admin al primo utente inserito nel DB in questo caso all' utente con l' email: admin@prova.com 