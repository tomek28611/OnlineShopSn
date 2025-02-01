using System;
using System.Collections.Generic;

namespace OnlineShop.Data;

public partial class ProductGalery
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string? ImageName { get; set; }
}
