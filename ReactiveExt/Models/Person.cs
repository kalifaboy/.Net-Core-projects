namespace ReactiveExt.Models
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return $"Person : {{\n Name : {Name},\n Age : {Age},\n Address : {Address}\n}}";
        }
    }
}
