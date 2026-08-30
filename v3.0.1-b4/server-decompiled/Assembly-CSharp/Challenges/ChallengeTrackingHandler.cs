using System;
using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
	// Token: 0x020018FA RID: 6394
	public class ChallengeTrackingHandler
	{
		// Token: 0x0600C51D RID: 50461 RVA: 0x0048A148 File Offset: 0x00488348
		public bool Update(float deltaTime)
		{
			if (this.LocalPlayer == null)
			{
				return true;
			}
			if (this.Owner == null || !this.Owner.IsActive)
			{
				return false;
			}
			if (this.LocalPlayer.IsInTrader != this.lastInTrader)
			{
				this.lastInTrader = this.LocalPlayer.IsInTrader;
				this.NeedsRefresh = true;
			}
			if (Vector3.Distance(this.LastCheckedPosition, this.LocalPlayer.position) > this.RefreshDistance || this.NeedsRefresh)
			{
				this.LastCheckedPosition = this.LocalPlayer.position;
				this.HandleTracking();
				this.NeedsRefresh = false;
			}
			return true;
		}

		// Token: 0x0600C51E RID: 50462 RVA: 0x0048A1EC File Offset: 0x004883EC
		public void AddTrackingEntry(TrackingEntry track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
			this.NeedsRefresh = true;
		}

		// Token: 0x0600C51F RID: 50463 RVA: 0x0048A21A File Offset: 0x0048841A
		public void RemoveTrackingEntry(TrackingEntry track)
		{
			if (this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Remove(track);
				this.NeedsRefresh = true;
			}
			if (this.trackingEntries.Count == 0)
			{
				QuestEventManager.Current.RemoveTrackerToBeUpdated(this);
			}
		}

		// Token: 0x0600C520 RID: 50464 RVA: 0x0048A258 File Offset: 0x00488458
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleTracking()
		{
			NavObjectManager instance = NavObjectManager.Instance;
			List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
			for (int i = 0; i < this.trackingEntries.Count; i++)
			{
				this.trackingEntries[i].StartUpdate();
			}
			if (!this.LocalPlayer.IsInTrader)
			{
				foreach (Chunk chunk in chunkArrayCopySync)
				{
					for (int j = 0; j < this.trackingEntries.Count; j++)
					{
						this.trackingEntries[j].HandleTrack(chunk);
					}
				}
			}
			for (int k = 0; k < this.trackingEntries.Count; k++)
			{
				this.trackingEntries[k].EndUpdate();
			}
		}

		// Token: 0x04009482 RID: 38018
		public Challenge Owner;

		// Token: 0x04009483 RID: 38019
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x04009484 RID: 38020
		public List<TrackingEntry> trackingEntries = new List<TrackingEntry>();

		// Token: 0x04009485 RID: 38021
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x04009486 RID: 38022
		public float RefreshDistance = 5f;

		// Token: 0x04009487 RID: 38023
		public bool NeedsRefresh;

		// Token: 0x04009488 RID: 38024
		[PublicizedFrom(EAccessModifier.Private)]
		public bool lastInTrader;
	}
}
