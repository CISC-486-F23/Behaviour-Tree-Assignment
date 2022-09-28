namespace Behaviours.Conditional
{
    public class HealthAvailable : Conditional
    {
        protected override bool Condition()
        {
            return _brain.HealthAvailable;
        }
    }
}