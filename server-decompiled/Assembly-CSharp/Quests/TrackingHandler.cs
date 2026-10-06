using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
	// Token: 0x020018CE RID: 6350
	public class TrackingHandler
	{
		// Token: 0x0600C406 RID: 50182 RVA: 0x00484AF8 File Offset: 0x00482CF8
		public bool Update(float deltaTime)
		{
			if (this.LocalPlayer == null)
			{
				return true;
			}
			Quest quest = this.LocalPlayer.QuestJournal.FindActiveQuest(this.QuestCode);
			if (quest == null || quest.OwnerJournal == null || quest.OwnerJournal.OwnerPlayer == null)
			{
				return false;
			}
			if (Vector3.Distance(this.LastCheckedPosition, this.LocalPlayer.position) > this.RefreshDistance || this.NeedsRefresh)
			{
				this.LastCheckedPosition = this.LocalPlayer.position;
				this.HandleTracking();
				this.NeedsRefresh = false;
			}
			return true;
		}

		// Token: 0x0600C407 RID: 50183 RVA: 0x00484B91 File Offset: 0x00482D91
		public void AddTrackingEntry(ObjectiveModifierTrackBlocks track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
				this.NeedsRefresh = true;
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
		}

		// Token: 0x0600C408 RID: 50184 RVA: 0x00484BBF File Offset: 0x00482DBF
		public void RemoveTrackingEntry(ObjectiveModifierTrackBlocks track)
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

		// Token: 0x0600C409 RID: 50185 RVA: 0x00484BFC File Offset: 0x00482DFC
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleTracking()
		{
			NavObjectManager instance = NavObjectManager.Instance;
			List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
			for (int i = 0; i < this.trackingEntries.Count; i++)
			{
				this.trackingEntries[i].StartUpdate();
			}
			foreach (Chunk chunk in chunkArrayCopySync)
			{
				for (int j = 0; j < this.trackingEntries.Count; j++)
				{
					this.trackingEntries[j].HandleTrack(chunk);
				}
			}
			for (int k = 0; k < this.trackingEntries.Count; k++)
			{
				this.trackingEntries[k].EndUpdate();
			}
		}

		// Token: 0x04009477 RID: 38007
		public int QuestCode;

		// Token: 0x04009478 RID: 38008
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x04009479 RID: 38009
		public List<ObjectiveModifierTrackBlocks> trackingEntries = new List<ObjectiveModifierTrackBlocks>();

		// Token: 0x0400947A RID: 38010
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x0400947B RID: 38011
		public float RefreshDistance = 5f;

		// Token: 0x0400947C RID: 38012
		public bool NeedsRefresh;
	}
}
