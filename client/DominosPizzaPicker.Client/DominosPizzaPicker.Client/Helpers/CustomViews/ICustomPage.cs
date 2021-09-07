using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DominosPizzaPicker.Client.Helpers.CustomViews
{
    public interface ICustomPage<V> where V : CustomViewModel
    {
        V ViewModel { get; set; }
    }
}
