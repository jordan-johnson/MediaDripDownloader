using Xunit;
using System;
using Serilog;
using MediaDrip.Downloader.Shared;

namespace MediaDrip.Downloader.Test
{
    public class LogTestFixture
    {
        public Logger LogSetup { get; private set; }

        public LogTestFixture()
        {
            LogSetup = new Logger();
        }
    }

    public class LogTest : IClassFixture<LogTestFixture>
    {
        private LogTestFixture _fixture;

        public LogTest(LogTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void PassIfLoggerCreated()
        {
            Assert.NotNull(_fixture.LogSetup);

            try
            {
                Log.Information("Passed; logger created.");
            }
            catch(System.Exception e)
            {
                Assert.True(false, $"Failed; logger not created. Exception thrown :: {e.Message}");
            }
        }

        [Fact]
        public void FailIfLoggerNotCreated()
        {

            
        }
    }
}