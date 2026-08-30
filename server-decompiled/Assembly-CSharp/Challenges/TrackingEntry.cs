using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018F8 RID: 6392
	[Preserve]
	public class TrackingEntry
	{
		// Token: 0x0600C514 RID: 50452 RVA: 0x00489DD4 File Offset: 0x00487FD4
		public void AddHooks()
		{
			if (this.TrackingHelper != null)
			{
				this.TrackingHelper.AddTrackingEntry(this);
			}
			QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
			QuestEventManager.Current.BlockChange += this.Current_BlockChange;
		}

		// Token: 0x0600C515 RID: 50453 RVA: 0x00489E24 File Offset: 0x00488024
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

		// Token: 0x0600C516 RID: 50454 RVA: 0x00489E9C File Offset: 0x0048809C
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

		// Token: 0x0600C517 RID: 50455 RVA: 0x00489F14 File Offset: 0x00488114
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

		// Token: 0x0600C518 RID: 50456 RVA: 0x00489F74 File Offset: 0x00488174
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

		// Token: 0x0600C519 RID: 50457 RVA: 0x0048A010 File Offset: 0x00488210
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

		// Token: 0x0600C51A RID: 50458 RVA: 0x0048A074 File Offset: 0x00488274
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

		// Token: 0x04009477 RID: 38007
		public float trackDistance = 20f;

		// Token: 0x04009478 RID: 38008
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal localPlayer;

		// Token: 0x04009479 RID: 38009
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TrackingEntry.TrackedBlock> TrackedBlocks = new List<TrackingEntry.TrackedBlock>();

		// Token: 0x0400947A RID: 38010
		public ItemClass TrackedItem;

		// Token: 0x0400947B RID: 38011
		public BaseChallengeObjective Owner;

		// Token: 0x0400947C RID: 38012
		public ChallengeTrackingHandler TrackingHelper;

		// Token: 0x0400947D RID: 38013
		public string blockIndexName = "quest_wood";

		// Token: 0x0400947E RID: 38014
		public string navObjectName = "quest_resource";

		// Token: 0x020018F9 RID: 6393
		public class TrackedBlock
		{
			// Token: 0x0600C51C RID: 50460 RVA: 0x0048A10D File Offset: 0x0048830D
			public TrackedBlock(Vector3i worldPos, string NavObjectName)
			{
				this.WorldPos = worldPos;
				this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
				this.KeepAlive = true;
			}

			// Token: 0x0400947F RID: 38015
			public Vector3i WorldPos;

			// Token: 0x04009480 RID: 38016
			public NavObject NavObject;

			// Token: 0x04009481 RID: 38017
			public bool KeepAlive;
		}
	}
}
