using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FFD RID: 4093
[Preserve]
public class XUiC_QuestSharedEntry : XUiController
{
	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x060080BD RID: 32957 RVA: 0x0030EE78 File Offset: 0x0030D078
	// (set) Token: 0x060080BE RID: 32958 RVA: 0x0030EE80 File Offset: 0x0030D080
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

	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x060080BF RID: 32959 RVA: 0x0030EEAC File Offset: 0x0030D0AC
	// (set) Token: 0x060080C0 RID: 32960 RVA: 0x0030EEB4 File Offset: 0x0030D0B4
	public XUiC_QuestWindowGroup QuestUIHandler { get; set; }

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x060080C1 RID: 32961 RVA: 0x0030EEBD File Offset: 0x0030D0BD
	// (set) Token: 0x060080C2 RID: 32962 RVA: 0x0030EEC5 File Offset: 0x0030D0C5
	public bool Tracked { get; set; }

	// Token: 0x060080C3 RID: 32963 RVA: 0x0030EED0 File Offset: 0x0030D0D0
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

	// Token: 0x060080C4 RID: 32964 RVA: 0x002E0AD8 File Offset: 0x002DECD8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060080C5 RID: 32965 RVA: 0x0030F283 File Offset: 0x0030D483
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

	// Token: 0x060080C6 RID: 32966 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060080C7 RID: 32967 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060080C8 RID: 32968 RVA: 0x0030F2B4 File Offset: 0x0030D4B4
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

	// Token: 0x04005F82 RID: 24450
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005F83 RID: 24451
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005F84 RID: 24452
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedColor;

	// Token: 0x04005F85 RID: 24453
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedIcon;

	// Token: 0x04005F86 RID: 24454
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005F87 RID: 24455
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005F88 RID: 24456
	public new bool Selected;

	// Token: 0x04005F89 RID: 24457
	public bool IsHovered;

	// Token: 0x04005F8A RID: 24458
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F8B RID: 24459
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F8E RID: 24462
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float, string> distanceFormatter = new CachedStringFormatter<float, string>((float _f, string _s) => _f.ToCultureInvariantString("0.0") + " " + _s);

	// Token: 0x04005F8F RID: 24463
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> zerodistanceFormatter = new CachedStringFormatter<string>((string _s) => "0 " + _s);
}
