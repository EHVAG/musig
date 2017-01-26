using System;
using EHVAG.MusiGServer.Controller;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using StatsHelix.Charizard;

namespace EHVAG.MusiGServer.Controller.Tests
{
    [TestFixture]
    public partial class ChannelControllerTest
    {
        /// <summary>Test-Stub für Add(String)</summary>
        [Test]
        public HttpResponse AddTest([PexAssumeUnderTest]ChannelController target, string channelId)
        {
            HttpResponse result = target.Add(channelId);
            return result;
            // TODO: Assertionen zu Methode ChannelControllerTest.AddTest(ChannelController, String) hinzufügen
        }
    }
}
