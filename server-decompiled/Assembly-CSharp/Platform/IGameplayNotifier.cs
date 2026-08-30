using System;

namespace Platform
{
	// Token: 0x02001B66 RID: 7014
	public interface IGameplayNotifier
	{
		// Token: 0x0600D1FD RID: 53757
		void Init(IPlatform platform);

		// Token: 0x0600D1FE RID: 53758
		void GameplayStart(bool isOnlineMultiplayer, bool isCrossplayEnabled);

		// Token: 0x0600D1FF RID: 53759
		void EndOnlineMultiplayer();

		// Token: 0x0600D200 RID: 53760
		void GameplayEnd();
	}
}
