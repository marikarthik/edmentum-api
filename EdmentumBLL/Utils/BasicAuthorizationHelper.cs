using System.Security.Cryptography;
using System.Text;

namespace EdmentumBLL.Utils
{
	public static class BasicAuthorizationHelper
	{
		private const string DOT = ".";
		private const string SEPARATOR = ":";
		private const string CHARSET = "UTF-8";

		public static string GenerateAuthorization(string accessKey, string secretKey)
		{
			long expireAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 50000 * 1000;
			return new BasicAuthorization(accessKey, secretKey, expireAt).Build();
		}

		private class BasicAuthorization
		{
			private readonly string accessKey;
			private readonly string secretKey;
			private long expireAt;

			public BasicAuthorization(string accessKey, string secretKey, long expireAt)
			{
				this.accessKey = accessKey;
				this.secretKey = secretKey;
				this.expireAt = expireAt;
			}

			public string Build()
			{
				string access = accessKey + DOT + expireAt;
				string secret = Hmac256(access, Sha256(secretKey));
				string concatenated = access + SEPARATOR + secret;
				byte[] bytes = Encoding.GetEncoding(CHARSET).GetBytes(concatenated);
				return Base64UrlEncode(bytes);
			}

			public static string Hmac256(string content, string secretKey)
			{
				byte[] keyBytes = HexToBytes(secretKey);
				using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
				{
					byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(content));
					return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
				}
			}

			private static byte[] HexToBytes(string hex)
			{
				byte[] bytes = new byte[hex.Length / 2];
				for (int i = 0; i < hex.Length; i += 2)
				{
					bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
				}
				return bytes;
			}

			private static string Sha256(string input)
			{
				using (SHA256 sha256 = SHA256.Create())
				{
					byte[] hashBytes = sha256.ComputeHash(Encoding.GetEncoding(CHARSET).GetBytes(input));
					return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
				}
			}

			private static string Base64UrlEncode(byte[] input)
			{
				string base64 = Convert.ToBase64String(input);
				return base64.Replace('+', '-').Replace('/', '_');
			}
		}
	}
}