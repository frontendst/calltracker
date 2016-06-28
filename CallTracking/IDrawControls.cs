using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CodeFirst
{
    interface IDrawControls
    {
        Control DrawControls(Control control);
    }
}
