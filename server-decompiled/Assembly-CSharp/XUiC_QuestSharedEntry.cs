using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200100D RID: 4109
[Preserve]
public class XUiC_QuestSharedEntry : XUiController
{
	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x0600810C RID: 33036 RVA: 0x00310994 File Offset: 0x0030EB94
	// (set) Token: 0x0600810D RID: 33037 RVA: 0x0031099C File Offset: 0x0030EB9C
	public Quest Quest
	{
		get
		{
			return this.quest;
		}
		set
		{
			this.quest = value;
			this.questClass = ((value != null) ? QuestClass.GetQuest(this.quest.ID) : null);
			this.IsDirty = true;
		}
	}

	// Token: 0x17000F34 RID: 3892
	// (get) Token: 0x0600810E RID: 33038 RVA: 0x003109C8 File Offset: 0x0030EBC8
	// (set) Token: 0x0600810F RID: 33039 RVA: 0x003109D0 File Offset: 0x0030EBD0
	public XUiC_QuestWindowGroup QuestUIHandler { get; set; }

	// Token: 0x17000F35 RID: 3893
	// (get) Token: 0x06008110 RID: 33040 RVA: 0x003109D9 File Offset: 0x0030EBD9
	// (set) Token: 0x06008111 RID: 33041 RVA: 0x003109E1 File Offset: 0x0030EBE1
	public bool Tracked { get; set; }

	// Token: 0x06008112 RID: 33042 RVA: 0x003109EC File Offset: 0x0030EBEC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.quest != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 783488098U)
		{
			if (num <= 112224632U)
			{
				if (num != 33281100U)
				{
					if (num == 112224632U)
					{
						if (bindingName == "questicon")
						{
							value = "";
							if (flag)
							{
								value = ((this.quest.CurrentState != Quest.QuestState.Failed) ? this.questClass.Icon : this.failedIcon);
							}
							return true;
						}
					}
				}
				else if (bindingName == "istracking")
				{
					value = (flag ? this.quest.Tracked.ToString() : "false");
					return true;
				}
			}
			else if (num != 765459171U)
			{
				if (num == 783488098U)
				{
					if (bindingName == "distance")
					{
						if (flag && this.quest.Active && this.quest.HasPosition)
						{
							Vector3 position = this.quest.Position;
							Vector3 position2 = this.xui.playerUI.entityPlayer.GetPosition();
							position.y = 0f;
							position2.y = 0f;
							float num2 = (position - position2).magnitude;
							float num3 = num2;
							string text = "m";
							if (num2 >= 1000f)
							{
								num2 /= 1000f;
								text = "km";
							}
							MapObjectTreasureChest mapObjectTreasureChest = this.quest.MapObject as MapObjectTreasureChest;
							if (mapObjectTreasureChest != null)
							{
								float num4 = (float)mapObjectTreasureChest.DefaultRadius;
								if (num3 < num4)
								{
									num4 = EffectManager.GetValue(PassiveEffects.TreasureRadius, null, num4, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
									num4 = Mathf.Clamp(num4, 0f, num4);
									if (num3 < num4)
									{
										value = this.zerodistanceFormatter.Format(text);
									}
								}
								else
								{
									value = this.distanceFormatter.Format(num2, text);
								}
							}
							else
							{
								value = this.distanceFormatter.Format(num2, text);
							}
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "rowstatecolor")
			{
				value = (this.Selected ? "255,255,255,255" : (this.IsHovered ? this.hoverColor : this.rowColor));
				return true;
			}
		}
		else if (num <= 2730462270U)
		{
			if (num != 1656712805U)
			{
				if (num == 2730462270U)
				{
					if (bindingName == "questname")
					{
						value = (flag ? this.questClass.Name : "");
						return true;
					}
				}
			}
			else if (bindingName == "rowstatesprite")
			{
				value = (this.Selected ? "ui_game_select_row" : "menu_empty");
				return true;
			}
		}
		else if (num != 3106195591U)
		{
			if (num == 3644377122U)
			{
				if (bindingName == "textstatecolor")
				{
					value = "255,255,255,255";
					if (flag)
					{
						if (this.quest.CurrentState == Quest.QuestState.InProgress)
						{
							value = this.enabledColor;
						}
						else
						{
							value = this.disabledColor;
						}
					}
					return true;
				}
			}
		}
		else if (bindingName == "iconcolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				switch (this.quest.CurrentState)
				{
				case Quest.QuestState.InProgress:
					value = this.enabledColor;
					break;
				case Quest.QuestState.Completed:
					value = this.disabledColor;
					break;
				case Quest.QuestState.Failed:
					value = this.failedColor;
					break;
				}
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008113 RID: 33043 RVA: 0x002E23C8 File Offset: 0x002E05C8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06008114 RID: 33044 RVA: 0x00310D9F File Offset: 0x0030EF9F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.Quest == null)
		{
			this.IsHovered = false;
			return;
		}
		if (this.IsHovered != _isOver)
		{
			this.IsHovered = _isOver;
			base.RefreshBindings();
		}
	}

	// Token: 0x06008115 RID: 33045 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06008116 RID: 33046 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06008117 RID: 33047 RVA: 0x00310DD0 File Offset: 0x0030EFD0
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "enabled_color")
		{
			this.enabledColor = value;
			return true;
		}
		if (name == "disabled_color")
		{
			this.disabledColor = value;
			return true;
		}
		if (name == "failed_color")
		{
			this.failedColor = value;
			return true;
		}
		if (name == "row_color")
		{
			this.rowColor = value;
			return true;
		}
		if (name == "hover_color")
		{
			this.hoverColor = value;
			return true;
		}
		if (!(name == "failed_icon"))
		{
			return base.ParseAttribute(name, value);
		}
		this.failedIcon = value;
		return true;
	}

	// Token: 0x04005FBD RID: 24509
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005FBE RID: 24510
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005FBF RID: 24511
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedColor;

	// Token: 0x04005FC0 RID: 24512
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedIcon;

	// Token: 0x04005FC1 RID: 24513
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005FC2 RID: 24514
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005FC3 RID: 24515
	public new bool Selected;

	// Token: 0x04005FC4 RID: 24516
	public bool IsHovered;

	// Token: 0x04005FC5 RID: 24517
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005FC6 RID: 24518
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005FC9 RID: 24521
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float, string> distanceFormatter = new CachedStringFormatter<float, string>((float _f, string _s) => _f.ToCultureInvariantString("0.0") + " " + _s);

	// Token: 0x04005FCA RID: 24522
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> zerodistanceFormatter = new CachedStringFormatter<string>((string _s) => "0 " + _s);
}
