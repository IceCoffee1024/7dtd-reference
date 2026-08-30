using System;
using UnityEngine.Scripting;

// Token: 0x02000FDE RID: 4062
[Preserve]
public class XUiC_QuestTurnInDetailsWindow : XUiController
{
	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x06008013 RID: 32787 RVA: 0x00309833 File Offset: 0x00307A33
	// (set) Token: 0x06008014 RID: 32788 RVA: 0x0030983B File Offset: 0x00307A3B
	public Quest CurrentQuest
	{
		get
		{
			return this.currentQuest;
		}
		set
		{
			this.currentQuest = value;
			this.questClass = ((value != null) ? QuestClass.GetQuest(this.currentQuest.ID) : null);
			base.RefreshBindings();
		}
	}

	// Token: 0x06008015 RID: 32789 RVA: 0x00309866 File Offset: 0x00307A66
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
	}

	// Token: 0x06008016 RID: 32790 RVA: 0x00309880 File Offset: 0x00307A80
	public override void OnOpen()
	{
		base.OnOpen();
		this.CurrentQuest = this.xui.Dialog.QuestTurnIn;
	}

	// Token: 0x06008017 RID: 32791 RVA: 0x003098A0 File Offset: 0x00307AA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2940790865U)
		{
			if (num <= 1468586257U)
			{
				if (num != 161079706U)
				{
					if (num == 1468586257U)
					{
						if (bindingName == "questsubtitle")
						{
							value = ((this.currentQuest != null) ? this.questClass.SubTitle : "");
							return true;
						}
					}
				}
				else if (bindingName == "sharedbyname")
				{
					if (this.currentQuest == null)
					{
						value = "";
					}
					else
					{
						PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.currentQuest.SharedOwnerID);
						if (playerDataFromEntityID != null)
						{
							value = GameUtils.SafeStringFormat(playerDataFromEntityID.PlayerName.DisplayName);
						}
						else
						{
							value = "";
						}
					}
					return true;
				}
			}
			else if (num != 1985695849U)
			{
				if (num == 2940790865U)
				{
					if (bindingName == "questdescription")
					{
						value = ((this.currentQuest != null) ? this.currentQuest.GetParsedText(this.questClass.Description) : "");
						return true;
					}
				}
			}
			else if (bindingName == "questcategory")
			{
				value = ((this.currentQuest != null) ? this.questClass.Category : "");
				return true;
			}
		}
		else if (num <= 3270262403U)
		{
			if (num != 3047389681U)
			{
				if (num == 3270262403U)
				{
					if (bindingName == "npcportrait")
					{
						if (this.currentQuest == null)
						{
							value = "";
						}
						else
						{
							value = this.NPC.NPCInfo.Portrait;
						}
						return true;
					}
				}
			}
			else if (bindingName == "questtitle")
			{
				value = ((this.currentQuest != null) ? this.questtitleFormatter.Format(this.questClass.Category, this.questClass.SubTitle) : this.xuiQuestDescriptionLabel);
				return true;
			}
		}
		else if (num != 3357817217U)
		{
			if (num != 3846167827U)
			{
				if (num == 4060322893U)
				{
					if (bindingName == "showempty")
					{
						value = (this.currentQuest == null).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "npcname")
			{
				if (this.currentQuest == null)
				{
					value = "";
				}
				else
				{
					value = Localization.Get(this.NPC.EntityName, false, null);
				}
				return true;
			}
		}
		else if (bindingName == "questcompletetext")
		{
			value = ((this.currentQuest != null) ? this.questClass.CompleteText : "Needs real complete text.");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x04005ED6 RID: 24278
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005ED7 RID: 24279
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005ED8 RID: 24280
	public EntityNPC NPC;

	// Token: 0x04005ED9 RID: 24281
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005EDA RID: 24282
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> questtitleFormatter = new CachedStringFormatter<string, string>((string _s, string _s1) => string.Format("{0} : {1}", _s, _s1));
}
