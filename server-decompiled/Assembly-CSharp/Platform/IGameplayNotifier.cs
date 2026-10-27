using System;

namespace Platform
{
	// Token: 0x02001BC9 RID: 7113
	public interface IGameplayNotifier
	{
		// Token: 0x0600D489 RID: 54409
		void Init(IPlatform platform);

		// Token: 0x0600D48A RID: 54410
		void GameplayStart(bool isOnlineMultiplayer, bool isCrossplayEnabled);

		// Token: 0x0600D48B RID: 54411
		void EndOnlineMultiplayer();

		// Token: 0x0600D48C RID: 54412
		void GameplayEnd();
	}
}
