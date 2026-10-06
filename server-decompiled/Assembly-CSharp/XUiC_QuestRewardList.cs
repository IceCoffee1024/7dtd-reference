using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FFA RID: 4090
[Preserve]
public class XUiC_QuestRewardList : XUiController
{
	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x060080AF RID: 32943 RVA: 0x0030E637 File Offset: 0x0030C837
	// (set) Token: 0x060080B0 RID: 32944 RVA: 0x0030E63F File Offset: 0x0030C83F
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

	// Token: 0x060080B1 RID: 32945 RVA: 0x0030E650 File Offset: 0x0030C850
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

	// Token: 0x060080B2 RID: 32946 RVA: 0x0030E690 File Offset: 0x0030C890
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

	// Token: 0x04005F75 RID: 24437
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F76 RID: 24438
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> rewardEntries = new List<XUiController>();
}
