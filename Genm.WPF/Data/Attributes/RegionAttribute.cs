using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genm.WPF.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegionAttribute : Attribute
    {
        public string Name { get; private set; }

        public RegionAttribute(string name)
        {
            this.Name = name;
        }
    }
}
