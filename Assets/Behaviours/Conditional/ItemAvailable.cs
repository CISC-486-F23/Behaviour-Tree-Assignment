namespace Behaviours.Conditional
{
    public class ItemAvailable : Conditional
    {
        protected override bool Condition()
        {
            return _brain.ItemAvailable;
        }
    }
}