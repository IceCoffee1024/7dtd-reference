using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FEB RID: 4075
[Preserve]
public class XUiC_QuestRewardList : XUiController
{
	// Token: 0x17000EFF RID: 3839
	// (get) Token: 0x060080A0 RID: 32928 RVA: 0x0030B64F File Offset: 0x0030984F
	// (set) Token: 0x060080A1 RID: 32929 RVA: 0x0030B657 File Offset: 0x00309857
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

	// Token: 0x060080A2 RID: 32930 RVA: 0x0030B668 File Offset: 0x00309868
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

	// Token: 0x060080A3 RID: 32931 RVA: 0x0030B6A8 File Offset: 0x003098A8
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

	// Token: 0x04005F4C RID: 24396
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F4D RID: 24397
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> rewardEntries = new List<XUiController>();
}
