using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeghToolsUi.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ArgumentPlayableAttribute : Attribute
    {
        public string ArgumentName { get; }
        public string ArgumentDescription { get;}
        public string ArgumentIcon { get; }

        public ArgumentPlayableAttribute(string argumentName, string argumentDescription, string argumentIcon)
        {
            ArgumentName = argumentName;
            ArgumentDescription = argumentDescription;
            ArgumentIcon = argumentIcon;
        }
    }
}
