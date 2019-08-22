using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiGameBot.Core;
using WikiGameBot.Core.PathValidation;
using WikiGameBot.Data.Entities;
using Xunit;

namespace Tests
{
    public class PathValidationTests
    {
        public PathValidator _pathValidator { get; set; }
        public string _inputString { get; set; }
        private readonly PageListExtractor _pageListExtractor;
        public Game _game { get; set; }
        public ValidationData _pathValidationData { get; set; }
        public List<string> _linkPath { get; set; }

        public PathValidationTests()
        {
            _pathValidator = new PathValidator();
            _pageListExtractor = new PageListExtractor();
        }

        [Fact]
        public async Task InvalidPathCanBeDetected()
        {
            GivenTheGame(new Game { StartingUrl = "https://en.wikipedia.org/wiki/Philip_Miller", EndingUrl = "https://en.wikipedia.org/wiki/Kim_Jong-il" });
            GivenTheInputString("Philip MIller -> Royal Society -> Third World War -> Korean War -> North Korea -> Kim Jong-il");
            WhenThePathIsGenerated();
            await AndThePathIsValidated();
            ThenThePathIsNotValid();
        }

        [Fact]
        public async Task PathLinkCanBeValidated()
        {
            GivenTheGame(new Game { StartingUrl = "https://en.wikipedia.org/wiki/Philip_Miller", EndingUrl = "https://en.wikipedia.org/wiki/Kim_Jong-il" });
            GivenTheInputString("Philip MIller -> Royal Society -> Second World War -> Korean War -> North Korea -> Kim Jong-il");
            WhenThePathIsGenerated();
            await AndThePathIsValidated();
            ThenThePathIsValid();
        }

        private void GivenTheGame(Game game)
        {
            _game = game;
        }

        private void ThenThePathIsNotValid()
        {
            Assert.False(_pathValidationData.IsValid);
        }
        private void ThenThePathIsValid()
        {
            Assert.True(_pathValidationData.IsValid);
        }

        private async Task AndThePathIsValidated()
        {
            _pathValidationData = await _pathValidator.Validate(_game,_linkPath);
        }

        private void WhenThePathIsGenerated()
        {
            _linkPath = _pageListExtractor.GeneratePageList(_inputString);
        }

        private void GivenTheInputString(string inputString)
        {
            _inputString = inputString;
        }
    }
}
