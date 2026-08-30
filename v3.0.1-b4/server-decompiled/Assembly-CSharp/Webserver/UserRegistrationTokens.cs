using System;
using System.Collections.Generic;

namespace Webserver
{
	// Token: 0x02001A9A RID: 6810
	public static class UserRegistrationTokens
	{
		// Token: 0x0600CCE0 RID: 52448 RVA: 0x004AC314 File Offset: 0x004AA514
		public static bool TryValidate(string _token, out UserRegistrationTokens.RegistrationData _data)
		{
			return UserRegistrationTokens.activeTokens.TryGetValue(_token, out _data) && _data.ExpiryTime > DateTime.Now;
		}

		// Token: 0x0600CCE1 RID: 52449 RVA: 0x004AC338 File Offset: 0x004AA538
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

		// Token: 0x04009B2C RID: 39724
		[PublicizedFrom(EAccessModifier.Private)]
		public const float tokenExpirationMinutes = 3f;

		// Token: 0x04009B2D RID: 39725
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<string, UserRegistrationTokens.RegistrationData> activeTokens = new Dictionary<string, UserRegistrationTokens.RegistrationData>();

		// Token: 0x02001A9B RID: 6811
		public class RegistrationData
		{
			// Token: 0x0600CCE3 RID: 52451 RVA: 0x004AC3AC File Offset: 0x004AA5AC
			public RegistrationData(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
			{
				this.PlayerName = _playerName;
				this.ExpiryTime = DateTime.Now + TimeSpan.FromMinutes(3.0);
				this.PlatformUserId = _platformUserId;
				this.CrossPlatformUserId = _crossPlatformUserId;
			}

			// Token: 0x04009B2E RID: 39726
			public readonly string PlayerName;

			// Token: 0x04009B2F RID: 39727
			public readonly DateTime ExpiryTime;

			// Token: 0x04009B30 RID: 39728
			public readonly PlatformUserIdentifierAbs PlatformUserId;

			// Token: 0x04009B31 RID: 39729
			public readonly PlatformUserIdentifierAbs CrossPlatformUserId;
		}
	}
}
