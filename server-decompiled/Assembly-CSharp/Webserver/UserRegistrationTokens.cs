using System;
using System.Collections.Generic;

namespace Webserver
{
	// Token: 0x02001AED RID: 6893
	public static class UserRegistrationTokens
	{
		// Token: 0x0600CFC1 RID: 53185 RVA: 0x004B6BD4 File Offset: 0x004B4DD4
		public static bool TryValidate(string _token, out UserRegistrationTokens.RegistrationData _data)
		{
			return UserRegistrationTokens.activeTokens.TryGetValue(_token, out _data) && _data.ExpiryTime > DateTime.Now;
		}

		// Token: 0x0600CFC2 RID: 53186 RVA: 0x004B6BF8 File Offset: 0x004B4DF8
		public static string CreateToken(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
		{
			string text = Utils.GenerateGuid();
			DateTime currentTime = DateTime.Now;
			UserRegistrationTokens.activeTokens.RemoveAll<string, UserRegistrationTokens.RegistrationData>(delegate(UserRegistrationTokens.RegistrationData _data)
			{
				if (!(_data.ExpiryTime < currentTime) && !_platformUserId.Equals(_data.PlatformUserId))
				{
					PlatformUserIdentifierAbs crossPlatformUserId = _crossPlatformUserId;
					return crossPlatformUserId != null && crossPlatformUserId.Equals(_data.CrossPlatformUserId);
				}
				return true;
			});
			UserRegistrationTokens.RegistrationData registrationData = new UserRegistrationTokens.RegistrationData(_playerName, _platformUserId, _crossPlatformUserId);
			UserRegistrationTokens.activeTokens[text] = registrationData;
			return text;
		}

		// Token: 0x04009D23 RID: 40227
		[PublicizedFrom(EAccessModifier.Private)]
		public const float tokenExpirationMinutes = 3f;

		// Token: 0x04009D24 RID: 40228
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<string, UserRegistrationTokens.RegistrationData> activeTokens = new Dictionary<string, UserRegistrationTokens.RegistrationData>();

		// Token: 0x02001AEE RID: 6894
		public class RegistrationData
		{
			// Token: 0x0600CFC4 RID: 53188 RVA: 0x004B6C6C File Offset: 0x004B4E6C
			public RegistrationData(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
			{
				this.PlayerName = _playerName;
				this.ExpiryTime = DateTime.Now + TimeSpan.FromMinutes(3.0);
				this.PlatformUserId = _platformUserId;
				this.CrossPlatformUserId = _crossPlatformUserId;
			}

			// Token: 0x04009D25 RID: 40229
			public readonly string PlayerName;

			// Token: 0x04009D26 RID: 40230
			public readonly DateTime ExpiryTime;

			// Token: 0x04009D27 RID: 40231
			public readonly PlatformUserIdentifierAbs PlatformUserId;

			// Token: 0x04009D28 RID: 40232
			public readonly PlatformUserIdentifierAbs CrossPlatformUserId;
		}
	}
}
