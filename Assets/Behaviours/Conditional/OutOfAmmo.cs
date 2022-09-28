namespace Behaviours.Conditional
{
    public class OutOfAmmo : Conditional
    {
        protected override bool Condition()
        {
            return _brain.OutOfAmmo;
        }
    }
}