using System;
using System.Collections.Generic;

// Token: 0x020002EA RID: 746
public class DialogResponseQuest : DialogResponse
{
	// Token: 0x06001574 RID: 5492 RVA: 0x0008056C File Offset: 0x0007E76C
	public DialogResponseQuest(string _questID, string _nextStatementID, string _returnStatementID, string _type, Dialog _ownerDialog, int _listIndex = -1, int _tier = -1)
		: base(_questID)
	{
		Quest quest = null;
		base.OwnerDialog = _ownerDialog;
		this.LastStatementID = _returnStatementID;
		this.Tier = _tier;
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(GameManager.Instance.World.GetPrimaryPlayer());
		EntityTrader entityTrader = uiforPlayer.xui.Dialog.Respondent as EntityTrader;
		if (entityTrader == null)
		{
			this.IsValid = false;
			return;
		}
		if (this.ID != "")
		{
			quest = QuestClass.GetQuest(this.ID).CreateQuest();
			quest.SetupTags();
			quest.QuestFaction = entityTrader.NPCInfo.QuestFaction;
			if (!quest.SetupPosition(entityTrader, null, null, -1))
			{
				this.IsValid = false;
			}
		}
		else
		{
			List<Quest> activeQuests = entityTrader.activeQuests;
			if (_type == "")
			{
				if (this.Tier == -1)
				{
					if (activeQuests != null && _listIndex < activeQuests.Count && activeQuests[_listIndex].QuestClass.QuestType == "")
					{
						quest = activeQuests[_listIndex];
					}
					else
					{
						this.IsValid = false;
					}
				}
				else if (activeQuests != null)
				{
					int num = 0;
					bool flag = false;
					for (int i = 0; i < activeQuests.Count; i++)
					{
						if ((int)activeQuests[i].QuestClass.DifficultyTier == this.Tier && activeQuests[i].QuestClass.QuestType == "")
						{
							if (num == _listIndex)
							{
								quest = activeQuests[i];
								flag = true;
								break;
							}
							num++;
						}
					}
					if (!flag)
					{
						this.IsValid = false;
					}
				}
			}
			else
			{
				int num2 = 0;
				int currentFactionTier = uiforPlayer.entityPlayer.QuestJournal.GetCurrentFactionTier(entityTrader.NPCInfo.QuestFaction, 0, false);
				for (int j = 0; j < activeQuests.Count; j++)
				{
					if (activeQuests[j].QuestClass.QuestType == _type && (int)activeQuests[j].QuestClass.DifficultyTier <= currentFactionTier)
					{
						if (_listIndex == num2)
						{
							quest = activeQuests[j];
							num2 = -1;
							break;
						}
						num2++;
					}
				}
				if (num2 != -1)
				{
					this.IsValid = false;
				}
			}
		}
		if (this.IsValid)
		{
			if (quest.QuestClass.ExtraTags.Test_AnySet(DialogResponseQuest.introtag) && !QuestJournal.IntroQuestEnabled)
			{
				this.IsValid = false;
				return;
			}
			this.Quest = quest;
			base.AddAction(new DialogActionAddQuest
			{
				Quest = quest,
				Owner = this,
				OwnerDialog = base.OwnerDialog,
				ListIndex = _listIndex
			});
			this.ReturnStatementID = _nextStatementID;
			base.NextStatementID = _nextStatementID;
			string text = ValueDisplayFormatters.RomanNumber((int)quest.QuestClass.DifficultyTier);
			this.Text = string.Concat(new string[]
			{
				"[[DECEA3]",
				Localization.Get("xuiTier", false, null).ToUpper(),
				" ",
				text,
				"[-]] ",
				quest.GetParsedText(quest.QuestClass.ResponseText)
			});
		}
	}

	// Token: 0x04000E65 RID: 3685
	public bool IsValid = true;

	// Token: 0x04000E66 RID: 3686
	public Quest Quest;

	// Token: 0x04000E67 RID: 3687
	public int Variation = -1;

	// Token: 0x04000E68 RID: 3688
	public int Tier = -1;

	// Token: 0x04000E69 RID: 3689
	public string LastStatementID;

	// Token: 0x04000E6A RID: 3690
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> introtag = FastTags<TagGroup.Global>.Parse("introquest");
}
