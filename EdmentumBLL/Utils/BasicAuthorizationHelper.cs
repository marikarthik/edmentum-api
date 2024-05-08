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
				string secret = AlgorithmHelper.Hmac256(access, AlgorithmHelper.Sha256(secretKey));
				string concatenated = access + SEPARATOR + secret;
				byte[] bytes = Encoding.GetEncoding(CHARSET).GetBytes(concatenated);
				return AlgorithmHelper.Base64UrlEncode(bytes);
			}
		}
	}
}