using ProcessKiller;

namespace ProcessKillerTests
{
    public class Tests
    {
        private Program _program { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            _program = new Program();
        }

        [TestCase("notepad","notepad")]
        public void SameProcessTest(string processName, string processToCompare)
        {
            bool isSameProcess = _program.IsSameProcess(processName, processToCompare);

            Assert.IsTrue(isSameProcess);
        }

        [TestCase("notepad", "Notepad")]
        [TestCase("notepad", "nOTEPAD")]
        [TestCase("notepad", "notepa")]
        [TestCase("notepad", "java")]
        public void DifferentProcessTest(string processName, string processToCompare)
        {
            bool isSameProcess = _program.IsSameProcess(processName, processToCompare);

            Assert.IsFalse(isSameProcess);
        }

        [TestCase(1, 2)]
        [TestCase(2, 2)]
        public void ProcessExpiredTest(double maxLifetime, double currentLifetime)
        {
            var startTime = DateTime.Now.AddMinutes(-currentLifetime);

            bool isExpired = _program.IsExpired(maxLifetime, startTime);

            Assert.IsTrue(isExpired);
        }

        [TestCase(2, 0)]
        [TestCase(2, 1)]
        public void ProcessNotExpiredTest(double maxLifetime, double currentLifetime)
        {
            var startTime = DateTime.Now.AddMinutes(-currentLifetime);

            bool isExpired = _program.IsExpired(maxLifetime, startTime);

            Assert.IsFalse(isExpired);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ArgumentInvalidTest(double argument)
        {
            argument = _program.MakeArgumentValid(argument);

            Assert.That(argument, Is.EqualTo(1));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void ArgumentValidTest(double argument)
        {
            double correct = argument;

            argument = _program.MakeArgumentValid(argument);

            Assert.That(argument, Is.EqualTo(correct));
        }
    }
}