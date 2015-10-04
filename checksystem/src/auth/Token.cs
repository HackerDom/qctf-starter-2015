using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using log4net;

namespace main.auth
{
	public static class Token
	{
		public static string Serialize(string token, byte[] key)
		{
			var bytes = Encoding.UTF8.GetBytes(token);
			var hmac = Hmac(bytes, 0, bytes.Length, key);
			var buffer = new byte[bytes.Length + hmac.Length];
			hmac.CopyTo(buffer, 0);
			bytes.CopyTo(buffer, hmac.Length);
			return HttpServerUtility.UrlTokenEncode(buffer);
		}

		public static string TryDeserialize(string token, byte[] key)
		{
			try
			{
				if(token == null)
					return null;
				var bytes = HttpServerUtility.UrlTokenDecode(token);
				if(bytes == null)
					return null;
				var dataLength = bytes.Length - HmacLength;
				var hmac = Hmac(bytes, HmacLength, dataLength, key);
				if(!TimingSecureEquals(bytes, 0, HmacLength, hmac))
					throw new Exception("Hmac is invalid");
				return Encoding.UTF8.GetString(bytes, HmacLength, dataLength);
			}
			catch(Exception e)
			{
				Log.Warn(string.Format("Failed to extract token from '{0}'", token), e);
				return null;
			}
		}

		private static byte[] Hmac(byte[] token, int offset, int count, byte[] key)
		{
			using(var hmac = new HMACMD5(key))
				return hmac.ComputeHash(token, offset, count);
		}

		private static bool TimingSecureEquals(byte[] value1, int offset, int count, byte[] value2)
		{
			if(value1 == null || value2 == null)
				throw new ArgumentNullException();
			if(offset + count > value1.Length)
				throw new ArgumentOutOfRangeException();
			if(count != value2.Length)
				return false;
			var result = 0;
			for(var i = 0; i < count; i++)
				result |= value1[offset + i] ^ value2[i];
			return result == 0;
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(Token));
		private const int HmacLength = 16;
	}
}