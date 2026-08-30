using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200191E RID: 6430
	[Preserve]
	public class TrackingEntry
	{
		// Token: 0x0600C69B RID: 50843 RVA: 0x00490120 File Offset: 0x0048E320
		public void AddHooks()
		{
			if (this.TrackingHelper != null)
			{
				this.TrackingHelper.AddTrackingEntry(this);
			}
			QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
			QuestEventManager.Current.BlockChange += this.Current_BlockChange;
		}

		// Token: 0x0600C69C RID: 50844 RVA: 0x00490170 File Offset: 0x0048E370
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

		// Token: 0x0600C69D RID: 50845 RVA: 0x004901E8 File Offset: 0x0048E3E8
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

		// Token: 0x0600C69E RID: 50846 RVA: 0x00490260 File Offset: 0x0048E460
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

		// Token: 0x0600C69F RID: 50847 RVA: 0x004902C0 File Offset: 0x0048E4C0
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

		// Token: 0x0600C6A0 RID: 50848 RVA: 0x0049035C File Offset: 0x0048E55C
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

		// Token: 0x0600C6A1 RID: 50849 RVA: 0x004903C0 File Offset: 0x0048E5C0
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

		// Token: 0x040095A0 RID: 38304
		public float trackDistance = 20f;

		// Token: 0x040095A1 RID: 38305
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal localPlayer;

		// Token: 0x040095A2 RID: 38306
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TrackingEntry.TrackedBlock> TrackedBlocks = new List<TrackingEntry.TrackedBlock>();

		// Token: 0x040095A3 RID: 38307
		public ItemClass TrackedItem;

		// Token: 0x040095A4 RID: 38308
		public BaseChallengeObjective Owner;

		// Token: 0x040095A5 RID: 38309
		public ChallengeTrackingHandler TrackingHelper;

		// Token: 0x040095A6 RID: 38310
		public string blockIndexName = "quest_wood";

		// Token: 0x040095A7 RID: 38311
		public string navObjectName = "quest_resource";

		// Token: 0x0200191F RID: 6431
		public class TrackedBlock
		{
			// Token: 0x0600C6A3 RID: 50851 RVA: 0x00490459 File Offset: 0x0048E659
			public TrackedBlock(Vector3i worldPos, string NavObjectName)
			{
				this.WorldPos = worldPos;
				this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
				this.KeepAlive = true;
			}

			// Token: 0x040095A8 RID: 38312
			public Vector3i WorldPos;

			// Token: 0x040095A9 RID: 38313
			public NavObject NavObject;

			// Token: 0x040095AA RID: 38314
			public bool KeepAlive;
		}
	}
}
