namespace Behaviours.Conditional
{
    public class AmmoFull : Conditional
    {
        protected override bool Condition()
        {
            return _brain.AmmoFull;
        }
    }
}