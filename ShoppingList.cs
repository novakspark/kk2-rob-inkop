// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;

    public ShoppingList(string path)
    {
        this.path = path;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public void RemoveAt(int number)
    {
        // FEL: Programmet kraschade  med System.ArgumentOutOfRangeException när jag skrev in högre siffra
        //  än antalet varor för att "ta bort" en vara i listan.
        // ORSAK: RemoveAt(number - 1) kördes utan att kolla om numret fanns i listan.
        // När number var störrre än antalet varor pekade number -1 på en plats som inte fanns.
        // LÖSNING: if-satsen kollar om numret är giltigt och tar bort varan om det stämmer. 
        // Om numret är ogiltigt händer inget.
        if (number >= 1 && number <= items.Count)
        {
            items.RemoveAt(number - 1);
        }
    }

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        // FEL: Programmet kraschade inte, men totala summan anger fel värde, 121 kr istället 136 kr.
        // ORSAK: int i = 1 gjorde att räkningen började från plats 1 istället för att börja från plats 0 i inköpslistan
        //        vilket resulterade i att "mjölk 15 kr" missades helt.
        // LÖSNING: ändrade i = 1 till i = 0 för att första varan (plats 0) ska räknas med.
        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (item.Name == name)
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }

        // FEL: Save() dolde fel med en tom catch och skrev "Listan är sparad" även när det misslyckades.
        // ORSAK: catch-blocket var tomt och meddelandet låg utanför try, så det skrevs alltid ut.
        // LÖSNING: Meddelandet ligger i try (visas bara om skrivningen lyckades) och specifika catch
        //          för IOException och UnauthorizedAccessException skriver ut vad som gick fel.
        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
            Console.WriteLine("Listan är sparad.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Kunde inte spara listan: {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Saknar behörighet att spara listan: {ex.Message}");
        }
    }

    // Reads the file back into the list.
    public void Load()
    {
        // FEL: Programmet kraschade med FileNotFoundException om items.txt saknades.
        // ORSAK: File.ReadAllText kastar undantag när filen inte finns och inget kontrollerade det.
        // LÖSNING: File.Exists kollar först. Saknas filen avslutar Load() och listan börjar tom.
        if (!File.Exists(path))
        {
            return;
        }
        string text = File.ReadAllText(path);
        string[] lines = text.Split('\n');

        foreach (string line in lines)
        {
            // FEL: Programmet kraschade direkt vid start med IndexOutOfRangeException i Load () på rad 90.
            // ORSAK: items.txt avslutas med radbrytning och därför ger Split en tom sista rad.
            //        Den tomma raden har inget ";", så bara en plats finns för parts 0.
            //        Rad 90 förösker hämta parts 1, som inte finns, därför uppstår IndexOutOfRangeException.
            // Lösning: Kollar med string.IsNullOrWhiteSpace(line) om raden är tom, om den är det hoppar jag över med continue
            //          för att part 1 aldrig hämtas.
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            string[] parts = line.Split(';');
            items.Add(new Item(parts[1].Trim(), int.Parse(parts[0])));
        }
    }
}
