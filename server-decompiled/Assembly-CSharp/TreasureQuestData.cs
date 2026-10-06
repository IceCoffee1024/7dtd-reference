using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200097F RID: 2431
[Preserve]
public class TreasureQuestData : BaseQuestData
{
	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06004686 RID: 18054 RVA: 0x001BF5A9 File Offset: 0x001BD7A9
	// (set) Token: 0x06004687 RID: 18055 RVA: 0x001BF5B1 File Offset: 0x001BD7B1
	public int BlocksPerReduction
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x06004688 RID: 18056 RVA: 0x001BF5BA File Offset: 0x001BD7BA
	// (set) Token: 0x06004689 RID: 18057 RVA: 0x001BF5C2 File Offset: 0x001BD7C2
	public Vector3i Position
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x0600468A RID: 18058 RVA: 0x001BF5CB File Offset: 0x001BD7CB
	// (set) Token: 0x0600468B RID: 18059 RVA: 0x001BF5D3 File Offset: 0x001BD7D3
	public Vector3 TreasureOffset
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x001BF5DC File Offset: 0x001BD7DC
	public TreasureQuestData(int _questCode, int _entityID, int _blocksPerReduction, Vector3i _position, Vector3 _treasureOffset)
	{
		this.questCode = _questCode;
		this.entityList.Add(_entityID);
		this.Position = _position;
		this.TreasureOffset = _treasureOffset;
		this.BlocksPerReduction = _blocksPerReduction;
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x001BF60E File Offset: 0x001BD80E
	public void AddSharedQuester(int _entityID, int _blocksPerReduction)
	{
		if (_blocksPerReduction < this.BlocksPerReduction)
		{
			this.SendBlocksPerReductionUpdate(this.BlocksPerReduction);
		}
		base.AddSharedQuester(_entityID);
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x001BF62C File Offset: 0x001BD82C
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

	// Token: 0x0600468F RID: 18063 RVA: 0x001BF6E9 File Offset: 0x001BD8E9
	public void UpdatePosition(Vector3i _pos)
	{
		this.Position = _pos;
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x001BF6F2 File Offset: 0x001BD8F2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void RemoveFromDictionary()
	{
		QuestEventManager.Current.TreasureQuestDictionary.Remove(this.questCode);
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnRemove(EntityPlayer player)
	{
	}
}
