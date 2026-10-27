using System;
using System.Collections.Generic;

namespace Webserver
{
	// Token: 0x02001B1E RID: 6942
	public static class UserRegistrationTokens
	{
		// Token: 0x0600D0D5 RID: 53461 RVA: 0x004BA6D0 File Offset: 0x004B88D0
		public static bool TryValidate(string _token, out UserRegistrationTokens.RegistrationData _data)
		{
			return UserRegistrationTokens.activeTokens.TryGetValue(_token, out _data) && _data.ExpiryTime > DateTime.Now;
		}

		// Token: 0x0600D0D6 RID: 53462 RVA: 0x004BA6F4 File Offset: 0x004B88F4
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

		// Token: 0x04009E0C RID: 40460
		[PublicizedFrom(EAccessModifier.Private)]
		public const float tokenExpirationMinutes = 3f;

		// Token: 0x04009E0D RID: 40461
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<string, UserRegistrationTokens.RegistrationData> activeTokens = new Dictionary<string, UserRegistrationTokens.RegistrationData>();

		// Token: 0x02001B1F RID: 6943
		public class RegistrationData
		{
			// Token: 0x0600D0D8 RID: 53464 RVA: 0x004BA768 File Offset: 0x004B8968
			public RegistrationData(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
			{
				this.PlayerName = _playerName;
				this.ExpiryTime = DateTime.Now + TimeSpan.FromMinutes(3.0);
				this.PlatformUserId = _platformUserId;
				this.CrossPlatformUserId = _crossPlatformUserId;
			}

			// Token: 0x04009E0E RID: 40462
			public readonly string PlayerName;

			// Token: 0x04009E0F RID: 40463
			public readonly DateTime ExpiryTime;

			// Token: 0x04009E10 RID: 40464
			public readonly PlatformUserIdentifierAbs PlatformUserId;

			// Token: 0x04009E11 RID: 40465
			public readonly PlatformUserIdentifierAbs CrossPlatformUserId;
		}
	}
}
