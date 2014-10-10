using System;

namespace SamplePlugin.Catalog.Api.Model
{
    public class BuyResponse
    {
        public bool OK { get; set; }

        public int ProductId { get; set; }

        public Guid ConfirmationGuid { get; set; }
    }
}
