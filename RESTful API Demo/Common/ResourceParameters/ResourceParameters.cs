using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ResourceParameters
{
    public abstract class ResourceParameters
    {
        private const int MAX_PAGE_SIZE = 20;
        public virtual string SearchQuery { get; set; }
        public virtual int PageNumber { get; set; } = 1;
        private int _PageSize = 10;
        public virtual int PageSize
        {
            get => this._PageSize;
            set => this._PageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }
    }
}
