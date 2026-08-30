using System;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001C96 RID: 7318
	public class PlayerInteractionsRecorderMulti : IPlayerInteractionsRecorder
	{
		// Token: 0x0600D8E3 RID: 55523 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600D8E4 RID: 55524 RVA: 0x004DC8DE File Offset: 0x004DAADE
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

		// Token: 0x0600D8E5 RID: 55525 RVA: 0x004DC916 File Offset: 0x004DAB16
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

		// Token: 0x0600D8E6 RID: 55526 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
