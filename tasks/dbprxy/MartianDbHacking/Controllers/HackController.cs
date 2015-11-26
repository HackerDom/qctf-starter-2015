using System.Collections.Generic;
using System.Web.Http;

namespace MartianDbHacking.Controllers
{
	public class HackController : ApiController
	{
		public string Get()
		{
			return "provide challenge from MartianDbProxy";
		}

		public object Get(string challenge)
		{
			if (string.IsNullOrEmpty(challenge))
				return "provide challenge from MartianDbProxy";
			if (!hacks.ContainsKey(challenge.ToUpper())) return "can't hack this challenge :(";
			return new HackRecord { ApiKey = "F8E331A69E014C9EB1E746F431EE53FB", SecurityToken = hacks[challenge] };
		}

		Dictionary<string, string> hacks = new Dictionary<string, string>
		{
			{"0C0D57E66FE7401C82B97338658421BD", "2290A406DDE94918A7A29BB6D254F64D"},
			{"AAF893E1D631430799F7F0FF27308162", "FFD737F3E0894D1296066F5F8BD262C4"},
			{"2DD4F4C12C3A43BDA3AD5F707725DA74", "ADEB04680ADC4E4FAB59C158A89ED420"},
			{"BC7017B8A3E4466D98B80F762D47CB15", "21601271F3CD43D8B1981DF57BD3B0F0"},
		};
	}

	public class HackRecord
	{
		public string ApiKey;
		public string SecurityToken;
	}
}
