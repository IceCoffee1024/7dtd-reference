using System;
using UnityEngine.Scripting;

// Token: 0x02000FC2 RID: 4034
[Preserve]
public class XUiC_QuestDescriptionWindow : XUiController
{
	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x06007F26 RID: 32550 RVA: 0x00303AB8 File Offset: 0x00301CB8
	// (set) Token: 0x06007F27 RID: 32551 RVA: 0x00303AC0 File Offset: 0x00301CC0
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

	// Token: 0x06007F28 RID: 32552 RVA: 0x00303AEB File Offset: 0x00301CEB
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
	}

	// Token: 0x06007F29 RID: 32553 RVA: 0x00303B08 File Offset: 0x00301D08
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

	// Token: 0x06007F2A RID: 32554 RVA: 0x00303CAA File Offset: 0x00301EAA
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

	// Token: 0x04005E0A RID: 24074
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005E0B RID: 24075
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005E0C RID: 24076
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005E0D RID: 24077
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005E0E RID: 24078
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string, string> questtitleFormatter = new CachedStringFormatter<string, string, string>((string _s, string _s1, string _s2) => string.Format("{0} : {1} {2}", _s, _s1, _s2));
}
