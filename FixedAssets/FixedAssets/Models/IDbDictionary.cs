using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Models
{
    public interface IDbDictionary
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
