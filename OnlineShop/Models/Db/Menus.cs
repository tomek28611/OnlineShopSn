﻿using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db
{
    public partial class Menus
    {
        public int Id { get; set; }

        public string? MenuTitle { get; set; }

        public string? Link { get; set; }

        public string? Type { get; set; }
    }
}
