using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FAF RID: 4015
[Preserve]
public class XUiC_OptionsSelector : XUiController
{
	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06007E5D RID: 32349 RVA: 0x00300E28 File Offset: 0x002FF028
	// (remove) Token: 0x06007E5E RID: 32350 RVA: 0x00300E60 File Offset: 0x002FF060
	public event XUiEvent_OnOptionSelectionChanged OnSelectionChanged;

	// Token: 0x17000EE5 RID: 3813
	// (get) Token: 0x06007E5F RID: 32351 RVA: 0x00300E95 File Offset: 0x002FF095
	// (set) Token: 0x06007E60 RID: 32352 RVA: 0x00300E9D File Offset: 0x002FF09D
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			this.selectedIndex = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x06007E61 RID: 32353 RVA: 0x00300EAD File Offset: 0x002FF0AD
	// (set) Token: 0x06007E62 RID: 32354 RVA: 0x00300EBA File Offset: 0x002FF0BA
	public string Title
	{
		get
		{
			return this.lblTitle.Text;
		}
		set
		{
			this.lblTitle.Text = value;
		}
	}

	// Token: 0x06007E63 RID: 32355 RVA: 0x00300EC8 File Offset: 0x002FF0C8
	public override void Init()
	{
		base.Init();
		this.leftArrow = base.GetChildById("leftArrow");
		this.rightArrow = base.GetChildById("rightArrow");
		this.textArea = base.GetChildById("textArea");
		this.clickable = base.GetChildById("clickable").ViewComponent;
		this.lblSelected = this.textArea.GetChildById("lblText").ViewComponent as XUiV_Label;
		this.lblTitle = base.GetChildById("lblTitle").ViewComponent as XUiV_Label;
		this.leftArrow.OnPress += this.HandleLeftArrowOnPress;
		this.rightArrow.OnPress += this.HandleRightArrowOnPress;
		this.rightArrow.ViewComponent.Position = new Vector2i(base.ViewComponent.Size.x - 30, this.rightArrow.ViewComponent.Position.y);
		this.textArea.ViewComponent.Size = new Vector2i(base.ViewComponent.Size.x - 80, this.textArea.ViewComponent.Size.y);
	}

	// Token: 0x06007E64 RID: 32356 RVA: 0x00301007 File Offset: 0x002FF207
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleSelectionChangedEvent()
	{
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this, this.selectedIndex);
		}
	}

	// Token: 0x06007E65 RID: 32357 RVA: 0x00301024 File Offset: 0x002FF224
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.xui.playerUI.CursorController.navigationTarget == this.clickable)
		{
			XUi.HandlePaging(this.xui, new Func<bool>(this.CycleRight), new Func<bool>(this.CycleLeft), false);
		}
		if (this.IsDirty)
		{
			if (this.items.Count > this.SelectedIndex)
			{
				this.lblSelected.Text = this.items[this.SelectedIndex];
			}
			else
			{
				this.lblSelected.Text = "";
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x06007E66 RID: 32358 RVA: 0x003010C9 File Offset: 0x002FF2C9
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleLeftArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleLeft();
	}

	// Token: 0x06007E67 RID: 32359 RVA: 0x003010D2 File Offset: 0x002FF2D2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleRightArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleRight();
	}

	// Token: 0x06007E68 RID: 32360 RVA: 0x003010DC File Offset: 0x002FF2DC
	public bool CycleLeft()
	{
		this.SelectedIndex -= this.Step;
		if (this.BoundsHandling == XUiC_OptionsSelector.BoundsHandlingTypes.Clamp)
		{
			if (this.SelectedIndex < 0)
			{
				this.SelectedIndex = 0;
				return false;
			}
		}
		else if (this.SelectedIndex < 0)
		{
			this.SelectedIndex = this.MaxCount - 1;
		}
		this.HandleSelectionChangedEvent();
		return true;
	}

	// Token: 0x06007E69 RID: 32361 RVA: 0x00301134 File Offset: 0x002FF334
	public bool CycleRight()
	{
		this.SelectedIndex += this.Step;
		if (this.BoundsHandling == XUiC_OptionsSelector.BoundsHandlingTypes.Clamp)
		{
			if (this.SelectedIndex >= this.MaxCount)
			{
				this.SelectedIndex = this.MaxCount - 1;
				return false;
			}
		}
		else if (this.SelectedIndex >= this.MaxCount)
		{
			this.SelectedIndex = 0;
		}
		this.HandleSelectionChangedEvent();
		return true;
	}

	// Token: 0x06007E6A RID: 32362 RVA: 0x00301196 File Offset: 0x002FF396
	public void SetIndex(int newIndex)
	{
		if (this.SelectedIndex != newIndex)
		{
			this.SelectedIndex = newIndex;
			this.HandleSelectionChangedEvent();
		}
	}

	// Token: 0x06007E6B RID: 32363 RVA: 0x003011AE File Offset: 0x002FF3AE
	public void ClearItems()
	{
		this.items.Clear();
		this.SelectedIndex = 0;
		this.MaxCount = 0;
		this.IsDirty = true;
	}

	// Token: 0x06007E6C RID: 32364 RVA: 0x003011D0 File Offset: 0x002FF3D0
	public int AddItem(string item)
	{
		this.items.Add(item);
		this.MaxCount = this.items.Count;
		this.IsDirty = true;
		return this.items.Count - 1;
	}

	// Token: 0x04005DBB RID: 23995
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController leftArrow;

	// Token: 0x04005DBC RID: 23996
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController rightArrow;

	// Token: 0x04005DBD RID: 23997
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController textArea;

	// Token: 0x04005DBE RID: 23998
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblTitle;

	// Token: 0x04005DBF RID: 23999
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblSelected;

	// Token: 0x04005DC0 RID: 24000
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView clickable;

	// Token: 0x04005DC1 RID: 24001
	public XUiC_OptionsSelector.BoundsHandlingTypes BoundsHandling = XUiC_OptionsSelector.BoundsHandlingTypes.Wrap;

	// Token: 0x04005DC2 RID: 24002
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = 1;

	// Token: 0x04005DC3 RID: 24003
	public int MaxCount = 1;

	// Token: 0x04005DC4 RID: 24004
	public int Step = 1;

	// Token: 0x04005DC5 RID: 24005
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> items = new List<string>();

	// Token: 0x02000FB0 RID: 4016
	public enum BoundsHandlingTypes
	{
		// Token: 0x04005DC7 RID: 24007
		Clamp,
		// Token: 0x04005DC8 RID: 24008
		Wrap
	}
}
