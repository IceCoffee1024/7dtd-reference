using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
	// Token: 0x020018A2 RID: 6306
	public class TrackingHandler
	{
		// Token: 0x0600C2AE RID: 49838 RVA: 0x004805BC File Offset: 0x0047E7BC
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

		// Token: 0x0600C2AF RID: 49839 RVA: 0x00480655 File Offset: 0x0047E855
		public void AddTrackingEntry(ObjectiveModifierTrackBlocks track)
		{
			if (!this.trackingEntries.Contains(track))
			{
				this.trackingEntries.Add(track);
				this.NeedsRefresh = true;
			}
			QuestEventManager.Current.AddTrackerToBeUpdated(this);
		}

		// Token: 0x0600C2B0 RID: 49840 RVA: 0x00480683 File Offset: 0x0047E883
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

		// Token: 0x0600C2B1 RID: 49841 RVA: 0x004806C0 File Offset: 0x0047E8C0
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

		// Token: 0x040093B1 RID: 37809
		public int QuestCode;

		// Token: 0x040093B2 RID: 37810
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x040093B3 RID: 37811
		public List<ObjectiveModifierTrackBlocks> trackingEntries = new List<ObjectiveModifierTrackBlocks>();

		// Token: 0x040093B4 RID: 37812
		[PublicizedFrom(EAccessModifier.Protected)]
		public Vector3 LastCheckedPosition = new Vector3(0f, 9999f, 0f);

		// Token: 0x040093B5 RID: 37813
		public float RefreshDistance = 5f;

		// Token: 0x040093B6 RID: 37814
		public bool NeedsRefresh;
	}
}
