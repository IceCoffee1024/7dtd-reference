using System;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CEF RID: 7407
	public class PlayerInteractionsRecorderMulti : IPlayerInteractionsRecorder
	{
		// Token: 0x0600DBF5 RID: 56309 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600DBF6 RID: 56310 RVA: 0x004E7A22 File Offset: 0x004E5C22
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

		// Token: 0x0600DBF7 RID: 56311 RVA: 0x004E7A5A File Offset: 0x004E5C5A
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

		// Token: 0x0600DBF8 RID: 56312 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
