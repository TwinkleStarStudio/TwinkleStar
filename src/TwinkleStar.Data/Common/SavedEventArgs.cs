using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwinkleStar.Data
{
    public class SavedEventArgs : EventArgs
    {
        public object Item = null;
        public SaveAction SaveAction = SaveAction.Insert;
        public SavedEventArgs(object item, SaveAction action)
        {
            this.Item = item;
            this.SaveAction = action;
        }
    }
}
