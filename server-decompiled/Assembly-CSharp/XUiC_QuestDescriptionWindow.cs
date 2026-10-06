using System;
using UnityEngine.Scripting;

// Token: 0x02000FEA RID: 4074
[Preserve]
public class XUiC_QuestDescriptionWindow : XUiController
{
	// Token: 0x17000F0E RID: 3854
	// (get) Token: 0x06008027 RID: 32807 RVA: 0x0030B264 File Offset: 0x00309464
	// (set) Token: 0x06008028 RID: 32808 RVA: 0x0030B26C File Offset: 0x0030946C
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

	// Token: 0x06008029 RID: 32809 RVA: 0x0030B297 File Offset: 0x00309497
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
	}

	// Token: 0x0600802A RID: 32810 RVA: 0x0030B2B4 File Offset: 0x003094B4
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

	// Token: 0x0600802B RID: 32811 RVA: 0x0030B456 File Offset: 0x00309656
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

	// Token: 0x04005EFA RID: 24314
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005EFB RID: 24315
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005EFC RID: 24316
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005EFD RID: 24317
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005EFE RID: 24318
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string, string> questtitleFormatter = new CachedStringFormatter<string, string, string>((string _s, string _s1, string _s2) => string.Format("{0} : {1} {2}", _s, _s1, _s2));
}
