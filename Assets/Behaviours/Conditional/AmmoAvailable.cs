namespace Behaviours.Conditional
{
    public class AmmoAvailable : Conditional
    {
        protected override bool Condition()
        {
            return _brain.AmmoAvailable;
        }
    }
}