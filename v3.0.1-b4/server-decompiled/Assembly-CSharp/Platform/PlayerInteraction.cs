using System;

namespace Platform
{
	// Token: 0x02001B68 RID: 7016
	public struct PlayerInteraction
	{
		// Token: 0x0600D166 RID: 53606 RVA: 0x004C0F3D File Offset: 0x004BF13D
		public PlayerInteraction(PlayerData _playerData, PlayerInteractionType _type)
		{
			this.PlayerData = _playerData;
			this.Type = _type;
		}

		// Token: 0x04009F9D RID: 40861
		public PlayerData PlayerData;

		// Token: 0x04009F9E RID: 40862
		public PlayerInteractionType Type;
	}
}
