using System.Collections.Generic;

namespace AzFunc1.Models
{
    public class MessageContent
    {
        public string Chain { get; set; }

        public List<Products> Products { get; set; }
    }

    public class Products
    {
        public string Ean { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string Img { get; set; }
    }
}