using System;
using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
	// Token: 0x0200194C RID: 6476
	public class ChallengeTrackingHandler
	{
		// Token: 0x0600C7FC RID: 51196 RVA: 0x00494980 File Offset: 0x00492B80
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

		// Token: 0x0600C7FD RID: 51197 RVA: 0x00494A24 File Offset: 0x00492C24
		public void AddTrackingEntry(TrackingEntry track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
			this.NeedsRefresh = true;
		}

		// Token: 0x0600C7FE RID: 51198 RVA: 0x00494A52 File Offset: 0x00492C52
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

		// Token: 0x0600C7FF RID: 51199 RVA: 0x00494A90 File Offset: 0x00492C90
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

		// Token: 0x04009671 RID: 38513
		public Challenge Owner;

		// Token: 0x04009672 RID: 38514
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x04009673 RID: 38515
		public List<TrackingEntry> trackingEntries = new List<TrackingEntry>();

		// Token: 0x04009674 RID: 38516
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x04009675 RID: 38517
		public float RefreshDistance = 5f;

		// Token: 0x04009676 RID: 38518
		public bool NeedsRefresh;

		// Token: 0x04009677 RID: 38519
		[PublicizedFrom(EAccessModifier.Private)]
		public bool lastInTrader;
	}
}
