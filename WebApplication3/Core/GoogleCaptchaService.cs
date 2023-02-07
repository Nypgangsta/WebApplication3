using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace WebApplication3.Core
{
	public class GoogleCaptchaService
	{
		public async Task<bool> VerifyToken(string token)
		{
			try
			{
				var url = $"https://www.google.com/recaptcha/api/siteverify?secret=6LfGsFkkAAAAAIGr5LL0uG9qhFQmk83SfexD3mbG&response={token}";
				using (var client = new HttpClient())
				{
					var httpResult = await client.GetAsync(url);
					if (httpResult.StatusCode != HttpStatusCode.OK)
					{
						return false;
					}
					var responseString = await httpResult.Content.ReadAsStringAsync();
					var googleResult = JsonConvert.DeserializeObject<GoogleRecaptchaResponse>(responseString);
					return googleResult.success && googleResult.score >= 0.5;
				}

			}
			catch(Exception ex)
			{
				return false;
			}
		}
	}
}
