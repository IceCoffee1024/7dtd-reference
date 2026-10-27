using System;

namespace Platform
{
	// Token: 0x02001BF2 RID: 7154
	public struct PlayerInteraction
	{
		// Token: 0x0600D57D RID: 54653 RVA: 0x004CF941 File Offset: 0x004CDB41
		public PlayerInteraction(PlayerData _playerData, PlayerInteractionType _type)
		{
			this.PlayerData = _playerData;
			this.Type = _type;
		}

		// Token: 0x0400A28C RID: 41612
		public PlayerData PlayerData;

		// Token: 0x0400A28D RID: 41613
		public PlayerInteractionType Type;
	}
}
