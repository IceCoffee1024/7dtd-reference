using System;

namespace Platform
{
	// Token: 0x02001B8F RID: 7055
	public struct PlayerInteraction
	{
		// Token: 0x0600D2F1 RID: 54001 RVA: 0x004C7331 File Offset: 0x004C5531
		public PlayerInteraction(PlayerData _playerData, PlayerInteractionType _type)
		{
			this.PlayerData = _playerData;
			this.Type = _type;
		}

		// Token: 0x0400A0CE RID: 41166
		public PlayerData PlayerData;

		// Token: 0x0400A0CF RID: 41167
		public PlayerInteractionType Type;
	}
}
