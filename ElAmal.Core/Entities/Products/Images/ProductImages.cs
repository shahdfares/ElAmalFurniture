using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities.Products.Images
{
    public class ProductImages : BaseEntity<Guid>
    {
        public string ImagesUrl { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }

        public string PublicId { get; set; } = null!;

        // RelationShips

        public Guid ProductsId { get; set; }
        public Product? Products { get; set; } 
    }
}
