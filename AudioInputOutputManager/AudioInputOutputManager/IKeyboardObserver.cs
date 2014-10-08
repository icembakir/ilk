using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioInputOutputManager
{
    public interface IKeyboardObserver
    {
        double KeyPressed(char inKey);
    }
}
