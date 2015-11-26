using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using Language;
using NUnit.Framework;

namespace MartianDbProxy.Controllers
{
	[TestFixture]
	[UseReporter(typeof(DiffReporter))]
	public class DbProxyController_specs
	{
	    private DbProxyController controller;


	    [SetUp]
	    public void SetUp()
	    {
            controller = new DbProxyController();
	    }

		[Test]
		public void greatings_on_get()
		{
			var response = controller.Post(null);
			Approvals.Verify(response.ErrorMessage.ToEarth() + "\n");
		}

		[Test]
		public void access_to_all_content()
		{
			var responses = controller.Contents.Select(c => controller.Post(new AccessRequest { SecurityToken = c.SecurityToken.ToMars(), ApiKey = DbProxyController.apiKey.ToMars()})).ToList();
		    foreach (var response in responses)
                Assert.IsNull(response.ErrorMessage);
            Approvals.VerifyAll(responses.Select(r => r.NextSecurityLevelChallenge), "responseNextChallenge");
		}
        
        [Test]
		public void no_acces_if_wrong_securityToken()
		{
            var response = controller.Post(new AccessRequest { SecurityToken = "sdfasfdasfdasdfasdf", ApiKey = DbProxyController.apiKey.ToMars() });
			Approvals.Verify(response.ErrorMessage.ToEarth());
		}
		[Test]
		public void no_acces_if_wrong_apiKey()
		{
            var response = controller.Post(new AccessRequest { SecurityToken = controller.Contents[0].SecurityToken, ApiKey = "badKey" });
			Approvals.Verify(response.ErrorMessage.ToEarth());
		}

		[Test]
		public void no_acces_if_no_apiKey()
		{
            var response = controller.Post(new AccessRequest { SecurityToken = controller.Contents[0].SecurityToken });
			Approvals.Verify(response.ErrorMessage.ToEarth());
		}
	}
}