using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200096C RID: 2412
[Preserve]
public class ObjectiveModifierTrackBlocks : BaseObjectiveModifier
{
	// Token: 0x06004684 RID: 18052 RVA: 0x001BD016 File Offset: 0x001BB216
	public override void AddHooks()
	{
		base.OwnerObjective.OwnerQuest.TrackingHelper.AddTrackingEntry(this);
		QuestEventManager.Current.BlockChange += this.Current_BlockChange;
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x001BD044 File Offset: 0x001BB244
	public override void RemoveHooks()
	{
		base.OwnerObjective.OwnerQuest.TrackingHelper.RemoveTrackingEntry(this);
		QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
		NavObjectManager instance = NavObjectManager.Instance;
		for (int i = this.TrackedBlocks.Count - 1; i >= 0; i--)
		{
			instance.UnRegisterNavObject(this.TrackedBlocks[i].NavObject);
			this.TrackedBlocks.RemoveAt(i);
		}
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x001BD0C0 File Offset: 0x001BB2C0
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

	// Token: 0x06004687 RID: 18055 RVA: 0x001BD138 File Offset: 0x001BB338
	public void StartUpdate()
	{
		if (this.localPlayer == null)
		{
			this.localPlayer = base.OwnerObjective.OwnerQuest.OwnerJournal.OwnerPlayer;
		}
		for (int i = 0; i < this.TrackedBlocks.Count; i++)
		{
			this.TrackedBlocks[i].KeepAlive = false;
		}
	}

	// Token: 0x06004688 RID: 18056 RVA: 0x001BD198 File Offset: 0x001BB398
	public void HandleTrack(Chunk c)
	{
		List<Vector3i> list;
		if (c.IndexedBlocks.TryGetValue(this.blockIndexName, out list))
		{
			foreach (Vector3i vector3i in list)
			{
				if (!c.GetBlockNoDamage(vector3i.x, vector3i.y, vector3i.z).ischild)
				{
					Vector3i vector3i2 = c.ToWorldPos(vector3i);
					if (Vector3.Distance(vector3i2, this.localPlayer.position) < this.trackDistance)
					{
						this.HandleAddTrackedBlock(vector3i2);
					}
				}
			}
		}
	}

	// Token: 0x06004689 RID: 18057 RVA: 0x001BD244 File Offset: 0x001BB444
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

	// Token: 0x0600468A RID: 18058 RVA: 0x001BD2A8 File Offset: 0x001BB4A8
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
		this.TrackedBlocks.Add(new ObjectiveModifierTrackBlocks.TrackedBlock(pos, this.navObjectName));
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x001BD310 File Offset: 0x001BB510
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveModifierTrackBlocks.PropBlockIndexName))
		{
			this.blockIndexName = properties.Values[ObjectiveModifierTrackBlocks.PropBlockIndexName];
		}
		if (properties.Values.ContainsKey(ObjectiveModifierTrackBlocks.PropNavObjectName))
		{
			this.navObjectName = properties.Values[ObjectiveModifierTrackBlocks.PropNavObjectName];
		}
		if (properties.Values.ContainsKey(ObjectiveModifierTrackBlocks.PropTrackDistance))
		{
			this.trackDistance = StringParsers.ParseFloat(properties.Values[ObjectiveModifierTrackBlocks.PropTrackDistance], 0, -1, NumberStyles.Any);
		}
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x001BD3A8 File Offset: 0x001BB5A8
	public override BaseObjectiveModifier Clone()
	{
		return new ObjectiveModifierTrackBlocks
		{
			blockIndexName = this.blockIndexName,
			navObjectName = this.navObjectName,
			trackDistance = this.trackDistance
		};
	}

	// Token: 0x0400387A RID: 14458
	[PublicizedFrom(EAccessModifier.Protected)]
	public string blockIndexName = "questTracked";

	// Token: 0x0400387B RID: 14459
	[PublicizedFrom(EAccessModifier.Protected)]
	public string navObjectName = "quest_resource";

	// Token: 0x0400387C RID: 14460
	public float trackDistance = 20f;

	// Token: 0x0400387D RID: 14461
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400387E RID: 14462
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<ObjectiveModifierTrackBlocks.TrackedBlock> TrackedBlocks = new List<ObjectiveModifierTrackBlocks.TrackedBlock>();

	// Token: 0x0400387F RID: 14463
	public static string PropBlockIndexName = "block_index_name";

	// Token: 0x04003880 RID: 14464
	public static string PropNavObjectName = "nav_object";

	// Token: 0x04003881 RID: 14465
	public static string PropTrackDistance = "track_distance";

	// Token: 0x0200096D RID: 2413
	public class TrackedBlock
	{
		// Token: 0x0600468F RID: 18063 RVA: 0x001BD427 File Offset: 0x001BB627
		public TrackedBlock(Vector3i worldPos, string NavObjectName)
		{
			this.WorldPos = worldPos;
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
			this.KeepAlive = true;
		}

		// Token: 0x04003882 RID: 14466
		public Vector3i WorldPos;

		// Token: 0x04003883 RID: 14467
		public NavObject NavObject;

		// Token: 0x04003884 RID: 14468
		public bool KeepAlive;
	}
}
