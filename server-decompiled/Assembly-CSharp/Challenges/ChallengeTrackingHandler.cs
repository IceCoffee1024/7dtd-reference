using System;
using System.Collections.Generic;
using UnityEngine;

namespace Challenges
{
	// Token: 0x02001920 RID: 6432
	public class ChallengeTrackingHandler
	{
		// Token: 0x0600C6A4 RID: 50852 RVA: 0x00490494 File Offset: 0x0048E694
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

		// Token: 0x0600C6A5 RID: 50853 RVA: 0x00490538 File Offset: 0x0048E738
		public void AddTrackingEntry(TrackingEntry track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
			this.NeedsRefresh = true;
		}

		// Token: 0x0600C6A6 RID: 50854 RVA: 0x00490566 File Offset: 0x0048E766
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

		// Token: 0x0600C6A7 RID: 50855 RVA: 0x004905A4 File Offset: 0x0048E7A4
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

		// Token: 0x040095AB RID: 38315
		public Challenge Owner;

		// Token: 0x040095AC RID: 38316
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x040095AD RID: 38317
		public List<TrackingEntry> trackingEntries = new List<TrackingEntry>();

		// Token: 0x040095AE RID: 38318
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x040095AF RID: 38319
		public float RefreshDistance = 5f;

		// Token: 0x040095B0 RID: 38320
		public bool NeedsRefresh;

		// Token: 0x040095B1 RID: 38321
		[PublicizedFrom(EAccessModifier.Private)]
		public bool lastInTrader;
	}
}
