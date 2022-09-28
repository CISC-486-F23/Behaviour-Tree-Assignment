namespace Behaviours.Conditional
{
    public class HealthLow : Conditional
    {
        protected override bool Condition()
        {
            return _brain.HealthLow;
        }
    }
}