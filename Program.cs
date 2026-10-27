ShoppingList list = new ShoppingList("items.txt");
list.Load();

while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

    // FEL: Programmet kraschade när jag skrev "två" istället för en siffra i menyn.
    // ORSAK: int.Parse kastar ett undantag när texten inte går att göra om till heltal, och inget fångas.
    // LÖSNING: int.TryParse svarar antingen true eller false istället för att krascha.
    // Meddelandet "Använd siffror." visas om man skriver talet i text, och continue tar dig tillbaka till menyn.
    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Använd siffor.");
        continue;
    }

    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();
        Console.Write("Pris: ");

        // FEL: Kraschade när jag skrev text som pris.
        // ORSAK: int.Parse kastar ett undantag när texten inte går att göra om till heltal.
        // LÖSNING: int.TryParse svarar antingen true eller false istället för att krascha.
        // Vid false visas meddelande och continue tar dig tillbaka till menyn utan att varan läggs till.
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Använd siffror.");
            continue;
        }
        // Item kastar undantag vid tomt namn eller negativt pris. Vi fångar dem här
// och visar ett tydligt meddelande i stället för att krascha.
Item item;

try
{
    item = new Item(name, price);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Ogiltig vara: {ex.Message}");
    continue;
}

// Add returnerar false om varan skulle spräcka budgettaket.
if (!list.Add(item))
{
    Console.WriteLine("Varan ryms inte inom budgeten och lades inte till.");
}
    }
    else if (choice == 2)
    {
        Console.Write("Nummer: ");

        // FEL: Samma krasch som när jag skrev text som nummer vid borttagning.
        // ORSAK: samma som för priset, int.Parse kan inte tolka inskrivna texten.
        // LÖSNING: Samma som för priset, int.TryParse och continue.
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Använd siffror.");
            continue;
        }
        list.RemoveAt(number);
    }
    else if (choice == 3)
    {
        list.Save();
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    else if (choice == 5)
    {
        break;
    }
}
