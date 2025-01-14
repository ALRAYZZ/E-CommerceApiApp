using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace E_CommerceClient.Services
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _localStorage.GetItemAsync<string>("authToken");

			if (string.IsNullOrEmpty(token))
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var claims = ParseClaimsFromJwt(token);
			var identity = new ClaimsIdentity(claims, "jwt");
			var user = new ClaimsPrincipal(identity);

			return new AuthenticationState(user);
		}

		public async Task MarkUserAsAuthenticated(string token)
		{
			await _localStorage.SetItemAsync("authToken", token);
			var claims = ParseClaimsFromJwt(token);
			var identity = new ClaimsIdentity(claims, "jwt");
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}


		public async Task MarkUserAsLoggedOut()
		{
			await _localStorage.RemoveItemAsync("authToken");
			var identity = new ClaimsIdentity();
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			var claims = new List<Claim>();
			var payload = jwt.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			keyValuePairs.TryGetValue(ClaimTypes.NameIdentifier, out object nameIdentifier);
			if (nameIdentifier != null)
			{
				claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier.ToString()));
			}

			return claims;
		}

		private byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}
	}
}