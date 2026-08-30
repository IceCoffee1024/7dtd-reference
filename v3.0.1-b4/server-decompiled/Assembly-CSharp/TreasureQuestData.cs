using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000968 RID: 2408
[Preserve]
public class TreasureQuestData : BaseQuestData
{
	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x0600465F RID: 18015 RVA: 0x001BC8AD File Offset: 0x001BAAAD
	// (set) Token: 0x06004660 RID: 18016 RVA: 0x001BC8B5 File Offset: 0x001BAAB5
	public int BlocksPerReduction
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06004661 RID: 18017 RVA: 0x001BC8BE File Offset: 0x001BAABE
	// (set) Token: 0x06004662 RID: 18018 RVA: 0x001BC8C6 File Offset: 0x001BAAC6
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06004663 RID: 18019 RVA: 0x001BC8CF File Offset: 0x001BAACF
	// (set) Token: 0x06004664 RID: 18020 RVA: 0x001BC8D7 File Offset: 0x001BAAD7
	public Vector3 TreasureOffset
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004665 RID: 18021 RVA: 0x001BC8E0 File Offset: 0x001BAAE0
	public TreasureQuestData(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.Position = _position;
		this.TreasureOffset = _treasureOffset;
		this.BlocksPerReduction = _blocksPerReduction;
	}

	// Token: 0x06004666 RID: 18022 RVA: 0x001BC912 File Offset: 0x001BAB12
	public void AddSharedQuester(int _entityID, int _blocksPerReduction)
	{
		if (_blocksPerReduction < this.BlocksPerReduction)
		{
			this.SendBlocksPerReductionUpdate(this.BlocksPerReduction);
		}
		base.AddSharedQuester(_entityID);
	}

	// Token: 0x06004667 RID: 18023 RVA: 0x001BC930 File Offset: 0x001BAB30
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

	// Token: 0x06004668 RID: 18024 RVA: 0x001BC9ED File Offset: 0x001BABED
	public void UpdatePosition(Vector3i _pos)
	{
		this.Position = _pos;
	}

	// Token: 0x06004669 RID: 18025 RVA: 0x001BC9F6 File Offset: 0x001BABF6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.TreasureQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x0600466A RID: 18026 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
	}
}
