using System;
using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
	// Token: 0x0200197D RID: 6525
	public class ChallengeTrackingHandler
	{
		// Token: 0x0600C910 RID: 51472 RVA: 0x00498374 File Offset: 0x00496574
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

		// Token: 0x0600C911 RID: 51473 RVA: 0x00498418 File Offset: 0x00496618
		public void AddTrackingEntry(TrackingEntry track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
			this.NeedsRefresh = true;
		}

		// Token: 0x0600C912 RID: 51474 RVA: 0x00498446 File Offset: 0x00496646
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

		// Token: 0x0600C913 RID: 51475 RVA: 0x00498484 File Offset: 0x00496684
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

		// Token: 0x0400975A RID: 38746
		public Challenge Owner;

		// Token: 0x0400975B RID: 38747
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x0400975C RID: 38748
		public List<TrackingEntry> trackingEntries = new List<TrackingEntry>();

		// Token: 0x0400975D RID: 38749
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x0400975E RID: 38750
		public float RefreshDistance = 5f;

		// Token: 0x0400975F RID: 38751
		public bool NeedsRefresh;

		// Token: 0x04009760 RID: 38752
		[PublicizedFrom(EAccessModifier.Private)]
		public bool lastInTrader;
	}
}
