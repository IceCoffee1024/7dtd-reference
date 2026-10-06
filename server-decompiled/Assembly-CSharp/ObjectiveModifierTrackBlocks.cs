using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000983 RID: 2435
[Preserve]
public class ObjectiveModifierTrackBlocks : BaseObjectiveModifier
{
	// Token: 0x060046AB RID: 18091 RVA: 0x001BFD12 File Offset: 0x001BDF12
	public override void AddHooks()
	{
		base.OwnerObjective.OwnerQuest.TrackingHelper.AddTrackingEntry(this);
		QuestEventManager.Current.BlockChange += this.Current_BlockChange;
	}

	// Token: 0x060046AC RID: 18092 RVA: 0x001BFD40 File Offset: 0x001BDF40
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

	// Token: 0x060046AD RID: 18093 RVA: 0x001BFDBC File Offset: 0x001BDFBC
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

	// Token: 0x060046AE RID: 18094 RVA: 0x001BFE34 File Offset: 0x001BE034
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

	// Token: 0x060046AF RID: 18095 RVA: 0x001BFE94 File Offset: 0x001BE094
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

	// Token: 0x060046B0 RID: 18096 RVA: 0x001BFF40 File Offset: 0x001BE140
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

	// Token: 0x060046B1 RID: 18097 RVA: 0x001BFFA4 File Offset: 0x001BE1A4
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

	// Token: 0x060046B2 RID: 18098 RVA: 0x001C000C File Offset: 0x001BE20C
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

	// Token: 0x060046B3 RID: 18099 RVA: 0x001C00A4 File Offset: 0x001BE2A4
	public override BaseObjectiveModifier Clone()
	{
		return new ObjectiveModifierTrackBlocks
		{
			blockIndexName = this.blockIndexName,
			navObjectName = this.navObjectName,
			trackDistance = this.trackDistance
		};
	}

	// Token: 0x04003907 RID: 14599
	[PublicizedFrom(EAccessModifier.Protected)]
	public string blockIndexName = "questTracked";

	// Token: 0x04003908 RID: 14600
	[PublicizedFrom(EAccessModifier.Protected)]
	public string navObjectName = "quest_resource";

	// Token: 0x04003909 RID: 14601
	public float trackDistance = 20f;

	// Token: 0x0400390A RID: 14602
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400390B RID: 14603
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<ObjectiveModifierTrackBlocks.TrackedBlock> TrackedBlocks = new List<ObjectiveModifierTrackBlocks.TrackedBlock>();

	// Token: 0x0400390C RID: 14604
	public static string PropBlockIndexName = "block_index_name";

	// Token: 0x0400390D RID: 14605
	public static string PropNavObjectName = "nav_object";

	// Token: 0x0400390E RID: 14606
	public static string PropTrackDistance = "track_distance";

	// Token: 0x02000984 RID: 2436
	public class TrackedBlock
	{
		// Token: 0x060046B6 RID: 18102 RVA: 0x001C0123 File Offset: 0x001BE323
		public TrackedBlock(Vector3i worldPos, string NavObjectName)
		{
			this.WorldPos = worldPos;
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
			this.KeepAlive = true;
		}

		// Token: 0x0400390F RID: 14607
		public Vector3i WorldPos;

		// Token: 0x04003910 RID: 14608
		public NavObject NavObject;

		// Token: 0x04003911 RID: 14609
		public bool KeepAlive;
	}
}
