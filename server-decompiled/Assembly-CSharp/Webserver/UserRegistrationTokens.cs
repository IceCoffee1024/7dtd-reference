using System;
using System.Collections.Generic;

namespace Webserver
{
	// Token: 0x02001AC1 RID: 6849
	public static class UserRegistrationTokens
	{
		// Token: 0x0600CE69 RID: 52841 RVA: 0x004B2708 File Offset: 0x004B0908
		public static bool TryValidate(string _token, out UserRegistrationTokens.RegistrationData _data)
		{
			return UserRegistrationTokens.activeTokens.TryGetValue(_token, out _data) && _data.ExpiryTime > DateTime.Now;
		}

		// Token: 0x0600CE6A RID: 52842 RVA: 0x004B272C File Offset: 0x004B092C
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

		// Token: 0x04009C5D RID: 40029
		[PublicizedFrom(EAccessModifier.Private)]
		public const float tokenExpirationMinutes = 3f;

		// Token: 0x04009C5E RID: 40030
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<string, UserRegistrationTokens.RegistrationData> activeTokens = new Dictionary<string, UserRegistrationTokens.RegistrationData>();

		// Token: 0x02001AC2 RID: 6850
		public class RegistrationData
		{
			// Token: 0x0600CE6C RID: 52844 RVA: 0x004B27A0 File Offset: 0x004B09A0
			public RegistrationData(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
			{
				this.PlayerName = _playerName;
				this.ExpiryTime = DateTime.Now + TimeSpan.FromMinutes(3.0);
				this.PlatformUserId = _platformUserId;
				this.CrossPlatformUserId = _crossPlatformUserId;
			}

			// Token: 0x04009C5F RID: 40031
			public readonly string PlayerName;

			// Token: 0x04009C60 RID: 40032
			public readonly DateTime ExpiryTime;

			// Token: 0x04009C61 RID: 40033
			public readonly PlatformUserIdentifierAbs PlatformUserId;

			// Token: 0x04009C62 RID: 40034
			public readonly PlatformUserIdentifierAbs CrossPlatformUserId;
		}
	}
}
