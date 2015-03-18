using LeertjeVanRommelen;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    [TestFixture]
    public class ProgramOptionsTests
    {
        private ProgramOptions _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new ProgramOptions();
        }

        [TestCase("-source")]
        [TestCase("--source")]
        [TestCase("-s")]
        public void WhenArgsAreInterpretedAndSourceIsSet_ThenFillSourceFile(string argName)
        {
            const string expected = "test.csv";
            var args = new[] { argName, expected };

            _subject.InterpretArguments(args);

            _subject.SourceFile.ShouldBe(expected);
        }

        [TestCase("-target")]
        [TestCase("--target")]
        [TestCase("--t")]
        public void WhenArgsAreInterpretedAndTargetIsSet_ThenFillConnectionString(string argName)
        {
            const string expected = "Dummy";
            var args = new[] { argName, expected };

            _subject.InterpretArguments(args);

            _subject.ConnectionString.ShouldBe(expected);
        }
    }
}