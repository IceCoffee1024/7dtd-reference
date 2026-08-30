using System;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC1 RID: 3777
[Preserve]
public class XUiC_ChallengeEntry : XUiController
{
	// Token: 0x17000DE9 RID: 3561
	// (get) Token: 0x06007676 RID: 30326 RVA: 0x002C7BC7 File Offset: 0x002C5DC7
	// (set) Token: 0x06007677 RID: 30327 RVA: 0x002C7BD0 File Offset: 0x002C5DD0
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

	// Token: 0x17000DEA RID: 3562
	// (get) Token: 0x06007678 RID: 30328 RVA: 0x002C7C4D File Offset: 0x002C5E4D
	// (set) Token: 0x06007679 RID: 30329 RVA: 0x002C7C55 File Offset: 0x002C5E55
	public XUiC_ChallengeWindowGroup JournalUIHandler { get; set; }

	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x0600767A RID: 30330 RVA: 0x002C7C5E File Offset: 0x002C5E5E
	// (set) Token: 0x0600767B RID: 30331 RVA: 0x002C7C66 File Offset: 0x002C5E66
	public bool Tracked { get; set; }

	// Token: 0x0600767C RID: 30332 RVA: 0x002C7C70 File Offset: 0x002C5E70
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

	// Token: 0x0600767D RID: 30333 RVA: 0x002C7EF4 File Offset: 0x002C60F4
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

	// Token: 0x0600767E RID: 30334 RVA: 0x002C7F5A File Offset: 0x002C615A
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		XUiC_ChallengeEntryList.SelectedEntry = this;
	}

	// Token: 0x0600767F RID: 30335 RVA: 0x002C7F69 File Offset: 0x002C6169
	public override void OnCursorUnSelected()
	{
		base.OnCursorUnSelected();
		this.Selected = false;
		this.IsDirty = true;
	}

	// Token: 0x06007680 RID: 30336 RVA: 0x002C7F80 File Offset: 0x002C6180
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

	// Token: 0x06007681 RID: 30337 RVA: 0x002C8074 File Offset: 0x002C6274
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

	// Token: 0x06007682 RID: 30338 RVA: 0x002C819C File Offset: 0x002C639C
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

	// Token: 0x04005740 RID: 22336
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x04005741 RID: 22337
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x04005742 RID: 22338
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005743 RID: 22339
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005744 RID: 22340
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectedColor;

	// Token: 0x04005745 RID: 22341
	[PublicizedFrom(EAccessModifier.Private)]
	public string trackedColor = "255, 180, 0, 255";

	// Token: 0x04005746 RID: 22342
	[PublicizedFrom(EAccessModifier.Private)]
	public string redeemableColor = "0,255,0,255";

	// Token: 0x04005747 RID: 22343
	public new bool Selected;

	// Token: 0x04005748 RID: 22344
	public bool IsHovered;

	// Token: 0x04005749 RID: 22345
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x0400574A RID: 22346
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i iconSize;

	// Token: 0x0400574B RID: 22347
	public bool IsRedeemBlinking;

	// Token: 0x0400574C RID: 22348
	public bool IsChallengeVisible;

	// Token: 0x0400574D RID: 22349
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge entry;

	// Token: 0x0400574E RID: 22350
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x0400574F RID: 22351
	[PublicizedFrom(EAccessModifier.Protected)]
	public TweenScale tweenScale;

	// Token: 0x04005751 RID: 22353
	public XUiC_ChallengeEntryList Owner;
}
