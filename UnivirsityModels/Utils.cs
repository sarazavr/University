using System;
using System.Collections.Generic;
using System.Text;

namespace UnivirsityModels
{
    public class Utils
    {
        public static object Clone(object element)
        {
            return element is ICloneable ? ((ICloneable)element).Clone() : element;
        }


    }
}
