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
        public async Task PagesWithAmbiguousLinksCanBeProcessed_1()
        {
            GivenTheGame(new Game { StartingUrl = "https://en.wikipedia.org/wiki/Operation_Goodwood_(naval)",
                EndingUrl = "https://en.wikipedia.org/wiki/Tamagotchi" });
            GivenTheInputString("Operation Goodwood (naval) -> Dwight D. Eisenhower -> atomic bomb -> Japan -> State of Japan ->" +
                " Japanese popular culture -> Japanese pop culture in the United States -> Namco -> Bandai -> Tamagotchi");
            WhenThePathIsGenerated();
            await AndThePathIsValidated();
            ThenThePathIsValid();
        }

        [Fact]
        public async Task PagesWithAmbiguousLinksCanBeProcessed_2()
        {
            GivenTheGame(new Game
            {
                StartingUrl = "https://en.wikipedia.org/wiki/Operation_Goodwood_(naval)",
                EndingUrl = "https://en.wikipedia.org/wiki/Tamagotchi"
            });
            GivenTheInputString("Operation Goodwood (naval) -> Dwight D. Eisenhower -> atomic bomb -> Japan -> Japan ->" +
                " Japanese popular culture -> Japanese pop culture in the United States -> Namco -> Bandai -> Tamagotchi");
            WhenThePathIsGenerated();
            await AndThePathIsValidated();
            ThenThePathIsValid();
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
