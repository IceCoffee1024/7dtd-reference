using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FD5 RID: 4053
[Preserve]
public class XUiC_QuestSharedEntry : XUiController
{
	// Token: 0x17000EC8 RID: 3784
	// (get) Token: 0x06007FBC RID: 32700 RVA: 0x0030765C File Offset: 0x0030585C
	// (set) Token: 0x06007FBD RID: 32701 RVA: 0x00307664 File Offset: 0x00305864
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

	// Token: 0x17000EC9 RID: 3785
	// (get) Token: 0x06007FBE RID: 32702 RVA: 0x00307690 File Offset: 0x00305890
	// (set) Token: 0x06007FBF RID: 32703 RVA: 0x00307698 File Offset: 0x00305898
	public XUiC_QuestWindowGroup QuestUIHandler { get; set; }

	// Token: 0x17000ECA RID: 3786
	// (get) Token: 0x06007FC0 RID: 32704 RVA: 0x003076A1 File Offset: 0x003058A1
	// (set) Token: 0x06007FC1 RID: 32705 RVA: 0x003076A9 File Offset: 0x003058A9
	public bool Tracked { get; set; }

	// Token: 0x06007FC2 RID: 32706 RVA: 0x003076B4 File Offset: 0x003058B4
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

	// Token: 0x06007FC3 RID: 32707 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06007FC4 RID: 32708 RVA: 0x00307A67 File Offset: 0x00305C67
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

	// Token: 0x06007FC5 RID: 32709 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x06007FC6 RID: 32710 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007FC7 RID: 32711 RVA: 0x00307A98 File Offset: 0x00305C98
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

	// Token: 0x04005E92 RID: 24210
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005E93 RID: 24211
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005E94 RID: 24212
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedColor;

	// Token: 0x04005E95 RID: 24213
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedIcon;

	// Token: 0x04005E96 RID: 24214
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005E97 RID: 24215
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005E98 RID: 24216
	public new bool Selected;

	// Token: 0x04005E99 RID: 24217
	public bool IsHovered;

	// Token: 0x04005E9A RID: 24218
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005E9B RID: 24219
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005E9E RID: 24222
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float, string> distanceFormatter = new CachedStringFormatter<float, string>((float _f, string _s) => _f.ToCultureInvariantString("0.0") + " " + _s);

	// Token: 0x04005E9F RID: 24223
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> zerodistanceFormatter = new CachedStringFormatter<string>((string _s) => "0 " + _s);
}
