using System;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001042 RID: 4162
[Preserve]
public class XUiC_SkillCraftingInfoEntry : XUiController
{
	// Token: 0x17000F44 RID: 3908
	// (set) Token: 0x06008414 RID: 33812 RVA: 0x00325867 File Offset: 0x00323A67
	public int ListIndex
	{
		set
		{
			if (value != this.listIndex)
			{
				this.listIndex = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000F45 RID: 3909
	// (get) Token: 0x06008415 RID: 33813 RVA: 0x00325880 File Offset: 0x00323A80
	// (set) Token: 0x06008416 RID: 33814 RVA: 0x00325888 File Offset: 0x00323A88
	public ProgressionClass.DisplayData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			if (value != this.data)
			{
				this.data = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000F46 RID: 3910
	// (get) Token: 0x06008417 RID: 33815 RVA: 0x003258A1 File Offset: 0x00323AA1
	// (set) Token: 0x06008418 RID: 33816 RVA: 0x003258A9 File Offset: 0x00323AA9
	public bool IsSelected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			this.isSelected = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000F47 RID: 3911
	// (set) Token: 0x06008419 RID: 33817 RVA: 0x003258B9 File Offset: 0x00323AB9
	public int MaxEntriesWithoutPaging
	{
		set
		{
			if (this.maxEntriesWithoutPaging != value)
			{
				this.maxEntriesWithoutPaging = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000F48 RID: 3912
	// (set) Token: 0x0600841A RID: 33818 RVA: 0x003258D2 File Offset: 0x00323AD2
	public int HiddenEntriesWithPaging
	{
		set
		{
			if (this.hiddenEntriesWithPaging != value)
			{
				this.hiddenEntriesWithPaging = value;
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x0600841B RID: 33819 RVA: 0x003258EB File Offset: 0x00323AEB
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600841C RID: 33820 RVA: 0x003258F4 File Offset: 0x00323AF4
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "color_bg_bought")
		{
			this.color_bg_bought = _value;
			return true;
		}
		if (_name == "color_bg_available")
		{
			this.color_bg_available = _value;
			return true;
		}
		if (_name == "color_bg_locked")
		{
			this.color_bg_locked = _value;
			return true;
		}
		if (_name == "color_lbl_available")
		{
			this.color_lbl_available = _value;
			return true;
		}
		if (!(_name == "color_lbl_locked"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.color_lbl_locked = _value;
		return true;
	}

	// Token: 0x0600841D RID: 33821 RVA: 0x0032597C File Offset: 0x00323B7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		bool flag = this.data != null;
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2155443558U)
		{
			if (num <= 375292317U)
			{
				if (num <= 94814074U)
				{
					if (num != 12870938U)
					{
						if (num == 94814074U)
						{
							if (_bindingName == "nextqualitytext")
							{
								if (!flag)
								{
									_value = "";
									return true;
								}
								int num2 = this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level);
								if (num2 < this.data.QualityStarts.Length + 1)
								{
									num2++;
								}
								_value = num2.ToString();
								return true;
							}
						}
					}
					else if (_bindingName == "color_fg")
					{
						_value = this.color_lbl_available;
						if (flag)
						{
							_value = ((this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level) == 0) ? this.color_lbl_locked : this.color_lbl_available);
						}
						return true;
					}
				}
				else if (num != 306828118U)
				{
					if (num != 372648664U)
					{
						if (num == 375292317U)
						{
							if (_bindingName == "showquality")
							{
								_value = (flag ? (this.data.HasQuality && this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level) > 0).ToString() : "false");
								return true;
							}
						}
					}
					else if (_bindingName == "show_selected")
					{
						_value = this.isSelected.ToString();
						return true;
					}
				}
				else if (_bindingName == "currentqualitycolor")
				{
					if (!flag)
					{
						_value = "255,255,255,255";
						return true;
					}
					Color32 color = QualityInfo.GetTierColor(this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level));
					_value = this.durabilitycolorFormatter.Format(color);
					return true;
				}
			}
			else if (num <= 859026113U)
			{
				if (num != 850736759U)
				{
					if (num == 859026113U)
					{
						if (_bindingName == "notcomplete")
						{
							_value = (flag ? (!this.data.IsComplete(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level)).ToString() : "false");
							return true;
						}
					}
				}
				else if (_bindingName == "nextpoints")
				{
					_value = (flag ? string.Format("{0}/{1}", entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level, this.data.GetNextPoints(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level).ToString()) : "");
					return true;
				}
			}
			else if (num != 1566407741U)
			{
				if (num != 1701888201U)
				{
					if (num == 2155443558U)
					{
						if (_bindingName == "nothiddenbypager")
						{
							_value = "true";
							return true;
						}
					}
				}
				else if (_bindingName == "itemcolor")
				{
					_value = "255,255,255,255";
					return true;
				}
			}
			else if (_bindingName == "hasentry")
			{
				_value = flag.ToString();
				return true;
			}
		}
		else if (num <= 3708628627U)
		{
			if (num <= 3185987134U)
			{
				if (num != 2842844659U)
				{
					if (num == 3185987134U)
					{
						if (_bindingName == "text")
						{
							_value = (flag ? this.data.GetName(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level) : "");
							return true;
						}
					}
				}
				else if (_bindingName == "showcomplete")
				{
					_value = (flag ? this.data.IsComplete(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level).ToString() : "false");
					return true;
				}
			}
			else if (num != 3263401906U)
			{
				if (num != 3503204070U)
				{
					if (num == 3708628627U)
					{
						if (_bindingName == "itemicon")
						{
							_value = (flag ? this.data.GetIcon(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level) : "");
							return true;
						}
					}
				}
				else if (_bindingName == "color_bg")
				{
					_value = this.color_bg_available;
					if (flag)
					{
						_value = ((this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level) == 0) ? this.color_bg_locked : this.color_bg_available);
					}
					return true;
				}
			}
			else if (_bindingName == "notcompletequality")
			{
				_value = (flag ? (this.data.HasQuality && !this.data.IsComplete(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level)).ToString() : "false");
				return true;
			}
		}
		else if (num <= 3933605619U)
		{
			if (num != 3924526227U)
			{
				if (num == 3933605619U)
				{
					if (_bindingName == "notcompletenoquality")
					{
						_value = (flag ? (!this.data.HasQuality && !this.data.IsComplete(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level)).ToString() : "false");
						return true;
					}
				}
			}
			else if (_bindingName == "showlock")
			{
				_value = (flag ? (this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level) == 0).ToString() : "false");
				return true;
			}
		}
		else if (num != 4168931239U)
		{
			if (num != 4220565952U)
			{
				if (num == 4277871100U)
				{
					if (_bindingName == "nextqualitycolor")
					{
						if (!flag)
						{
							_value = "";
							return true;
						}
						int num3 = this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level);
						if (num3 < this.data.QualityStarts.Length + 1)
						{
							num3++;
						}
						_value = this.nextdurabilitycolorFormatter.Format(QualityInfo.GetTierColor(num3));
						return true;
					}
				}
			}
			else if (_bindingName == "currentqualitytext")
			{
				_value = (flag ? this.data.GetQualityLevel(entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level).ToString() : "");
				return true;
			}
		}
		else if (_bindingName == "iconatlas")
		{
			if (!flag)
			{
				_value = "ItemIconAtlas";
			}
			else
			{
				_value = ((entityPlayer.Progression.GetProgressionValue(this.data.Owner.Name).Level >= this.data.QualityStarts[0]) ? "ItemIconAtlas" : "ItemIconAtlasGreyscale");
			}
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x0600841E RID: 33822 RVA: 0x00324608 File Offset: 0x00322808
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x040061FA RID: 25082
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x040061FB RID: 25083
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x040061FC RID: 25084
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x040061FD RID: 25085
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x040061FE RID: 25086
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x040061FF RID: 25087
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x04006200 RID: 25088
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData data;

	// Token: 0x04006201 RID: 25089
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSelected;

	// Token: 0x04006202 RID: 25090
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 5;

	// Token: 0x04006203 RID: 25091
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x04006204 RID: 25092
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x04006205 RID: 25093
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x04006206 RID: 25094
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006207 RID: 25095
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor nextdurabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006208 RID: 25096
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
