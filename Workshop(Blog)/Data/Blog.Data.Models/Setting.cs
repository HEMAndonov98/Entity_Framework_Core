﻿using Blog.Data.Common.Models;

namespace Blog.Data.Models
{
    using Blog.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
