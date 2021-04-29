using System;

namespace Genm.WPF.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class IconPathAttribute : Attribute
    {
        public string Path { get; set; }

        public IconPathAttribute(string path)
        {
            this.Path = path;
        }
    }

}
