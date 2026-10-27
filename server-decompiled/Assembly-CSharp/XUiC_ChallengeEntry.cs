using System;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF0 RID: 3824
[Preserve]
public class XUiC_ChallengeEntry : XUiController
{
	// Token: 0x17000E24 RID: 3620
	// (get) Token: 0x06007738 RID: 30520 RVA: 0x002CFA1B File Offset: 0x002CDC1B
	// (set) Token: 0x06007739 RID: 30521 RVA: 0x002CFA24 File Offset: 0x002CDC24
	public Challenge Entry
	{
		get
		{
			return this.entry;
		}
		set
		{
			base.ViewComponent.Enabled = value != null;
			this.entry = value;
			this.challengeClass = ((this.entry != null) ? this.entry.ChallengeClass : null);
			if (this.challengeClass != null)
			{
				this.IsChallengeVisible = this.challengeClass.ChallengeGroup.IsVisible(this.entry.Owner.Player);
			}
			else
			{
				this.IsChallengeVisible = true;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x17000E25 RID: 3621
	// (get) Token: 0x0600773A RID: 30522 RVA: 0x002CFAA1 File Offset: 0x002CDCA1
	// (set) Token: 0x0600773B RID: 30523 RVA: 0x002CFAA9 File Offset: 0x002CDCA9
	public XUiC_ChallengeWindowGroup JournalUIHandler { get; set; }

	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x0600773C RID: 30524 RVA: 0x002CFAB2 File Offset: 0x002CDCB2
	// (set) Token: 0x0600773D RID: 30525 RVA: 0x002CFABA File Offset: 0x002CDCBA
	public bool Tracked { get; set; }

	// Token: 0x0600773E RID: 30526 RVA: 0x002CFAC4 File Offset: 0x002CDCC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.entry != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 765459171U)
		{
			if (num != 21245043U)
			{
				if (num != 480848564U)
				{
					if (num == 765459171U)
					{
						if (bindingName == "rowstatecolor")
						{
							value = this.rowColor;
							if (flag)
							{
								if (this.Selected)
								{
									value = this.selectedColor;
								}
								else if (this.IsHovered)
								{
									value = this.hoverColor;
								}
							}
							return true;
						}
					}
				}
				else if (bindingName == "fillamount")
				{
					value = ((flag && this.entry.IsActive && this.IsChallengeVisible) ? this.entry.FillAmount.ToString() : "0");
					return true;
				}
			}
			else if (bindingName == "iconname")
			{
				value = "";
				if (flag)
				{
					value = (this.IsChallengeVisible ? this.challengeClass.Icon : "ui_game_symbol_other");
				}
				return true;
			}
		}
		else if (num <= 1566407741U)
		{
			if (num != 1149663213U)
			{
				if (num == 1566407741U)
				{
					if (bindingName == "hasentry")
					{
						value = (flag ? "true" : "false");
						return true;
					}
				}
			}
			else if (bindingName == "tracked")
			{
				value = (flag ? this.entry.IsTracked.ToString() : "false");
				return true;
			}
		}
		else if (num != 2240895362U)
		{
			if (num == 3106195591U)
			{
				if (bindingName == "iconcolor")
				{
					value = "255,255,255,255";
					if (flag)
					{
						if (!this.IsChallengeVisible)
						{
							value = this.disabledColor;
						}
						else if (this.entry.ChallengeState == Challenge.ChallengeStates.Redeemed)
						{
							value = this.disabledColor;
						}
						else if (this.entry.ReadyToComplete)
						{
							value = this.redeemableColor;
						}
						else if (this.entry.IsTracked)
						{
							value = this.trackedColor;
						}
						else if (this.IsHovered)
						{
							value = this.hoverColor;
						}
						else
						{
							value = this.enabledColor;
						}
					}
					return true;
				}
			}
		}
		else if (bindingName == "fillactive")
		{
			if (flag)
			{
				if (!this.IsChallengeVisible)
				{
					value = "false";
				}
				else
				{
					value = this.entry.IsActive.ToString();
				}
			}
			else
			{
				value = "false";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600773F RID: 30527 RVA: 0x002CFD48 File Offset: 0x002CDF48
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("itemIcon");
		if (childById != null)
		{
			this.itemIconSprite = (XUiV_Sprite)childById.ViewComponent;
			this.iconSize = this.itemIconSprite.Size;
		}
		this.tweenScale = this.itemIconSprite.UiTransform.gameObject.AddComponent<TweenScale>();
		this.IsDirty = true;
	}

	// Token: 0x06007740 RID: 30528 RVA: 0x002CFDAE File Offset: 0x002CDFAE
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		XUiC_ChallengeEntryList.SelectedEntry = this;
	}

	// Token: 0x06007741 RID: 30529 RVA: 0x002CFDBD File Offset: 0x002CDFBD
	public override void OnCursorUnSelected()
	{
		base.OnCursorUnSelected();
		this.Selected = false;
		this.IsDirty = true;
	}

	// Token: 0x06007742 RID: 30530 RVA: 0x002CFDD4 File Offset: 0x002CDFD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		if (!this.IsChallengeVisible)
		{
			this.IsHovered = false;
			return;
		}
		base.OnHovered(_isOver);
		if (this.Entry == null)
		{
			this.IsHovered = false;
			return;
		}
		if (this.Entry != null && !this.IsRedeemBlinking)
		{
			if (_isOver)
			{
				this.tweenScale.from = Vector3.one;
				this.tweenScale.to = Vector3.one * 1.1f;
				this.tweenScale.enabled = true;
				this.tweenScale.duration = 0.5f;
			}
			else
			{
				this.tweenScale.from = Vector3.one * 1.1f;
				this.tweenScale.to = Vector3.one;
				this.tweenScale.enabled = true;
				this.tweenScale.duration = 0.5f;
			}
		}
		if (this.IsHovered != _isOver)
		{
			this.IsHovered = _isOver;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007743 RID: 30531 RVA: 0x002CFEC8 File Offset: 0x002CE0C8
	public override void Update(float _dt)
	{
		if (this.IsDirty || (this.entry != null && this.entry.NeedsUIUpdate))
		{
			if (this.challengeClass != null)
			{
				this.IsChallengeVisible = this.challengeClass.ChallengeGroup.IsVisible(this.entry.Owner.Player);
			}
			else
			{
				this.IsChallengeVisible = true;
			}
			base.ViewComponent.SoundPlayOnHover = this.IsChallengeVisible;
			base.ViewComponent.SoundPlayOnClick = this.IsChallengeVisible;
		}
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
		if (this.IsRedeemBlinking && !this.Selected)
		{
			this.tweenScale.enabled = false;
			float num = Mathf.PingPong(Time.time, 0.5f);
			float num2 = 1f;
			if (num > 0.25f)
			{
				num2 = 1f + num - 0.25f;
			}
			this.itemIconSprite.Size = new Vector2i((int)((float)this.iconSize.x * num2), (int)((float)this.iconSize.y * num2));
			return;
		}
		if (this.Selected)
		{
			this.itemIconSprite.Size = this.iconSize;
		}
	}

	// Token: 0x06007744 RID: 30532 RVA: 0x002CFFF0 File Offset: 0x002CE1F0
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
		if (!(name == "selected_color"))
		{
			return base.ParseAttribute(name, value);
		}
		this.selectedColor = value;
		return true;
	}

	// Token: 0x04005842 RID: 22594
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005843 RID: 22595
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005844 RID: 22596
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005845 RID: 22597
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005846 RID: 22598
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectedColor;

	// Token: 0x04005847 RID: 22599
	[PublicizedFrom(EAccessModifier.Private)]
	public string trackedColor = "255, 180, 0, 255";

	// Token: 0x04005848 RID: 22600
	[PublicizedFrom(EAccessModifier.Private)]
	public string redeemableColor = "0,255,0,255";

	// Token: 0x04005849 RID: 22601
	public new bool Selected;

	// Token: 0x0400584A RID: 22602
	public bool IsHovered;

	// Token: 0x0400584B RID: 22603
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x0400584C RID: 22604
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i iconSize;

	// Token: 0x0400584D RID: 22605
	public bool IsRedeemBlinking;

	// Token: 0x0400584E RID: 22606
	public bool IsChallengeVisible;

	// Token: 0x0400584F RID: 22607
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge entry;

	// Token: 0x04005850 RID: 22608
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x04005851 RID: 22609
	[PublicizedFrom(EAccessModifier.Protected)]
	public TweenScale tweenScale;

	// Token: 0x04005853 RID: 22611
	public XUiC_ChallengeEntryList Owner;
}
