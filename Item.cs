// One item on the shopping list.
class Item
{
    public string Name { get; set; }
    public int Price { get; set; }

    public Item(string name, int price)
    {
        // Ett ogiltigt objekt ska aldrig kunna skapas, så vi kontrollerar innan något sparas.
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Namnet får inte vara tomt.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Priset får inte vara negativt.");
        }
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price} kr";
    }
}
