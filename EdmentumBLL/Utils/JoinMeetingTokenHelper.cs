using System.Security.Cryptography;
using System.Text;

namespace EdmentumBLL.Utils
{
    public static class JoinMeetingTokenHelper
    {
        private const string DOT = ".";

        public static string GenerateJoinToken(Dictionary<string, string> parameters)
        {
            // Remove null or empty values
            parameters = parameters.Where(p => !string.IsNullOrEmpty(p.Value)).ToDictionary(p => p.Key, p => p.Value);
            // Sort the parameters
            var sortedParameters = parameters.OrderBy(p => p.Value).Select(p => p.Value);
            // Merge sorted parameters into a single string
            string mergeString = string.Join("", sortedParameters);
            // Generate the signature
            string signature = AlgorithmHelper.Hmac256(mergeString, AlgorithmHelper.Sha256(Constants.SecretKey));
            // Add the signature parameter
            parameters.Add("signature", signature);
            // Convert parameters to JSON
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
            // Create the token
            string token = Constants.AccessKey + DOT + AesEncrypt(json, Constants.SecretKey, Constants.AccessKey);
            
            return token;
        }

        private static string AesEncrypt(string plainText, string secret, string ivStr)
        {
            try
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(secret);
                    aesAlg.IV = Encoding.UTF8.GetBytes(ivStr);
                    using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                    {
                        byte[] cipherText = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);
                        return AlgorithmHelper.Base64UrlEncode(cipherText);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"AES encrypt fail: {e.Message}", e);
            }
        }

        
    }
}
