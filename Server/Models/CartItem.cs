namespace PawsomeProject.Server.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int Qty { get; set; }


    }
}
