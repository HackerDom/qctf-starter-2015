using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Http;
using Language;

namespace MartianDbProxy.Controllers
{
	public class DbProxyController : ApiController
	{
		public AccessResponse Post([FromBody]AccessRequest request)
		{
			if (request == null)
				return new AccessResponse
				{
					ErrorMessage = MartianLanguage.ProvideValidXml,
					NextSecurityLevelChallenge = "0C0D57E66FE7401C82B97338658421BD",
					Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(MartianLanguage.Greetings))
				};
			if (request.ApiKey == null)
				return new AccessResponse { ErrorMessage = MartianLanguage.NoApiKey };
			if (request.SecurityToken == null)
				return new AccessResponse { ErrorMessage = MartianLanguage.NoSecurityToken};
			if (!request.ApiKey.IsMartian())
				return new AccessResponse { ErrorMessage = MartianLanguage.WrongLanguageInApiKey };
			if (request.ApiKey.ToEarth().ToUpper() != apiKey)
				return new AccessResponse { ErrorMessage = MartianLanguage.BadApiKey };
			var token = request.SecurityToken;
			if (!token.IsMartian())
				return new AccessResponse { ErrorMessage = MartianLanguage.WrongLanguageInSecurityToken };
			token = token.ToEarth().ToUpper();
			var requestedContent = Contents.FirstOrDefault(c => c.SecurityToken == token);
			if (requestedContent == null)
				return new AccessResponse { ErrorMessage = MartianLanguage.NoAccess };
			return new AccessResponse
			{
				Content = requestedContent.GetContentAsBase64(),
				NextSecurityLevelChallenge = requestedContent.NextLevelChallenge
			};
		}

		public readonly SecuredContent[] Contents =
		{
			new SecuredContent(LoadOcrTask(), "2290A406DDE94918A7A29BB6D254F64D", "AAF893E1D631430799F7F0FF27308162"),
			new SecuredContent(MartianLanguage.Flag1, "FFD737F3E0894D1296066F5F8BD262C4", "2DD4F4C12C3A43BDA3AD5F707725DA74"),
			new SecuredContent(MartianLanguage.Flag2, "ADEB04680ADC4E4FAB59C158A89ED420", "BC7017B8A3E4466D98B80F762D47CB15"),
			new SecuredContent(MartianLanguage.Flag3, "21601271F3CD43D8B1981DF57BD3B0F0", "")
		};

	    private static byte[] LoadOcrTask()
	    {
	        using (var stream = typeof(DbProxyController).Assembly.GetManifestResourceStream("MartianDbProxy.mars.png"))
	        {
	            Debug.Assert(stream != null);
                var buffer = new byte[10000000];
	            var read = stream.Read(buffer, 0, buffer.Length);
	            return buffer.Take(read).ToArray();
	        }
	    }

	    public static string apiKey = "F8E331A69E014C9EB1E746F431EE53FB";
	}

	public class SecuredContent
	{
        public SecuredContent(byte[] content, string securityToken, string nextLevelChallenge)
        {
            Content = content;
            SecurityToken = securityToken;
            NextLevelChallenge = nextLevelChallenge;
        }
        public SecuredContent(string content, string securityToken, string nextLevelChallenge)
            :this(Encoding.UTF8.GetBytes(content), securityToken, nextLevelChallenge)
        {
        }

		public readonly byte[] Content;

		public string GetContentAsBase64()
		{
			return Convert.ToBase64String(Content);
		}

		public readonly string SecurityToken;
		public readonly string NextLevelChallenge;
	}


	public class AccessRequest
	{
		public string SecurityToken;
		public string ApiKey;
	}

	public class AccessResponse
	{
		public string ErrorMessage;
		public string NextSecurityLevelChallenge;
		public string Content;

		public override string ToString()
		{
			var s = Encoding.UTF8.GetString(Convert.FromBase64String(Content ?? ""));
			return string.Format("ErrorMessage: {0}\nNextSecurityLevelChallenge: {1}\nContent:\n{2}", ErrorMessage, NextSecurityLevelChallenge, s);
		}
	}

	class MartianLanguage
	{
		public static readonly string BadApiKey = "bad api key".ToMars();
		public static readonly string NoAccess = "no access or no data".ToMars();
		public static readonly string ProvideValidXml = "provide valid AccessRequest in format of extraverbose martian language (xml)".ToMars();
		public static readonly string Greetings = "martian knowledge database security token required".ToMars();
		public static readonly string Flag3 = "scientists security key: mars0_314159265358979323".ToMars();
		public static readonly string Flag2 = "no content on this security level".ToMars();
        public static readonly string Flag1 = "no content on this security level".ToMars();
		public static readonly string NoApiKey = "no apikey element".ToMars();
		public static readonly string NoSecurityToken = "no securitytoken element".ToMars();
		public static readonly string WrongLanguageInApiKey = "unexpected characters in apikey. wrong language?".ToMars();
		public static readonly string WrongLanguageInSecurityToken = "unexpected characters in securitytoken. wrong language?".ToMars();

	}
}
