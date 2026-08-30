using System;

namespace Platform
{
	// Token: 0x02001B3F RID: 6975
	public interface IGameplayNotifier
	{
		// Token: 0x0600D072 RID: 53362
		void Init(IPlatform platform);

		// Token: 0x0600D073 RID: 53363
		void GameplayStart(bool isOnlineMultiplayer, bool isCrossplayEnabled);

		// Token: 0x0600D074 RID: 53364
		void EndOnlineMultiplayer();

		// Token: 0x0600D075 RID: 53365
		void GameplayEnd();
	}
}
