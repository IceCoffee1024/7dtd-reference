using System;
using System.Text;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200107C RID: 4220
[Preserve]
public class XUiC_SkillCraftingInfoEntry : XUiController
{
	// Token: 0x17000FBE RID: 4030
	// (set) Token: 0x0600859E RID: 34206 RVA: 0x0032FAAF File Offset: 0x0032DCAF
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

	// Token: 0x17000FBF RID: 4031
	// (get) Token: 0x0600859F RID: 34207 RVA: 0x0032FAC8 File Offset: 0x0032DCC8
	// (set) Token: 0x060085A0 RID: 34208 RVA: 0x0032FAD0 File Offset: 0x0032DCD0
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

	// Token: 0x17000FC0 RID: 4032
	// (get) Token: 0x060085A1 RID: 34209 RVA: 0x0032FAE9 File Offset: 0x0032DCE9
	// (set) Token: 0x060085A2 RID: 34210 RVA: 0x0032FAF1 File Offset: 0x0032DCF1
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

	// Token: 0x17000FC1 RID: 4033
	// (set) Token: 0x060085A3 RID: 34211 RVA: 0x0032FB01 File Offset: 0x0032DD01
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

	// Token: 0x17000FC2 RID: 4034
	// (set) Token: 0x060085A4 RID: 34212 RVA: 0x0032FB1A File Offset: 0x0032DD1A
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

	// Token: 0x060085A5 RID: 34213 RVA: 0x0032FB33 File Offset: 0x0032DD33
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060085A6 RID: 34214 RVA: 0x0032FB3C File Offset: 0x0032DD3C
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

	// Token: 0x060085A7 RID: 34215 RVA: 0x0032FBC4 File Offset: 0x0032DDC4
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

	// Token: 0x060085A8 RID: 34216 RVA: 0x0032E850 File Offset: 0x0032CA50
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
		base.Update(_dt);
	}

	// Token: 0x04006344 RID: 25412
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_bought;

	// Token: 0x04006345 RID: 25413
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_available;

	// Token: 0x04006346 RID: 25414
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_bg_locked;

	// Token: 0x04006347 RID: 25415
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_available;

	// Token: 0x04006348 RID: 25416
	[PublicizedFrom(EAccessModifier.Private)]
	public string color_lbl_locked;

	// Token: 0x04006349 RID: 25417
	[PublicizedFrom(EAccessModifier.Private)]
	public int listIndex;

	// Token: 0x0400634A RID: 25418
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass.DisplayData data;

	// Token: 0x0400634B RID: 25419
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSelected;

	// Token: 0x0400634C RID: 25420
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxEntriesWithoutPaging = 5;

	// Token: 0x0400634D RID: 25421
	[PublicizedFrom(EAccessModifier.Private)]
	public int hiddenEntriesWithPaging = 2;

	// Token: 0x0400634E RID: 25422
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float, bool> attributeSubtractionFormatter = new CachedStringFormatter<string, float, bool>((string _s, float _f, bool _b) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#") + (_b ? "%" : "")));

	// Token: 0x0400634F RID: 25423
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, float> attributeSetValueFormatter = new CachedStringFormatter<string, float>((string _s, float _f) => string.Format("{0}: {1}", _s, _f.ToCultureInvariantString("0.#")));

	// Token: 0x04006350 RID: 25424
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006351 RID: 25425
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor nextdurabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006352 RID: 25426
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder effectsStringBuilder = new StringBuilder();
}
