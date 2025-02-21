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
        FOREIGN KEY (IngredienteId) REFERENCES Ingrediente(Id)
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