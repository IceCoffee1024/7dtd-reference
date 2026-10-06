using System;

namespace Platform
{
	// Token: 0x02001BC1 RID: 7105
	public struct PlayerInteraction
	{
		// Token: 0x0600D469 RID: 54377 RVA: 0x004CBE19 File Offset: 0x004CA019
		public PlayerInteraction(PlayerData _playerData, PlayerInteractionType _type)
		{
			this.PlayerData = _playerData;
			this.Type = _type;
		}

		// Token: 0x0400A1A3 RID: 41379
		public PlayerData PlayerData;

		// Token: 0x0400A1A4 RID: 41380
		public PlayerInteractionType Type;
	}
}
