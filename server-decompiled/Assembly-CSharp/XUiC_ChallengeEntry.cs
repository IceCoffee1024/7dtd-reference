using System;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ED6 RID: 3798
[Preserve]
public class XUiC_ChallengeEntry : XUiController
{
	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x06007717 RID: 30487 RVA: 0x002CB0EB File Offset: 0x002C92EB
	// (set) Token: 0x06007718 RID: 30488 RVA: 0x002CB0F4 File Offset: 0x002C92F4
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

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x06007719 RID: 30489 RVA: 0x002CB171 File Offset: 0x002C9371
	// (set) Token: 0x0600771A RID: 30490 RVA: 0x002CB179 File Offset: 0x002C9379
	public XUiC_ChallengeWindowGroup JournalUIHandler { get; set; }

	// Token: 0x17000E05 RID: 3589
	// (get) Token: 0x0600771B RID: 30491 RVA: 0x002CB182 File Offset: 0x002C9382
	// (set) Token: 0x0600771C RID: 30492 RVA: 0x002CB18A File Offset: 0x002C938A
	public bool Tracked { get; set; }

	// Token: 0x0600771D RID: 30493 RVA: 0x002CB194 File Offset: 0x002C9394
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

	// Token: 0x0600771E RID: 30494 RVA: 0x002CB418 File Offset: 0x002C9618
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

	// Token: 0x0600771F RID: 30495 RVA: 0x002CB47E File Offset: 0x002C967E
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		XUiC_ChallengeEntryList.SelectedEntry = this;
	}

	// Token: 0x06007720 RID: 30496 RVA: 0x002CB48D File Offset: 0x002C968D
	public override void OnCursorUnSelected()
	{
		base.OnCursorUnSelected();
		this.Selected = false;
		this.IsDirty = true;
	}

	// Token: 0x06007721 RID: 30497 RVA: 0x002CB4A4 File Offset: 0x002C96A4
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

	// Token: 0x06007722 RID: 30498 RVA: 0x002CB598 File Offset: 0x002C9798
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

	// Token: 0x06007723 RID: 30499 RVA: 0x002CB6C0 File Offset: 0x002C98C0
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

	// Token: 0x040057E5 RID: 22501
	[PublicizedFrom(EAccessModifier.Private)]
	public string enabledColor;

	// Token: 0x040057E6 RID: 22502
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor;

	// Token: 0x040057E7 RID: 22503
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040057E8 RID: 22504
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040057E9 RID: 22505
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectedColor;

	// Token: 0x040057EA RID: 22506
	[PublicizedFrom(EAccessModifier.Private)]
	public string trackedColor = "255, 180, 0, 255";

	// Token: 0x040057EB RID: 22507
	[PublicizedFrom(EAccessModifier.Private)]
	public string redeemableColor = "0,255,0,255";

	// Token: 0x040057EC RID: 22508
	public new bool Selected;

	// Token: 0x040057ED RID: 22509
	public bool IsHovered;

	// Token: 0x040057EE RID: 22510
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Sprite itemIconSprite;

	// Token: 0x040057EF RID: 22511
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i iconSize;

	// Token: 0x040057F0 RID: 22512
	public bool IsRedeemBlinking;

	// Token: 0x040057F1 RID: 22513
	public bool IsChallengeVisible;

	// Token: 0x040057F2 RID: 22514
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge entry;

	// Token: 0x040057F3 RID: 22515
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challengeClass;

	// Token: 0x040057F4 RID: 22516
	[PublicizedFrom(EAccessModifier.Protected)]
	public TweenScale tweenScale;

	// Token: 0x040057F6 RID: 22518
	public XUiC_ChallengeEntryList Owner;
}
