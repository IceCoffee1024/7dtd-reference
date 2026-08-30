using System;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CBD RID: 7357
	public class PlayerInteractionsRecorderMulti : IPlayerInteractionsRecorder
	{
		// Token: 0x0600DA7B RID: 55931 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform owner)
		{
		}

		// Token: 0x0600DA7C RID: 55932 RVA: 0x004E2EDA File Offset: 0x004E10DA
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

		// Token: 0x0600DA7D RID: 55933 RVA: 0x004E2F12 File Offset: 0x004E1112
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

		// Token: 0x0600DA7E RID: 55934 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
