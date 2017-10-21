namespace BMI
{
    public interface IBmiReport
    {
        double GetBmiIndex(double height, double weight);
        string GetBmiCategory(double index);
    }
}