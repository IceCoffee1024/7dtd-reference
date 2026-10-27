using System;
using UnityEngine.Scripting;

// Token: 0x02000FFA RID: 4090
[Preserve]
public class XUiC_QuestDescriptionWindow : XUiController
{
	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x06008076 RID: 32886 RVA: 0x0030CD80 File Offset: 0x0030AF80
	// (set) Token: 0x06008077 RID: 32887 RVA: 0x0030CD88 File Offset: 0x0030AF88
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

	// Token: 0x06008078 RID: 32888 RVA: 0x0030CDB3 File Offset: 0x0030AFB3
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
	}

	// Token: 0x06008079 RID: 32889 RVA: 0x0030CDD0 File Offset: 0x0030AFD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "questdescription")
		{
			value = ((this.currentQuest != null) ? this.currentQuest.GetParsedText(this.questClass.Description) : "");
			return true;
		}
		if (bindingName == "questcategory")
		{
			value = ((this.currentQuest != null) ? this.questClass.Category : "");
			return true;
		}
		if (bindingName == "questsubtitle")
		{
			value = ((this.currentQuest != null) ? this.questClass.SubTitle : "");
			return true;
		}
		if (bindingName == "questtitle")
		{
			value = ((this.currentQuest != null) ? this.questtitleFormatter.Format(this.questClass.Category, this.questClass.SubTitle, (this.currentQuest.GetSharedWithCount() == 0) ? "" : ("(" + this.currentQuest.GetSharedWithCount().ToString() + ")")) : this.xuiQuestDescriptionLabel);
			return true;
		}
		if (bindingName == "sharedbyname")
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
		if (!(bindingName == "showempty"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = (this.currentQuest == null).ToString();
		return true;
	}

	// Token: 0x0600807A RID: 32890 RVA: 0x0030CF72 File Offset: 0x0030B172
	public void SetQuest(XUiC_QuestEntry questEntry)
	{
		this.entry = questEntry;
		if (this.entry != null)
		{
			this.CurrentQuest = this.entry.Quest;
			return;
		}
		this.CurrentQuest = null;
	}

	// Token: 0x04005F35 RID: 24373
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005F36 RID: 24374
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F37 RID: 24375
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005F38 RID: 24376
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005F39 RID: 24377
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string, string> questtitleFormatter = new CachedStringFormatter<string, string, string>((string _s, string _s1, string _s2) => string.Format("{0} : {1} {2}", _s, _s1, _s2));
}
