
namespace AutogenDevelopmentTeam.Console.Services
{
    public class ApplicationBuilder
    {
        private readonly List<string> _steps;

        public ApplicationBuilder()
        {
            _steps = new List<string>();
        }

        public void AddStep(string step)
        {
            _steps.Add(step);
        }

        public void DisplaySteps()
        {
            System.Console.WriteLine("Steps to implement the discussed application:");
            foreach (var step in _steps)
            {
                System.Console.WriteLine($"- {step}");
            }
        }

        public List<string> GetSteps()
        {
            return _steps;
        }
    }
}