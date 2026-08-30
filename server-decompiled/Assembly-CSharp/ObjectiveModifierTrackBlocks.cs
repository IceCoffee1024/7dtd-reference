using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200097D RID: 2429
[Preserve]
public class ObjectiveModifierTrackBlocks : BaseObjectiveModifier
{
	// Token: 0x0600470A RID: 18186 RVA: 0x001BF80A File Offset: 0x001BDA0A
	public override void AddHooks()
	{
		base.OwnerObjective.OwnerQuest.TrackingHelper.AddTrackingEntry(this);
		QuestEventManager.Current.BlockChange += this.Current_BlockChange;
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x001BF838 File Offset: 0x001BDA38
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

	// Token: 0x0600470C RID: 18188 RVA: 0x001BF8B4 File Offset: 0x001BDAB4
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

	// Token: 0x0600470D RID: 18189 RVA: 0x001BF92C File Offset: 0x001BDB2C
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

	// Token: 0x0600470E RID: 18190 RVA: 0x001BF98C File Offset: 0x001BDB8C
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

	// Token: 0x0600470F RID: 18191 RVA: 0x001BFA38 File Offset: 0x001BDC38
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

	// Token: 0x06004710 RID: 18192 RVA: 0x001BFA9C File Offset: 0x001BDC9C
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

	// Token: 0x06004711 RID: 18193 RVA: 0x001BFB04 File Offset: 0x001BDD04
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

	// Token: 0x06004712 RID: 18194 RVA: 0x001BFB9C File Offset: 0x001BDD9C
	public override BaseObjectiveModifier Clone()
	{
		return new ObjectiveModifierTrackBlocks
		{
			blockIndexName = this.blockIndexName,
			navObjectName = this.navObjectName,
			trackDistance = this.trackDistance
		};
	}

	// Token: 0x0400390B RID: 14603
	[PublicizedFrom(EAccessModifier.Protected)]
	public string blockIndexName = "questTracked";

	// Token: 0x0400390C RID: 14604
	[PublicizedFrom(EAccessModifier.Protected)]
	public string navObjectName = "quest_resource";

	// Token: 0x0400390D RID: 14605
	public float trackDistance = 20f;

	// Token: 0x0400390E RID: 14606
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal localPlayer;

	// Token: 0x0400390F RID: 14607
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<ObjectiveModifierTrackBlocks.TrackedBlock> TrackedBlocks = new List<ObjectiveModifierTrackBlocks.TrackedBlock>();

	// Token: 0x04003910 RID: 14608
	public static string PropBlockIndexName = "block_index_name";

	// Token: 0x04003911 RID: 14609
	public static string PropNavObjectName = "nav_object";

	// Token: 0x04003912 RID: 14610
	public static string PropTrackDistance = "track_distance";

	// Token: 0x0200097E RID: 2430
	public class TrackedBlock
	{
		// Token: 0x06004715 RID: 18197 RVA: 0x001BFC1B File Offset: 0x001BDE1B
		public TrackedBlock(Vector3i worldPos, string NavObjectName)
		{
			this.WorldPos = worldPos;
			this.NavObject = NavObjectManager.Instance.RegisterNavObject(NavObjectName, this.WorldPos.ToVector3Center(), "", false, -1, null);
			this.KeepAlive = true;
		}

		// Token: 0x04003913 RID: 14611
		public Vector3i WorldPos;

		// Token: 0x04003914 RID: 14612
		public NavObject NavObject;

		// Token: 0x04003915 RID: 14613
		public bool KeepAlive;
	}
}
