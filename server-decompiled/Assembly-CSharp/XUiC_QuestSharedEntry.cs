using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FEE RID: 4078
[Preserve]
public class XUiC_QuestSharedEntry : XUiController
{
	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x060080AE RID: 32942 RVA: 0x0030BE90 File Offset: 0x0030A090
	// (set) Token: 0x060080AF RID: 32943 RVA: 0x0030BE98 File Offset: 0x0030A098
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

	// Token: 0x17000F02 RID: 3842
	// (get) Token: 0x060080B0 RID: 32944 RVA: 0x0030BEC4 File Offset: 0x0030A0C4
	// (set) Token: 0x060080B1 RID: 32945 RVA: 0x0030BECC File Offset: 0x0030A0CC
	public XUiC_QuestWindowGroup QuestUIHandler { get; set; }

	// Token: 0x17000F03 RID: 3843
	// (get) Token: 0x060080B2 RID: 32946 RVA: 0x0030BED5 File Offset: 0x0030A0D5
	// (set) Token: 0x060080B3 RID: 32947 RVA: 0x0030BEDD File Offset: 0x0030A0DD
	public bool Tracked { get; set; }

	// Token: 0x060080B4 RID: 32948 RVA: 0x0030BEE8 File Offset: 0x0030A0E8
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

	// Token: 0x060080B5 RID: 32949 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060080B6 RID: 32950 RVA: 0x0030C29B File Offset: 0x0030A49B
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

	// Token: 0x060080B7 RID: 32951 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060080B8 RID: 32952 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060080B9 RID: 32953 RVA: 0x0030C2CC File Offset: 0x0030A4CC
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

	// Token: 0x04005F59 RID: 24409
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005F5A RID: 24410
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005F5B RID: 24411
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedColor;

	// Token: 0x04005F5C RID: 24412
	[PublicizedFrom(EAccessModifier.Private)]
	public string failedIcon;

	// Token: 0x04005F5D RID: 24413
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005F5E RID: 24414
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005F5F RID: 24415
	public new bool Selected;

	// Token: 0x04005F60 RID: 24416
	public bool IsHovered;

	// Token: 0x04005F61 RID: 24417
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F62 RID: 24418
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F65 RID: 24421
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float, string> distanceFormatter = new CachedStringFormatter<float, string>((float _f, string _s) => _f.ToCultureInvariantString("0.0") + " " + _s);

	// Token: 0x04005F66 RID: 24422
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> zerodistanceFormatter = new CachedStringFormatter<string>((string _s) => "0 " + _s);
}
