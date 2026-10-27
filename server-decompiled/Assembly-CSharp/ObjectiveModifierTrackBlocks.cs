using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098D RID: 2445
[Preserve]
public class ObjectiveModifierTrackBlocks : BaseObjectiveModifier
{
	// Token: 0x060046CE RID: 18126 RVA: 0x001C0D96 File Offset: 0x001BEF96
	public override void AddHooks()
	{
		base.OwnerObjective.OwnerQuest.TrackingHelper.AddTrackingEntry(this);
		QuestEventManager.Current.BlockChange += this.Current_BlockChange;
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x001C0DC4 File Offset: 0x001BEFC4
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

	// Token: 0x060046D0 RID: 18128 RVA: 0x001C0E40 File Offset: 0x001BF040
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

	// Token: 0x060046D1 RID: 18129 RVA: 0x001C0EB8 File Offset: 0x001BF0B8
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

	// Token: 0x060046D2 RID: 18130 RVA: 0x001C0F18 File Offset: 0x001BF118
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

	// Token: 0x060046D3 RID: 18131 RVA: 0x001C0FC4 File Offset: 0x001BF1C4
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

	// Token: 0x060046D4 RID: 18132 RVA: 0x001C1028 File Offset: 0x001BF228
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

	// Token: 0x060046D5 RID: 18133 RVA: 0x001C1090 File Offset: 0x001BF290
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

	// Token: 0x060046D6 RID: 18134 RVA: 0x001C1128 File Offset: 0x001BF328
	public override BaseObjectiveModifier Clone()
	{
		return new ObjectiveModifierTrackBlocks
		{
			blockIndexName = this.blockIndexName,
			navObjectName = this.navObjectName,
			trackDistance = this.trackDistance
		};
	}

	// Token: 0x04003927 RID: 14631
	[PublicizedFrom(EAccessModifier.Protected)]
	public string blockIndexName = "questTracked";

	// Token: 0x04003928 RID: 14632
	[PublicizedFrom(EAccessModifier.Protected)]
	public string navObjectName = "quest_resource";

	// Token: 0x04003929 RID: 14633
	public float trackDistance = 20f;

	// Token: 0x0400392A RID: 14634
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400392B RID: 14635
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<ObjectiveModifierTrackBlocks.TrackedBlock> TrackedBlocks = new List<ObjectiveModifierTrackBlocks.TrackedBlock>();

	// Token: 0x0400392C RID: 14636
	public static string PropBlockIndexName = "block_index_name";

	// Token: 0x0400392D RID: 14637
	public static string PropNavObjectName = "nav_object";

	// Token: 0x0400392E RID: 14638
	public static string PropTrackDistance = "track_distance";

	// Token: 0x0200098E RID: 2446
	public class TrackedBlock
	{
		// Token: 0x060046D9 RID: 18137 RVA: 0x001C11A7 File Offset: 0x001BF3A7
		public TrackedBlock(Vector3i worldPos, string NavObjectName)
		{
			this.WorldPos = worldPos;
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
			this.KeepAlive = true;
		}

		// Token: 0x0400392F RID: 14639
		public Vector3i WorldPos;

		// Token: 0x04003930 RID: 14640
		public NavObject NavObject;

		// Token: 0x04003931 RID: 14641
		public bool KeepAlive;
	}
}
