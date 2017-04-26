using System.Collections.Generic;

namespace bvlf_v2.Models
{
    public class CustomListViewModel<T>
    {
        private List<CustomListItem<T>> listItems;
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

        public List<CustomListItem<T>> ListItems
        {
            get { return listItems; }

            set
            {
                listItems = value;
                UpdatedSelectedItems();
            }
        }

        public string Id { get; set; }

        public void Repopulate(List<CustomListItem<T>> newListItems)
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

    public class CustomListItem<T>
    {
        public bool Selected { get; set; }
        public string Text { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}