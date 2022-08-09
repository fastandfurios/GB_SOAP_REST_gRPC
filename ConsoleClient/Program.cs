#region references
using ConsoleClient.Application.Commands;
using ConsoleClient.Application.Enums;
using ConsoleClient.Application.Models;
using LibraryWebService;
#endregion

#region phrases
Console.WriteLine("Клиент приветствует вас! Выбирете действие, которое требуется выполнить");
Console.WriteLine("1 - добавить книгу;");
Console.WriteLine("2 - удалить книгу;");
Console.WriteLine("3 - изменить данные книги;");
Console.WriteLine("4 - отобразить весь список книг;");
Console.WriteLine("5 - поиск книги по ее идентификатору;");
Console.WriteLine("6 - поиск книг определенной категории;");
Console.WriteLine("7 - поиск книг определенного автора;");
Console.WriteLine("8 - поиск книг по названию");
Console.WriteLine("9 - выход");
#endregion

var number = 0;

do
{
    Console.WriteLine();
    Console.Write("Ввод: ");
    int.TryParse(Console.ReadLine(), out number);

    switch (number)
    {
        case (int)Numbers.One:
            Add();
            break;
        case (int)Numbers.Two:
            Delete();
            break;
        case (int)Numbers.Three:
            Update();
            break;
        case (int)Numbers.Four:
            GetAll();
            break;
        case (int)Numbers.Five:
            GetById();
            break;
        case (int)Numbers.Six:
            GetBooksByCategory();
            break;
        case (int)Numbers.Seven:
            GetBooksByAuthor();
            break;
        case (int)Numbers.Eight:
            GetBooksByTitle();
            break;
    }

    if(number == (int)Numbers.Nine) break;
} while (true);

#region methods
static void Add()
{
    var command = new AddCommand();

    var book = BuildBook(new Book());

    var result = command.Execute(book);

    Console.WriteLine(result > 0 ? "Книга успешно добавлена" : "Не удалось добавить книгу");
}

static void Delete()
{
    var command = new DeleteCommand();
    var book = new Book();
    Console.Write("Id: ");
    book.Id = Console.ReadLine();
    
    var result = command.Execute(book);

    Console.WriteLine(result > 0 ? "Книга успешно удалена" : "Не удалось удалить книгу");
}

static void GetAll()
{
    var command = new GetAllCommand();
    var books = command.Execute(new object());
    Display(books);
}

static void GetBooksByAuthor()
{
    var command = new GetBooksByAuthorCommand();
    Console.Write("Введите автора: ");
    var authorName = Console.ReadLine();
    var books = command.Execute(authorName);
    Display(books);
}

static void GetBooksByCategory()
{
    var command = new GetBooksByCategoryCommand();
    Console.Write("Введите категорию: ");
    var category = Console.ReadLine();
    var books = command.Execute(category);
    Display(books);
}

static void GetById()
{
    var command = new GetByIdCommand();
    Console.Write("Введите id: ");
    var id = Console.ReadLine();
    var book = command.Execute(id);

    Console.WriteLine($"Книга: {book.Title} {book.AgeLimit} {book.Category} {book.Lang} {book.Pages} {book.PublicationDate}");
    foreach (var author in book.Authors)
        Console.WriteLine($"Автор(ы): {author.Name} {author.Lang}");
}

static void GetBooksByTitle()
{
    var command = new GetBooksByTitleCommand();
    Console.Write("Введите название книги: ");
    var title = Console.ReadLine();
    var books = command.Execute(title);
    Display(books);
}

static void Update()
{
    var command = new UpdateCommand();

    var book = new Book();
    Console.Write("Id: ");
    book.Id = Console.ReadLine();

    var updateBook = BuildBook(book);

    var result = command.Execute(updateBook);

    Console.WriteLine(result > 0 ? "Книга успешно обновлена" : "Не удалось обновить книгу");
}

static void Display(List<BookClient> books)
{
    Console.WriteLine();
    foreach (var book in books)
    {
        Console.WriteLine($"Книга: {book.Title} {book.AgeLimit} {book.Category} {book.Lang} {book.Pages} {book.PublicationDate}");
        foreach (var author in book.Authors)
            Console.WriteLine($"Автор: {author.Name} {author.Lang}");
    }
}

static Book BuildBook(Book book)
{
    Console.Write("Title: ");
    book.Title = Console.ReadLine();
    Console.Write("Category: ");
    book.Category = Console.ReadLine();
    Console.Write("Lang: ");
    book.Lang = Console.ReadLine();
    Console.Write("Pages: ");
    int.TryParse(Console.ReadLine(), out var pages);
    book.Pages = pages;
    Console.Write("AgeLimit: ");
    int.TryParse(Console.ReadLine(), out var ageLimit);
    book.AgeLimit = ageLimit;
    Console.Write("Количество авторов: ");
    int.TryParse(Console.ReadLine(), out var number);
    var authors = new Author[number];

    for (int i = 0; i < authors.Length; i++)
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Lang: ");
        var lang = Console.ReadLine();

        authors[i] = new Author
        {
            Name = name,
            Lang = lang
        };
    }

    book.Authors = authors;
    Console.Write("PublicationDate: ");
    int.TryParse(Console.ReadLine(), out var publicationData);
    book.PublicationDate = publicationData;

    return book;
}
#endregion