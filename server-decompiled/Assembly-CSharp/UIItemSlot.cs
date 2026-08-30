using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003D RID: 61
public abstract class UIItemSlot : MonoBehaviour
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000162 RID: 354
	public abstract InvGameItem observedItem
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x06000163 RID: 355
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract InvGameItem Replace(InvGameItem item);

	// Token: 0x06000164 RID: 356 RVA: 0x0000ED88 File Offset: 0x0000CF88
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTooltip(bool show)
	{
		InvGameItem invGameItem = (show ? this.mItem : null);
		if (invGameItem != null)
		{
			InvBaseItem baseItem = invGameItem.baseItem;
			if (baseItem != null)
			{
				string text = string.Concat(new string[]
				{
					"[",
					NGUIText.EncodeColor(invGameItem.color),
					"]",
					invGameItem.name,
					"[-]\n"
				});
				text = string.Concat(new string[]
				{
					text,
					"[AFAFAF]Level ",
					invGameItem.itemLevel.ToString(),
					" ",
					baseItem.slot.ToString()
				});
				List<InvStat> list = invGameItem.CalculateStats();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					InvStat invStat = list[i];
					if (invStat.amount != 0)
					{
						if (invStat.amount < 0)
						{
							text = text + "\n[FF0000]" + invStat.amount.ToString();
						}
						else
						{
							text = text + "\n[00FF00]+" + invStat.amount.ToString();
						}
						if (invStat.modifier == InvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + invStat.id.ToString();
						text += "[-]";
					}
					i++;
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.Show(text);
				return;
			}
		}
		UITooltip.Hide();
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000EF12 File Offset: 0x0000D112
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClick()
	{
		if (UIItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
			return;
		}
		if (this.mItem != null)
		{
			UIItemSlot.mDraggedItem = this.Replace(null);
			if (UIItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000EF50 File Offset: 0x0000D150
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrag(Vector2 delta)
	{
		if (UIItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000EF8C File Offset: 0x0000D18C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrop(GameObject go)
	{
		InvGameItem invGameItem = this.Replace(UIItemSlot.mDraggedItem);
		if (UIItemSlot.mDraggedItem == invGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (invGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIItemSlot.mDraggedItem = invGameItem;
		this.UpdateCursor();
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000EFE4 File Offset: 0x0000D1E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateCursor()
	{
		if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
		{
			UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas as INGUIAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
			return;
		}
		UICursor.Clear();
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000F034 File Offset: 0x0000D234
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		InvGameItem observedItem = this.observedItem;
		if (this.mItem != observedItem)
		{
			this.mItem = observedItem;
			InvBaseItem invBaseItem = ((observedItem != null) ? observedItem.baseItem : null);
			if (this.label != null)
			{
				string text = ((observedItem != null) ? observedItem.name : null);
				if (string.IsNullOrEmpty(this.mText))
				{
					this.mText = this.label.text;
				}
				this.label.text = ((text != null) ? text : this.mText);
			}
			if (this.icon != null)
			{
				if (invBaseItem == null || invBaseItem.iconAtlas == null)
				{
					this.icon.enabled = false;
				}
				else
				{
					this.icon.atlas = invBaseItem.iconAtlas as INGUIAtlas;
					this.icon.spriteName = invBaseItem.iconName;
					this.icon.enabled = true;
					this.icon.MakePixelPerfect();
				}
			}
			if (this.background != null)
			{
				this.background.color = ((observedItem != null) ? observedItem.color : Color.white);
			}
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000F14C File Offset: 0x0000D34C
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIItemSlot()
	{
	}

	// Token: 0x040001F2 RID: 498
	public UISprite icon;

	// Token: 0x040001F3 RID: 499
	public UIWidget background;

	// Token: 0x040001F4 RID: 500
	public UILabel label;

	// Token: 0x040001F5 RID: 501
	public AudioClip grabSound;

	// Token: 0x040001F6 RID: 502
	public AudioClip placeSound;

	// Token: 0x040001F7 RID: 503
	public AudioClip errorSound;

	// Token: 0x040001F8 RID: 504
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public InvGameItem mItem;

	// Token: 0x040001F9 RID: 505
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string mText = "";

	// Token: 0x040001FA RID: 506
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static InvGameItem mDraggedItem;
}
