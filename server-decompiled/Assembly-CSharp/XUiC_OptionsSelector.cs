using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FBF RID: 4031
[Preserve]
public class XUiC_OptionsSelector : XUiController
{
	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06007EAC RID: 32428 RVA: 0x00302944 File Offset: 0x00300B44
	// (remove) Token: 0x06007EAD RID: 32429 RVA: 0x0030297C File Offset: 0x00300B7C
	public event XUiEvent_OnOptionSelectionChanged OnSelectionChanged;

	// Token: 0x17000EF0 RID: 3824
	// (get) Token: 0x06007EAE RID: 32430 RVA: 0x003029B1 File Offset: 0x00300BB1
	// (set) Token: 0x06007EAF RID: 32431 RVA: 0x003029B9 File Offset: 0x00300BB9
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

	// Token: 0x17000EF1 RID: 3825
	// (get) Token: 0x06007EB0 RID: 32432 RVA: 0x003029C9 File Offset: 0x00300BC9
	// (set) Token: 0x06007EB1 RID: 32433 RVA: 0x003029D6 File Offset: 0x00300BD6
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

	// Token: 0x06007EB2 RID: 32434 RVA: 0x003029E4 File Offset: 0x00300BE4
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

	// Token: 0x06007EB3 RID: 32435 RVA: 0x00302B23 File Offset: 0x00300D23
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleSelectionChangedEvent()
	{
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this, this.selectedIndex);
		}
	}

	// Token: 0x06007EB4 RID: 32436 RVA: 0x00302B40 File Offset: 0x00300D40
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

	// Token: 0x06007EB5 RID: 32437 RVA: 0x00302BE5 File Offset: 0x00300DE5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleLeftArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleLeft();
	}

	// Token: 0x06007EB6 RID: 32438 RVA: 0x00302BEE File Offset: 0x00300DEE
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleRightArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleRight();
	}

	// Token: 0x06007EB7 RID: 32439 RVA: 0x00302BF8 File Offset: 0x00300DF8
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

	// Token: 0x06007EB8 RID: 32440 RVA: 0x00302C50 File Offset: 0x00300E50
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

	// Token: 0x06007EB9 RID: 32441 RVA: 0x00302CB2 File Offset: 0x00300EB2
	public void SetIndex(int newIndex)
	{
		if (this.SelectedIndex != newIndex)
		{
			this.SelectedIndex = newIndex;
			this.HandleSelectionChangedEvent();
		}
	}

	// Token: 0x06007EBA RID: 32442 RVA: 0x00302CCA File Offset: 0x00300ECA
	public void ClearItems()
	{
		this.items.Clear();
		this.SelectedIndex = 0;
		this.MaxCount = 0;
		this.IsDirty = true;
	}

	// Token: 0x06007EBB RID: 32443 RVA: 0x00302CEC File Offset: 0x00300EEC
	public int AddItem(string item)
	{
		this.items.Add(item);
		this.MaxCount = this.items.Count;
		this.IsDirty = true;
		return this.items.Count - 1;
	}

	// Token: 0x04005DF6 RID: 24054
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController leftArrow;

	// Token: 0x04005DF7 RID: 24055
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController rightArrow;

	// Token: 0x04005DF8 RID: 24056
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController textArea;

	// Token: 0x04005DF9 RID: 24057
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblTitle;

	// Token: 0x04005DFA RID: 24058
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblSelected;

	// Token: 0x04005DFB RID: 24059
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView clickable;

	// Token: 0x04005DFC RID: 24060
	public XUiC_OptionsSelector.BoundsHandlingTypes BoundsHandling = XUiC_OptionsSelector.BoundsHandlingTypes.Wrap;

	// Token: 0x04005DFD RID: 24061
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = 1;

	// Token: 0x04005DFE RID: 24062
	public int MaxCount = 1;

	// Token: 0x04005DFF RID: 24063
	public int Step = 1;

	// Token: 0x04005E00 RID: 24064
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> items = new List<string>();

	// Token: 0x02000FC0 RID: 4032
	public enum BoundsHandlingTypes
	{
		// Token: 0x04005E02 RID: 24066
		Clamp,
		// Token: 0x04005E03 RID: 24067
		Wrap
	}
}
