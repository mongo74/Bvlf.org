using System;
using System.Collections.Generic;
using bvlf_v2.Models;

namespace bvlf_v2.Helpers
{
    [Serializable]
    public class RadioButtonListViewModel<T>
    {
        private List<RadioButtonListItem<T>> listItems;
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

        public string Id { get; set; }

        public List<RadioButtonListItem<T>> ListItems
        {
            get { return listItems; }

            set
            {
                listItems = value;
                UpdatedSelectedItems();
            }
        }

        public void Repopulate(List<RadioButtonListItem<T>> newListItems)
        {
            listItems = newListItems;
            UpdatedSelectedItems();
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