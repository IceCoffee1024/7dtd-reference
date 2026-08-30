using System;
using UnityEngine;

// Token: 0x0200113B RID: 4411
public class XUiV_FilledSprite : XUiV_Sprite
{
	// Token: 0x1700105F RID: 4191
	// (get) Token: 0x06008C22 RID: 35874 RVA: 0x00351D51 File Offset: 0x0034FF51
	// (set) Token: 0x06008C23 RID: 35875 RVA: 0x000027FC File Offset: 0x000009FC
	public override UIBasicSprite.Type Type
	{
		get
		{
			return this.type;
		}
		set
		{
		}
	}

	// Token: 0x06008C24 RID: 35876 RVA: 0x00351D59 File Offset: 0x0034FF59
	public XUiV_FilledSprite(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008C25 RID: 35877 RVA: 0x00351D64 File Offset: 0x0034FF64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		if (base.applyAtlasAndSprite(false))
		{
			Vector4 border = this.sprite.border;
			this.spriteBorder = new Vector2i(Mathf.RoundToInt(border.x + border.z), Mathf.RoundToInt(border.y + border.w));
		}
		if (this.fillSpritePad)
		{
			XUiUtils.ApplyFillPaddedSprite(this.sprite, this.spriteName);
		}
		this.sprite.centerType = (this.fillCenter ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible);
		int num = ((this.fillDirection == UIBasicSprite.FillDirection.Horizontal) ? Mathf.RoundToInt(this.fillAmount * (float)this.size.x) : this.size.x);
		int num2 = ((this.fillDirection == UIBasicSprite.FillDirection.Vertical) ? Mathf.RoundToInt(this.fillAmount * (float)this.size.y) : this.size.y);
		if (num != this.sprite.width || num2 != this.sprite.height || this.positionDirty)
		{
			this.positionDirty = false;
			this.sprite.SetDimensions(num, num2);
			this.hideFill = (this.fillDirection == UIBasicSprite.FillDirection.Horizontal && num < this.spriteBorder.x) || (this.fillDirection == UIBasicSprite.FillDirection.Vertical && num2 < this.spriteBorder.y);
			if (this.hideFill)
			{
				this.sprite.color = Color.clear;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			if (this.fillDirection == UIBasicSprite.FillDirection.Horizontal)
			{
				if (!this.fillInvert)
				{
					num3 = 0;
					num4 = Mathf.FloorToInt((float)(-(float)this.size.x + num) / 2f);
					num5 = -this.size.x + num;
				}
				else
				{
					num3 = this.size.x - num;
					num4 = Mathf.CeilToInt((float)(this.size.x - num) / 2f);
					num5 = 0;
				}
			}
			else
			{
				if (this.fillDirection != UIBasicSprite.FillDirection.Vertical)
				{
					Log.Warning("[XUi] FilledSprite only allows FillDirections Horizontal and Vertical. On " + base.Controller.GetParentWindow().ID + "." + base.ID);
					return;
				}
				if (!this.fillInvert)
				{
					num6 = 0;
					num7 = Mathf.FloorToInt((float)(-(float)this.size.y + num2) / 2f);
					num8 = -this.size.y + num2;
				}
				else
				{
					num6 = this.size.y - num2;
					num7 = Mathf.CeilToInt((float)(this.size.y - num2) / 2f);
					num8 = 0;
				}
			}
			int num9;
			switch (this.pivot)
			{
			case UIWidget.Pivot.TopLeft:
			case UIWidget.Pivot.Left:
			case UIWidget.Pivot.BottomLeft:
				num9 = num3;
				break;
			case UIWidget.Pivot.Top:
			case UIWidget.Pivot.Center:
			case UIWidget.Pivot.Bottom:
				num9 = num4;
				break;
			case UIWidget.Pivot.TopRight:
			case UIWidget.Pivot.Right:
			case UIWidget.Pivot.BottomRight:
				num9 = num5;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			int num10 = num9;
			switch (this.pivot)
			{
			case UIWidget.Pivot.TopLeft:
			case UIWidget.Pivot.Top:
			case UIWidget.Pivot.TopRight:
				num9 = num8;
				break;
			case UIWidget.Pivot.Left:
			case UIWidget.Pivot.Center:
			case UIWidget.Pivot.Right:
				num9 = num7;
				break;
			case UIWidget.Pivot.BottomLeft:
			case UIWidget.Pivot.Bottom:
			case UIWidget.Pivot.BottomRight:
				num9 = num6;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			int num11 = num9;
			this.positionDirty = false;
			this.uiTransform.localPosition = new Vector3((float)(base.PaddedPosition.x + num10), (float)(base.PaddedPosition.y + num11), 0f);
		}
		if (!this.hideFill)
		{
			this.sprite.color = base.opacityModColor(this.color);
		}
		this.sprite.type = this.type;
		this.sprite.flip = this.flip;
		this.refreshBoxCollider();
	}

	// Token: 0x06008C26 RID: 35878 RVA: 0x00352114 File Offset: 0x00350314
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void refreshBoxCollider()
	{
		float num = (float)this.size.x * 0.5f;
		float num2 = (float)this.size.y * 0.5f;
		float num3;
		float num4;
		switch (this.pivot)
		{
		case UIWidget.Pivot.TopLeft:
			num3 = num;
			num4 = 0f - num2;
			break;
		case UIWidget.Pivot.Top:
			num3 = 0f;
			num4 = 0f - num2;
			break;
		case UIWidget.Pivot.TopRight:
			num3 = 0f - num;
			num4 = 0f - num2;
			break;
		case UIWidget.Pivot.Left:
			num3 = num;
			num4 = 0f;
			break;
		case UIWidget.Pivot.Center:
			num3 = 0f;
			num4 = 0f;
			break;
		case UIWidget.Pivot.Right:
			num3 = 0f - num;
			num4 = 0f;
			break;
		case UIWidget.Pivot.BottomLeft:
			num3 = num;
			num4 = num2;
			break;
		case UIWidget.Pivot.Bottom:
			num3 = 0f;
			num4 = num2;
			break;
		case UIWidget.Pivot.BottomRight:
			num3 = 0f - num;
			num4 = num2;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.collider.center = new Vector3(num3, num4, 0f);
		this.collider.size = new Vector3((float)this.size.x * base.ColliderScale + (float)(2 * base.ColliderPadding.x), (float)this.size.y * base.ColliderScale + (float)(2 * base.ColliderPadding.y), 0f);
	}

	// Token: 0x06008C27 RID: 35879 RVA: 0x00352264 File Offset: 0x00350464
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		base.FillCenter = true;
		this.type = UIBasicSprite.Type.Sliced;
		base.FillDirection = UIBasicSprite.FillDirection.Horizontal;
	}

	// Token: 0x0400671B RID: 26395
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i spriteBorder;

	// Token: 0x0400671C RID: 26396
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hideFill;
}
