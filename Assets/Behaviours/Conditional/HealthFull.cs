namespace Behaviours.Conditional
{
    public class HealthFull : Conditional
    {
        protected override bool Condition()
        {
            return _brain.HealthFull;
        }
    }
}