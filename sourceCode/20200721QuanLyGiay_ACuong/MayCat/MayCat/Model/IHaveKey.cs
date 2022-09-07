using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MayCat
{
    public interface IHaveKey
    {
        Dictionary<Key, Button> KeyButtons { get; set; }
    }
}
