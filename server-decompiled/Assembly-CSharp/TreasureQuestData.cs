using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000979 RID: 2425
[Preserve]
public class TreasureQuestData : BaseQuestData
{
	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x060046E5 RID: 18149 RVA: 0x001BF0A1 File Offset: 0x001BD2A1
	// (set) Token: 0x060046E6 RID: 18150 RVA: 0x001BF0A9 File Offset: 0x001BD2A9
	public int BlocksPerReduction
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x060046E7 RID: 18151 RVA: 0x001BF0B2 File Offset: 0x001BD2B2
	// (set) Token: 0x060046E8 RID: 18152 RVA: 0x001BF0BA File Offset: 0x001BD2BA
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x060046E9 RID: 18153 RVA: 0x001BF0C3 File Offset: 0x001BD2C3
	// (set) Token: 0x060046EA RID: 18154 RVA: 0x001BF0CB File Offset: 0x001BD2CB
	public Vector3 TreasureOffset
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x001BF0D4 File Offset: 0x001BD2D4
	public TreasureQuestData(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.Position = _position;
		this.TreasureOffset = _treasureOffset;
		this.BlocksPerReduction = _blocksPerReduction;
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x001BF106 File Offset: 0x001BD306
	public void AddSharedQuester(int _entityID, int _blocksPerReduction)
	{
		if (_blocksPerReduction < this.BlocksPerReduction)
		{
			this.SendBlocksPerReductionUpdate(this.BlocksPerReduction);
		}
		base.AddSharedQuester(_entityID);
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x001BF124 File Offset: 0x001BD324
	public void SendBlocksPerReductionUpdate(int _newBlocksPerReduction)
	{
		this.BlocksPerReduction = _newBlocksPerReduction;
		World world = GameManager.Instance.World;
		for (int i = 0; i < this.entityList.Count; i++)
		{
			EntityPlayer entityPlayer = world.GetEntity(this.entityList[i]) as EntityPlayer;
			if (entityPlayer is EntityPlayerLocal)
			{
				ObjectiveTreasureChest objectiveForQuest = entityPlayer.QuestJournal.GetObjectiveForQuest<ObjectiveTreasureChest>(this.questCode);
				if (objectiveForQuest != null)
				{
					objectiveForQuest.CurrentBlocksPerReduction = this.BlocksPerReduction;
				}
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestTreasurePoint>().Setup(this.questCode, this.BlocksPerReduction), false, this.entityList[i], -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x001BF1E1 File Offset: 0x001BD3E1
	public void UpdatePosition(Vector3i _pos)
	{
		this.Position = _pos;
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x001BF1EA File Offset: 0x001BD3EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.TreasureQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
	}
}
