using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTFG.CustomTabs
{
    public class CustomTabButtons : StackLayout { }

    public class CustomTabButton : Button
    {
        public CustomTabButton()
        {
            Clicked += ThisTabButtonClicked;
        }


        public void ThisTabButtonClicked(object s, EventArgs e)
        {
            CustomTabs prnt = validParentCustomTabs();
            if (prnt == null) return;
            prnt.SelectedTabButton = this;
        }


        private CustomTabs validParentCustomTabs()
        {
            if (Parent != null && Parent.Parent != null && Parent.Parent.GetType() == typeof(CustomTabs))
                return ((CustomTabs)Parent.Parent);
            else
            {
                throw new Exception( "Error, el padre de un CustomTabButton debe ser un CustomTabs");
            }
        }
    }
}
