using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;

namespace BillPath.ViewModels
{
    public struct NamedColor
    {
        public NamedColor(string name, Color color)
        {
            if (string.IsNullOrWhiteSpace(name))
                if (name == null)
                    throw new ArgumentNullException("name");
                else
                    throw new ArgumentException("Cannot be empty or white space!", "name");

            _name = name;
            _color = color;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
        public string LocalizedName
        {
            get
            {
                return ResourceLoader.GetForViewIndependentUse("ColorNames").GetString(_name);
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
        }

        private readonly string _name;
        private readonly Color _color;
    }
}