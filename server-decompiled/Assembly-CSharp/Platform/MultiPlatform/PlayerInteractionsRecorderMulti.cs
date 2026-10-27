using System;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001D20 RID: 7456
	public class PlayerInteractionsRecorderMulti : IPlayerInteractionsRecorder
	{
		// Token: 0x0600DD09 RID: 56585 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600DD0A RID: 56586 RVA: 0x004EB5C2 File Offset: 0x004E97C2
		public void RecordPlayerInteraction(PlayerInteraction interaction)
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform != null)
			{
				IPlayerInteractionsRecorder playerInteractionsRecorder = nativePlatform.PlayerInteractionsRecorder;
				if (playerInteractionsRecorder != null)
				{
					playerInteractionsRecorder.RecordPlayerInteraction(interaction);
				}
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform == null)
			{
				return;
			}
			IPlayerInteractionsRecorder playerInteractionsRecorder2 = crossplatformPlatform.PlayerInteractionsRecorder;
			if (playerInteractionsRecorder2 == null)
			{
				return;
			}
			playerInteractionsRecorder2.RecordPlayerInteraction(interaction);
		}

		// Token: 0x0600DD0B RID: 56587 RVA: 0x004EB5FA File Offset: 0x004E97FA
		public void RecordPlayerInteractions(IEnumerable<PlayerInteraction> interactions)
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (nativePlatform != null)
			{
				IPlayerInteractionsRecorder playerInteractionsRecorder = nativePlatform.PlayerInteractionsRecorder;
				if (playerInteractionsRecorder != null)
				{
					playerInteractionsRecorder.RecordPlayerInteractions(interactions);
				}
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform == null)
			{
				return;
			}
			IPlayerInteractionsRecorder playerInteractionsRecorder2 = crossplatformPlatform.PlayerInteractionsRecorder;
			if (playerInteractionsRecorder2 == null)
			{
				return;
			}
			playerInteractionsRecorder2.RecordPlayerInteractions(interactions);
		}

		// Token: 0x0600DD0C RID: 56588 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
