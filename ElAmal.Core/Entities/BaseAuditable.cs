using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities
{
    public abstract class BaseAuditable<TKey> : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
