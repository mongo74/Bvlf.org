using System.Collections.Generic;
using bvlf_v2.Models;

namespace bvlf_v2.Helpers
{
    public class CheckBoxListViewModel<T>
    {
        private List<CheckBoxListItem<T>> listItems;
        private T selectedValue;

        public T SelectedValue
        {
            get { return selectedValue; }

            set
            {
                selectedValue = value;
                UpdatedSelectedItems();
            }
        }

        public List<CheckBoxListItem<T>> ListItems
        {
            get { return listItems; }

            set
            {
                listItems = value;
                UpdatedSelectedItems();
            }
        }

        private void UpdatedSelectedItems()
        {
            if (ListItems == null)
            {
                return;
            }

            ListItems.ForEach(li => li.Selected = Equals(li.Value, SelectedValue));
        }
    }
}