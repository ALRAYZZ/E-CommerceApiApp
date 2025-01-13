using Microsoft.AspNetCore.Identity;

namespace E_CommerceApi.Services
{
	public class PasswordResetService
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<PasswordResetService> _logger;

		public PasswordResetService(UserManager<IdentityUser> userManager, ILogger<PasswordResetService> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}

		public async Task<bool> ResetPasswordAsync(string username, string newPassword)
		{
			var user = await _userManager.FindByNameAsync(username);
			if (user == null)
			{
				_logger.LogError($"User {username} not found");
				return false;
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					_logger.LogError($"Error resetting password: {error.Description}");
				}
				return false;
			}

			_logger.LogInformation($"Password reset for user {username}");
			return true;
		}
	}
}
