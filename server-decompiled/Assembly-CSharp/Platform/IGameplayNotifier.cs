using System;

namespace Platform
{
	// Token: 0x02001B98 RID: 7064
	public interface IGameplayNotifier
	{
		// Token: 0x0600D375 RID: 54133
		void Init(IPlatform platform);

		// Token: 0x0600D376 RID: 54134
		void GameplayStart(bool isOnlineMultiplayer, bool isCrossplayEnabled);

		// Token: 0x0600D377 RID: 54135
		void EndOnlineMultiplayer();

		// Token: 0x0600D378 RID: 54136
		void GameplayEnd();
	}
}
