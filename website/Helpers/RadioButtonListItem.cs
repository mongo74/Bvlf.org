namespace bvlf_v2.Models
{
    public class RadioButtonListItem<T>
    {
        public bool Selected { get; set; }
        public T Value { get; set; }
        public string Text { get; set; }
        public string ClassName { get; set; }
        public bool IsVolzet { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}