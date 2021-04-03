using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTFG.CustomTabs
{
    public class CustomTab : StackLayout { }

    public class CustomTabs : StackLayout
    {
        private Color _selectedColor = Color.White;
        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        private Color _unselectedColor = Color.WhiteSmoke;
        public Color UnselectedColor
        { 
            get { return _unselectedColor; }
            set
            {
                _unselectedColor = value;
            }
        }

        internal List<CustomTabButton> TabButtons
        {
            get
            {
                CustomTabButtons tabButtons =
                   (CustomTabButtons)Children.First(c => c.GetType() == typeof(CustomTabButtons));
                var buttonEnumerable = tabButtons.Children.Select(c => (CustomTabButton)c);
                var buttonList = buttonEnumerable.Where(c => c.GetType() == typeof(CustomTabButton)).ToList();
                return buttonList;
            }
        }

        internal List<CustomTab> Tabs
        {
            get
            {
                var childList = Children.Where(c => c.GetType() == typeof(CustomTab));
                var tabList = childList.Select(c => (CustomTab)c).ToList();
                return tabList;
            }
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;

                if (Tabs.Count > 0)
                    SelectionUIUpdate();
            }
        }

        public CustomTabButton SelectedTabButton
        {
            get { return TabButtons[_selectedTabIndex]; }
            set
            {
                var tabIndex = TabButtons.FindIndex(t => t == value);
                if (tabIndex == -1)
                    throw new Exception(
                       "SelectedTabButton asignó un botón que no se encuentra entre los elementos secundarios de CustomTabButtons.");
                if (tabIndex != _selectedTabIndex)
                    SelectedTabIndex = tabIndex;
            }
        }

        public CustomTab SelectedTab
        {
            get { return Tabs[_selectedTabIndex]; }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (TabButtons.Count != Tabs.Count)
            {
                throw new Exception("El número de botones y el número de pestañas no coinciden.");
            }
            SelectionUIUpdate();
        }

        private void SelectionUIUpdate()
        {
            foreach (var btn in TabButtons)
            {
                btn.BackgroundColor = UnselectedColor;
                btn.BorderWidth = 0;
            }

            SelectedTabButton.BackgroundColor = SelectedColor;
            SelectedTabButton.BorderWidth = 1;
            SelectedTabButton.BorderColor = Color.WhiteSmoke;

            foreach (var tb in Tabs)
                tb.IsVisible = false;
            SelectedTab.IsVisible = true;
        }
    }
}
