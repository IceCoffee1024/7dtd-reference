using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
	// Token: 0x020018FF RID: 6399
	public class TrackingHandler
	{
		// Token: 0x0600C51A RID: 50458 RVA: 0x004884A4 File Offset: 0x004866A4
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

		// Token: 0x0600C51B RID: 50459 RVA: 0x0048853D File Offset: 0x0048673D
		public void AddTrackingEntry(ObjectiveModifierTrackBlocks track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
				this.NeedsRefresh = true;
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
		}

		// Token: 0x0600C51C RID: 50460 RVA: 0x0048856B File Offset: 0x0048676B
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

		// Token: 0x0600C51D RID: 50461 RVA: 0x004885A8 File Offset: 0x004867A8
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

		// Token: 0x04009560 RID: 38240
		public int QuestCode;

		// Token: 0x04009561 RID: 38241
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x04009562 RID: 38242
		public List<ObjectiveModifierTrackBlocks> trackingEntries = new List<ObjectiveModifierTrackBlocks>();

		// Token: 0x04009563 RID: 38243
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x04009564 RID: 38244
		public float RefreshDistance = 5f;

		// Token: 0x04009565 RID: 38245
		public bool NeedsRefresh;
	}
}
