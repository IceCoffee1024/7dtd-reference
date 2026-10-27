using System;
using System.Collections.Generic;
using Twitch;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010BB RID: 4283
[Preserve]
public class XUiC_TwitchCommandList : XUiController
{
	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x060087E5 RID: 34789 RVA: 0x0033EBCF File Offset: 0x0033CDCF
	// (set) Token: 0x060087E6 RID: 34790 RVA: 0x0033EBD7 File Offset: 0x0033CDD7
	public XUiC_TwitchWindow Owner { get; set; }

	// Token: 0x060087E7 RID: 34791 RVA: 0x0033EBE0 File Offset: 0x0033CDE0
	public float GetHeight()
	{
		if (!this.twitchManager.IsReady || this.twitchManager.VotingManager.VotingIsActive)
		{
			return 0f;
		}
		if (this.commandLists.ContainsKey(this.CurrentKey))
		{
			return (float)(this.commandLists[this.CurrentKey].Count * 30);
		}
		return 0f;
	}

	// Token: 0x060087E8 RID: 34792 RVA: 0x0033EC48 File Offset: 0x0033CE48
	public override void Init()
	{
		base.Init();
		XUiC_TwitchCommandEntry[] childrenByType = base.GetChildrenByType<XUiC_TwitchCommandEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			if (childrenByType[i] != null)
			{
				this.commandEntries.Add(childrenByType[i]);
			}
		}
		this.twitchManager = TwitchManager.Current;
		this.twitchManager.CommandsChanged -= this.TwitchManager_CommandsChanged;
		this.twitchManager.CommandsChanged += this.TwitchManager_CommandsChanged;
	}

	// Token: 0x060087E9 RID: 34793 RVA: 0x0033ECBD File Offset: 0x0033CEBD
	[PublicizedFrom(EAccessModifier.Private)]
	public void TwitchManager_CommandsChanged()
	{
		this.SetupCommandList();
		this.lastUpdate = 0f;
		this.commandListIndex = -1;
	}

	// Token: 0x060087EA RID: 34794 RVA: 0x0033ECD8 File Offset: 0x0033CED8
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetPrevCategory()
	{
		bool flag = false;
		int num = 0;
		while (!flag)
		{
			this.commandListIndex--;
			if (this.commandListIndex < 0)
			{
				this.commandListIndex = this.commandGroupList.Count - 1;
			}
			if (this.commandLists.ContainsKey(this.commandGroupList[this.commandListIndex].groupName))
			{
				flag = true;
			}
			num++;
			if (num > this.commandGroupList.Count)
			{
				break;
			}
		}
		this.ResetKey();
	}

	// Token: 0x060087EB RID: 34795 RVA: 0x0033ED58 File Offset: 0x0033CF58
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetNextCategory()
	{
		bool flag = false;
		int num = 0;
		while (!flag)
		{
			this.commandListIndex++;
			if (this.commandListIndex >= this.commandGroupList.Count)
			{
				this.commandListIndex = 0;
			}
			if (this.commandGroupList.Count == 0 || this.commandLists.ContainsKey(this.commandGroupList[this.commandListIndex].groupName))
			{
				flag = true;
			}
			num++;
			if (num > this.commandGroupList.Count)
			{
				break;
			}
		}
		this.ResetKey();
	}

	// Token: 0x060087EC RID: 34796 RVA: 0x0033EDE0 File Offset: 0x0033CFE0
	public override void Update(float _dt)
	{
		if (!this.twitchManager.IsReady)
		{
			return;
		}
		if (Time.time - this.lastUpdate >= this.secondRotation)
		{
			this.IsDirty = true;
			if (this.commandLists.Count > 0)
			{
				this.GetNextCategory();
			}
			this.lastUpdate = Time.time;
		}
		if (this.IsDirty)
		{
			if (this.commandLists.Count == 0)
			{
				this.SetupCommandList();
			}
			if (this.commandLists.Count != 0 && this.commandLists.ContainsKey(this.CurrentKey))
			{
				TwitchAction[] array = (from a in this.commandLists[this.CurrentKey]
					orderby a.Command
					orderby a.PointType
					select a).ToArray<TwitchAction>();
				int num = 0;
				int num2 = 0;
				while (num2 < array.Length && num < this.commandEntries.Count)
				{
					if (this.commandEntries[num] != null)
					{
						this.commandEntries[num].Owner = this.Owner;
						this.commandEntries[num].Action = array[num2];
						num++;
					}
					num2++;
				}
				for (int i = num; i < this.commandEntries.Count; i++)
				{
					this.commandEntries[i].Action = null;
				}
				this.IsDirty = false;
			}
		}
		base.Update(_dt);
	}

	// Token: 0x060087ED RID: 34797 RVA: 0x0033EF69 File Offset: 0x0033D169
	public void MoveForward()
	{
		this.GetNextCategory();
		this.lastUpdate = Time.time - 2f;
		this.IsDirty = true;
	}

	// Token: 0x060087EE RID: 34798 RVA: 0x0033EF89 File Offset: 0x0033D189
	public void MoveBackward()
	{
		this.GetPrevCategory();
		this.lastUpdate = Time.time - 2f;
		this.IsDirty = true;
	}

	// Token: 0x060087EF RID: 34799 RVA: 0x0033EFAC File Offset: 0x0033D1AC
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchActionGroup AddCommandGroup(string groupName)
	{
		for (int i = 0; i < this.commandGroupList.Count; i++)
		{
			if (this.commandGroupList[i].groupName == groupName)
			{
				return this.commandGroupList[i];
			}
		}
		int categoryIndex = TwitchActionManager.Current.GetCategoryIndex(groupName);
		this.commandGroupList.Add(new TwitchActionGroup
		{
			ActionList = new List<TwitchAction>(),
			groupName = groupName,
			displayName = TwitchActionManager.Current.CategoryList[categoryIndex].DisplayName,
			index = categoryIndex
		});
		return this.commandGroupList[this.commandGroupList.Count - 1];
	}

	// Token: 0x060087F0 RID: 34800 RVA: 0x0033F060 File Offset: 0x0033D260
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveCommandGroup(string groupName)
	{
		for (int i = 0; i < this.commandGroupList.Count; i++)
		{
			if (this.commandGroupList[i].groupName == groupName)
			{
				this.commandGroupList.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x060087F1 RID: 34801 RVA: 0x0033F0AC File Offset: 0x0033D2AC
	public void SetupCommandList()
	{
		this.commandLists.Clear();
		string name = TwitchActionManager.Current.CategoryList[0].Name;
		int num = 0;
		foreach (string text in this.twitchManager.AvailableCommands.Keys)
		{
			TwitchAction twitchAction = this.twitchManager.AvailableCommands[text];
			if (!twitchAction.OnCooldown && twitchAction.OnlyUsableByType == TwitchAction.OnlyUsableTypes.Everyone)
			{
				num++;
			}
			if (num > 10)
			{
				break;
			}
		}
		bool flag = num <= 10;
		if (flag)
		{
			this.commandLists.Add(name, new List<TwitchAction>());
		}
		foreach (string text2 in this.twitchManager.AvailableCommands.Keys)
		{
			TwitchAction twitchAction2 = this.twitchManager.AvailableCommands[text2];
			if (twitchAction2.PointType != TwitchAction.PointTypes.Bits || !(this.twitchManager.BroadcasterType == ""))
			{
				if (flag)
				{
					if (!twitchAction2.OnCooldown && twitchAction2.OnlyUsableByType == TwitchAction.OnlyUsableTypes.Everyone)
					{
						twitchAction2.groupIndex = 0;
						this.commandLists[name].Add(twitchAction2);
						this.AddCommandGroup(name).ActionList.Add(twitchAction2);
					}
				}
				else if (twitchAction2.HasExtraConditions())
				{
					string text3 = twitchAction2.CategoryNames[0];
					if (!this.commandLists.ContainsKey(text3))
					{
						this.commandLists.Add(text3, new List<TwitchAction>());
					}
					this.AddCommandGroup(text3).ActionList.Add(twitchAction2);
					twitchAction2.groupIndex = 0;
					this.commandLists[text3].Add(twitchAction2);
				}
			}
		}
		this.commandListIndex = 0;
		this.lastUpdate = Time.time;
		if (!flag)
		{
			bool flag2 = true;
			while (flag2)
			{
				flag2 = false;
				foreach (string text4 in this.commandLists.Keys)
				{
					bool flag3 = false;
					if (this.commandLists[text4].Count > 10)
					{
						List<TwitchAction> list = this.commandLists[text4];
						this.commandLists.Remove(text4);
						this.RemoveCommandGroup(text4);
						for (int i = 0; i < list.Count; i++)
						{
							TwitchAction twitchAction3 = list[i];
							string text5 = twitchAction3.CategoryNames[twitchAction3.groupIndex];
							if (twitchAction3.CategoryNames.Count > twitchAction3.groupIndex + 1)
							{
								twitchAction3.groupIndex++;
								text5 = twitchAction3.CategoryNames[twitchAction3.groupIndex];
								flag3 = true;
							}
							if (!this.commandLists.ContainsKey(text5))
							{
								this.commandLists.Add(text5, new List<TwitchAction>());
							}
							this.AddCommandGroup(text5).ActionList.Add(twitchAction3);
							this.commandLists[text5].Add(twitchAction3);
						}
						if (flag3)
						{
							flag2 = true;
							break;
						}
						this.commandLists.Remove(text4);
						this.RemoveCommandGroup(text4);
						for (int j = 0; j < list.Count; j++)
						{
							TwitchAction twitchAction4 = list[j];
							int num2 = j / 10 + 1;
							string text6 = twitchAction4.CategoryNames[twitchAction4.groupIndex] + num2.ToString();
							if (!this.commandLists.ContainsKey(text6))
							{
								this.commandLists.Add(text6, new List<TwitchAction>());
							}
							this.AddCommandGroup(text6).ActionList.Add(twitchAction4);
							this.commandLists[text6].Add(twitchAction4);
						}
						flag2 = true;
						break;
					}
				}
			}
		}
		this.commandGroupList = (from x in this.commandGroupList
			orderby x.index, x.groupName
			select x).ToList<TwitchActionGroup>();
		this.ResetKey();
	}

	// Token: 0x060087F2 RID: 34802 RVA: 0x0033F568 File Offset: 0x0033D768
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetKey()
	{
		if (this.commandLists.Count <= 1)
		{
			this.CurrentTitle = (this.CurrentKey = Localization.Get("TwitchActionCategory_Commands", false, null));
			return;
		}
		this.CurrentKey = this.commandGroupList[this.commandListIndex].groupName;
		this.CurrentTitle = this.commandGroupList[this.commandListIndex].displayName;
	}

	// Token: 0x060087F3 RID: 34803 RVA: 0x0033F5D7 File Offset: 0x0033D7D7
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		this.twitchManager = TwitchManager.Current;
	}

	// Token: 0x060087F4 RID: 34804 RVA: 0x0033F5F4 File Offset: 0x0033D7F4
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "complete_icon")
		{
			this.completeIconName = value;
			return true;
		}
		if (name == "incomplete_icon")
		{
			this.incompleteIconName = value;
			return true;
		}
		if (name == "complete_color")
		{
			Color32 color = StringParsers.ParseColor(value);
			this.completeColor = string.Format("{0},{1},{2},{3}", new object[] { color.r, color.g, color.b, color.a });
			this.completeHexColor = Utils.ColorToHex(color);
			return true;
		}
		if (!(name == "incomplete_color"))
		{
			return base.ParseAttribute(name, value);
		}
		Color32 color2 = StringParsers.ParseColor(value);
		this.incompleteColor = string.Format("{0},{1},{2},{3}", new object[] { color2.r, color2.g, color2.b, color2.a });
		this.incompleteHexColor = Utils.ColorToHex(color2);
		return true;
	}

	// Token: 0x04006505 RID: 25861
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchCommandEntry> commandEntries = new List<XUiC_TwitchCommandEntry>();

	// Token: 0x04006506 RID: 25862
	public string completeIconName = "";

	// Token: 0x04006507 RID: 25863
	public string incompleteIconName = "";

	// Token: 0x04006508 RID: 25864
	public string completeHexColor = "FF00FF00";

	// Token: 0x04006509 RID: 25865
	public string incompleteHexColor = "FFB400";

	// Token: 0x0400650A RID: 25866
	public string warningHexColor = "FFFF00FF";

	// Token: 0x0400650B RID: 25867
	public string inactiveHexColor = "888888FF";

	// Token: 0x0400650C RID: 25868
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x0400650D RID: 25869
	public string completeColor = "0,255,0,255";

	// Token: 0x0400650E RID: 25870
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x0400650F RID: 25871
	public string warningColor = "255,255,0,255";

	// Token: 0x04006510 RID: 25872
	public Dictionary<string, List<TwitchAction>> commandLists = new Dictionary<string, List<TwitchAction>>();

	// Token: 0x04006511 RID: 25873
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchActionGroup> commandGroupList = new List<TwitchActionGroup>();

	// Token: 0x04006512 RID: 25874
	[PublicizedFrom(EAccessModifier.Private)]
	public int commandListIndex = -1;

	// Token: 0x04006513 RID: 25875
	public string CurrentKey = "";

	// Token: 0x04006514 RID: 25876
	public string CurrentTitle = "";

	// Token: 0x04006515 RID: 25877
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastUpdate;

	// Token: 0x04006516 RID: 25878
	[PublicizedFrom(EAccessModifier.Private)]
	public float secondRotation = 10f;

	// Token: 0x04006517 RID: 25879
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchManager twitchManager;
}
