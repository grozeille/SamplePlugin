using System;

namespace SamplePlugin.Catalog.Api.Model
{
    public class BuyRequest
    {
        public int ProductId { get; set; }

        public String Address { get; set; }

        public String BuyerEmail { get; set; }

        public int Quantity { get; set; }
    }
}
