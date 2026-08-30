using System;
using UnityEngine.Scripting;

// Token: 0x02000FDB RID: 4059
[Preserve]
public class XUiC_QuestDescriptionWindow : XUiController
{
	// Token: 0x17000EE7 RID: 3815
	// (get) Token: 0x06008018 RID: 32792 RVA: 0x0030828C File Offset: 0x0030648C
	// (set) Token: 0x06008019 RID: 32793 RVA: 0x00308294 File Offset: 0x00306494
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

	// Token: 0x0600801A RID: 32794 RVA: 0x003082BF File Offset: 0x003064BF
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
	}

	// Token: 0x0600801B RID: 32795 RVA: 0x003082DC File Offset: 0x003064DC
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

	// Token: 0x0600801C RID: 32796 RVA: 0x0030847E File Offset: 0x0030667E
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

	// Token: 0x04005ED1 RID: 24273
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005ED2 RID: 24274
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005ED3 RID: 24275
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005ED4 RID: 24276
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005ED5 RID: 24277
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string, string> questtitleFormatter = new CachedStringFormatter<string, string, string>((string _s, string _s1, string _s2) => string.Format("{0} : {1} {2}", _s, _s1, _s2));
}
