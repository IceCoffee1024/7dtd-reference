using System;
using UnityEngine.Scripting;

// Token: 0x02000FF7 RID: 4087
[Preserve]
public class XUiC_QuestTurnInDetailsWindow : XUiController
{
	// Token: 0x17000F0E RID: 3854
	// (get) Token: 0x06008105 RID: 33029 RVA: 0x0030E067 File Offset: 0x0030C267
	// (set) Token: 0x06008106 RID: 33030 RVA: 0x0030E06F File Offset: 0x0030C26F
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

	// Token: 0x06008107 RID: 33031 RVA: 0x0030E09A File Offset: 0x0030C29A
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
	}

	// Token: 0x06008108 RID: 33032 RVA: 0x0030E0B4 File Offset: 0x0030C2B4
	public override void OnOpen()
	{
		base.OnOpen();
		this.CurrentQuest = this.xui.Dialog.QuestTurnIn;
	}

	// Token: 0x06008109 RID: 33033 RVA: 0x0030E0D4 File Offset: 0x0030C2D4
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

	// Token: 0x04005F9D RID: 24477
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F9E RID: 24478
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005F9F RID: 24479
	public EntityNPC NPC;

	// Token: 0x04005FA0 RID: 24480
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005FA1 RID: 24481
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> questtitleFormatter = new CachedStringFormatter<string, string>((string _s, string _s1) => string.Format("{0} : {1}", _s, _s1));
}
