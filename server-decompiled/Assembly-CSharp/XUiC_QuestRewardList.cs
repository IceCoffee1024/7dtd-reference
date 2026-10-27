using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200100A RID: 4106
[Preserve]
public class XUiC_QuestRewardList : XUiController
{
	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x060080FE RID: 33022 RVA: 0x00310153 File Offset: 0x0030E353
	// (set) Token: 0x060080FF RID: 33023 RVA: 0x0031015B File Offset: 0x0030E35B
	public Quest Quest
	{
		get
		{
			return this.quest;
		}
		set
		{
			this.quest = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06008100 RID: 33024 RVA: 0x0031016C File Offset: 0x0030E36C
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_QuestRewardEntry>(null);
		XUiController[] array = childrenByType;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.rewardEntries.Add(array[i]);
			}
		}
	}

	// Token: 0x06008101 RID: 33025 RVA: 0x003101AC File Offset: 0x0030E3AC
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			if (this.quest != null)
			{
				List<BaseReward> list = new List<BaseReward>();
				Quest quest2;
				for (Quest quest = this.quest; quest != null; quest = quest2)
				{
					quest2 = null;
					for (int i = 0; i < quest.Rewards.Count; i++)
					{
						if (quest.Rewards[i] is RewardQuest)
						{
							quest2 = QuestClass.CreateQuest(quest.Rewards[i].ID);
						}
						if (!quest.Rewards[i].HiddenReward && quest.Rewards[i].ReceiveStage == BaseReward.ReceiveStages.QuestCompletion && (quest == this.quest || !(quest.Rewards[i] is RewardQuest)))
						{
							list.Add(quest.Rewards[i]);
						}
					}
				}
				int count = this.rewardEntries.Count;
				int count2 = list.Count;
				int num = 0;
				for (int j = 0; j < count; j++)
				{
					if (this.rewardEntries[num] is XUiC_QuestRewardEntry)
					{
						if (j < count2)
						{
							((XUiC_QuestRewardEntry)this.rewardEntries[num]).Reward = list[j];
							((XUiC_QuestRewardEntry)this.rewardEntries[num]).ChainQuest = list[j].OwnerQuest != this.Quest;
							num++;
						}
						else
						{
							((XUiC_QuestRewardEntry)this.rewardEntries[num]).Reward = null;
							num++;
						}
					}
				}
			}
			else
			{
				int count3 = this.rewardEntries.Count;
				for (int k = 0; k < count3; k++)
				{
					if (this.rewardEntries[k] is XUiC_QuestRewardEntry)
					{
						((XUiC_QuestRewardEntry)this.rewardEntries[k]).Reward = null;
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005FB0 RID: 24496
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005FB1 RID: 24497
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> rewardEntries = new List<XUiController>();
}
