namespace Restouran.Common
{
    public delegate void MenuItemEventHandler(MenuItem item, string message);

    public class MenuItem
    {
        public delegate void OrderEventHandler(Order order, string status);
        public Guid IdItem {get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }    
    }
}
