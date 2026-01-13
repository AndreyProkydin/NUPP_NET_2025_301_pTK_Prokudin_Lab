using Restouran.Common;
using System;

const string FilePath = "menu.json";
MenuCrudService service = new MenuCrudService();

service.Load(FilePath);
Console.WriteLine($"\n[INFO] Loading data from {FilePath}...");

var allItems = service.ReadAll();
if (!allItems.Any())
{
    Console.WriteLine("Menu is empty.");
}
else
{
    foreach (var item in allItems)
    {
        Console.WriteLine($"ID: {item.IdItem} | Name: {item.Name} | Price: {item.Price}");
    }
}

MainDish borsch = new MainDish
{
    Name = "Borsch",
    Description = "Traditional Ukrainian soup",
    Price = 120.50f,
    IsSpicy = false,
    TypeOfDish = "Soup"
};

Dessert cheesecake = new Dessert
{
    Name = "Cheesecake",
    Description = "Classic dessert",
    Price = 95.00f,
    TypeOfDessert = "Cake"
};

service.Create(borsch);
service.Create(cheesecake);

Console.WriteLine($"Added: {borsch.Name}");
Console.WriteLine($"Added: {cheesecake.Name}");

MenuItem foundItem = service.Read(borsch.IdItem);
if (foundItem != null)
{
    Console.WriteLine($"Found: {foundItem.Name} (ID: {foundItem.IdItem})");
}

cheesecake.Price = 110.00f;
service.Update(cheesecake);
Console.WriteLine($"New price for '{cheesecake.Name}': {cheesecake.Price}");

service.Remove(borsch);
Console.WriteLine($"Item '{borsch.Name}' removed.");

foreach (var item in service.ReadAll())
{
    Console.WriteLine($"ID: {item.IdItem} | Name: {item.Name} | Price: {item.Price}");
}

service.Save(FilePath);
Console.WriteLine($"\n[INFO] Data saved to {FilePath}.");
