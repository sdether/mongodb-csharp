using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public interface IValueConverter
    {
        object ConvertFromDocumentValue(object documentValue);

        object ConvertToDocumentValue(object memberValue);
    }
}
