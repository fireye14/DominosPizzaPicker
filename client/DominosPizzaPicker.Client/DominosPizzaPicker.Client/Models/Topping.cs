namespace DominosPizzaPicker.Client.Models
{
    public class Topping
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMeat { get; set; }
        public bool IsCheese { get; set; }
        public bool Used { get; set; }
    }
}
