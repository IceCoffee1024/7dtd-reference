using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200197B RID: 6523
	[Preserve]
	public class TrackingEntry
	{
		// Token: 0x0600C907 RID: 51463 RVA: 0x00498000 File Offset: 0x00496200
		public void AddHooks()
		{
			if (this.TrackingHelper != null)
			{
				this.TrackingHelper.AddTrackingEntry(this);
			}
			QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
			QuestEventManager.Current.BlockChange += this.Current_BlockChange;
		}

		// Token: 0x0600C908 RID: 51464 RVA: 0x00498050 File Offset: 0x00496250
		public void RemoveHooks()
		{
			if (this.TrackingHelper != null)
			{
				this.TrackingHelper.RemoveTrackingEntry(this);
			}
			QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
			NavObjectManager instance = NavObjectManager.Instance;
			for (int i = this.TrackedBlocks.Count - 1; i >= 0; i--)
			{
				instance.UnRegisterNavObject(this.TrackedBlocks[i].NavObject);
				this.TrackedBlocks.RemoveAt(i);
			}
		}

		// Token: 0x0600C909 RID: 51465 RVA: 0x004980C8 File Offset: 0x004962C8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_BlockChange(Block blockOld, Block blockNew, Vector3i blockPos)
		{
			if (blockOld.IndexName == this.blockIndexName)
			{
				for (int i = 0; i < this.TrackedBlocks.Count; i++)
				{
					if (this.TrackedBlocks[i].WorldPos == blockPos)
					{
						NavObjectManager.Instance.UnRegisterNavObject(this.TrackedBlocks[i].NavObject);
						this.TrackedBlocks.RemoveAt(i);
						return;
					}
				}
			}
		}

		// Token: 0x0600C90A RID: 51466 RVA: 0x00498140 File Offset: 0x00496340
		public void StartUpdate()
		{
			if (this.localPlayer == null)
			{
				this.localPlayer = this.Owner.Owner.Owner.Player;
			}
			for (int i = 0; i < this.TrackedBlocks.Count; i++)
			{
				this.TrackedBlocks[i].KeepAlive = false;
			}
		}

		// Token: 0x0600C90B RID: 51467 RVA: 0x004981A0 File Offset: 0x004963A0
		public void HandleTrack(Chunk c)
		{
			List<Vector3i> list;
			if (c.IndexedBlocks.TryGetValue(this.blockIndexName, out list))
			{
				foreach (Vector3i vector3i in list)
				{
					Vector3i vector3i2 = c.ToWorldPos(vector3i);
					if (!c.GetBlock(vector3i).ischild && Vector3.Distance(vector3i2, this.localPlayer.position) < this.trackDistance)
					{
						this.HandleAddTrackedBlock(vector3i2);
					}
				}
			}
		}

		// Token: 0x0600C90C RID: 51468 RVA: 0x0049823C File Offset: 0x0049643C
		public void EndUpdate()
		{
			NavObjectManager instance = NavObjectManager.Instance;
			for (int i = this.TrackedBlocks.Count - 1; i >= 0; i--)
			{
				if (!this.TrackedBlocks[i].KeepAlive)
				{
					instance.UnRegisterNavObject(this.TrackedBlocks[i].NavObject);
					this.TrackedBlocks.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600C90D RID: 51469 RVA: 0x004982A0 File Offset: 0x004964A0
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleAddTrackedBlock(Vector3i pos)
		{
			for (int i = 0; i < this.TrackedBlocks.Count; i++)
			{
				if (pos == this.TrackedBlocks[i].WorldPos)
				{
					this.TrackedBlocks[i].KeepAlive = true;
				}
			}
			this.TrackedBlocks.Add(new TrackingEntry.TrackedBlock(pos, this.navObjectName));
		}

		// Token: 0x0400974F RID: 38735
		public float trackDistance = 20f;

		// Token: 0x04009750 RID: 38736
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal localPlayer;

		// Token: 0x04009751 RID: 38737
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TrackingEntry.TrackedBlock> TrackedBlocks = new List<TrackingEntry.TrackedBlock>();

		// Token: 0x04009752 RID: 38738
		public ItemClass TrackedItem;

		// Token: 0x04009753 RID: 38739
		public BaseChallengeObjective Owner;

		// Token: 0x04009754 RID: 38740
		public ChallengeTrackingHandler TrackingHelper;

		// Token: 0x04009755 RID: 38741
		public string blockIndexName = "quest_wood";

		// Token: 0x04009756 RID: 38742
		public string navObjectName = "quest_resource";

		// Token: 0x0200197C RID: 6524
		public class TrackedBlock
		{
			// Token: 0x0600C90F RID: 51471 RVA: 0x00498339 File Offset: 0x00496539
			public TrackedBlock(Vector3i worldPos, string NavObjectName)
			{
				this.WorldPos = worldPos;
				this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
				this.KeepAlive = true;
			}

			// Token: 0x04009757 RID: 38743
			public Vector3i WorldPos;

			// Token: 0x04009758 RID: 38744
			public NavObject NavObject;

			// Token: 0x04009759 RID: 38745
			public bool KeepAlive;
		}
	}
}
