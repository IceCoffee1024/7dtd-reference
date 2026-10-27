using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000989 RID: 2441
[Preserve]
public class TreasureQuestData : BaseQuestData
{
	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x060046A9 RID: 18089 RVA: 0x001C062D File Offset: 0x001BE82D
	// (set) Token: 0x060046AA RID: 18090 RVA: 0x001C0635 File Offset: 0x001BE835
	public int BlocksPerReduction
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x060046AB RID: 18091 RVA: 0x001C063E File Offset: 0x001BE83E
	// (set) Token: 0x060046AC RID: 18092 RVA: 0x001C0646 File Offset: 0x001BE846
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x060046AD RID: 18093 RVA: 0x001C064F File Offset: 0x001BE84F
	// (set) Token: 0x060046AE RID: 18094 RVA: 0x001C0657 File Offset: 0x001BE857
	public Vector3 TreasureOffset
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060046AF RID: 18095 RVA: 0x001C0660 File Offset: 0x001BE860
	public TreasureQuestData(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.Position = _position;
		this.TreasureOffset = _treasureOffset;
		this.BlocksPerReduction = _blocksPerReduction;
	}

	// Token: 0x060046B0 RID: 18096 RVA: 0x001C0692 File Offset: 0x001BE892
	public void AddSharedQuester(int _entityID, int _blocksPerReduction)
	{
		if (_blocksPerReduction < this.BlocksPerReduction)
		{
			this.SendBlocksPerReductionUpdate(this.BlocksPerReduction);
		}
		base.AddSharedQuester(_entityID);
	}

	// Token: 0x060046B1 RID: 18097 RVA: 0x001C06B0 File Offset: 0x001BE8B0
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

	// Token: 0x060046B2 RID: 18098 RVA: 0x001C076D File Offset: 0x001BE96D
	public void UpdatePosition(Vector3i _pos)
	{
		this.Position = _pos;
	}

	// Token: 0x060046B3 RID: 18099 RVA: 0x001C0776 File Offset: 0x001BE976
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.TreasureQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x060046B4 RID: 18100 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
	}
}
