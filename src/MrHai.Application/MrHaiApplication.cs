using MrHai.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Application
{
    [Export(typeof(IMrHaiApplication))]
    internal class MrHaiApplication
        : MrHaiService, IMrHaiApplication
    {
    }
}
