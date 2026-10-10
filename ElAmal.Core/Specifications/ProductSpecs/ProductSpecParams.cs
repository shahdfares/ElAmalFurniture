using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Specifications.ProductSpecs
{
    public class ProductSpecParams
    {
		private const int MaxValue = 20;

		private int PageSize = 15;

		public int pageSize
		{
			get { return PageSize; }
			set { PageSize = value > MaxValue ? MaxValue : value; }
		}

		public int PageIndex { get; set; } = 1;

        public string? Sort { get; set; }
        public string? Search { get; set; }
        public string? CategorySlug { get; set; }

    }
}
