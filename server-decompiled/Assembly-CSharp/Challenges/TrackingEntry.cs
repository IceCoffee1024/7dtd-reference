using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200194A RID: 6474
	[Preserve]
	public class TrackingEntry
	{
		// Token: 0x0600C7F3 RID: 51187 RVA: 0x0049460C File Offset: 0x0049280C
		public void AddHooks()
		{
			if (this.TrackingHelper != null)
			{
				this.TrackingHelper.AddTrackingEntry(this);
			}
			QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
			QuestEventManager.Current.BlockChange += this.Current_BlockChange;
		}

		// Token: 0x0600C7F4 RID: 51188 RVA: 0x0049465C File Offset: 0x0049285C
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

		// Token: 0x0600C7F5 RID: 51189 RVA: 0x004946D4 File Offset: 0x004928D4
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

		// Token: 0x0600C7F6 RID: 51190 RVA: 0x0049474C File Offset: 0x0049294C
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

		// Token: 0x0600C7F7 RID: 51191 RVA: 0x004947AC File Offset: 0x004929AC
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

		// Token: 0x0600C7F8 RID: 51192 RVA: 0x00494848 File Offset: 0x00492A48
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

		// Token: 0x0600C7F9 RID: 51193 RVA: 0x004948AC File Offset: 0x00492AAC
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

		// Token: 0x04009666 RID: 38502
		public float trackDistance = 20f;

		// Token: 0x04009667 RID: 38503
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal localPlayer;

		// Token: 0x04009668 RID: 38504
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TrackingEntry.TrackedBlock> TrackedBlocks = new List<TrackingEntry.TrackedBlock>();

		// Token: 0x04009669 RID: 38505
		public ItemClass TrackedItem;

		// Token: 0x0400966A RID: 38506
		public BaseChallengeObjective Owner;

		// Token: 0x0400966B RID: 38507
		public ChallengeTrackingHandler TrackingHelper;

		// Token: 0x0400966C RID: 38508
		public string blockIndexName = "quest_wood";

		// Token: 0x0400966D RID: 38509
		public string navObjectName = "quest_resource";

		// Token: 0x0200194B RID: 6475
		public class TrackedBlock
		{
			// Token: 0x0600C7FB RID: 51195 RVA: 0x00494945 File Offset: 0x00492B45
			public TrackedBlock(Vector3i worldPos, string NavObjectName)
			{
				this.WorldPos = worldPos;
				this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
				this.KeepAlive = true;
			}

			// Token: 0x0400966E RID: 38510
			public Vector3i WorldPos;

			// Token: 0x0400966F RID: 38511
			public NavObject NavObject;

			// Token: 0x04009670 RID: 38512
			public bool KeepAlive;
		}
	}
}
