using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FA0 RID: 4000
[Preserve]
public class XUiC_OptionsSelector : XUiController
{
	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x06007E4D RID: 32333 RVA: 0x002FDDC0 File Offset: 0x002FBFC0
	// (remove) Token: 0x06007E4E RID: 32334 RVA: 0x002FDDF8 File Offset: 0x002FBFF8
	public event XUiEvent_OnOptionSelectionChanged OnSelectionChanged;

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x06007E4F RID: 32335 RVA: 0x002FDE2D File Offset: 0x002FC02D
	// (set) Token: 0x06007E50 RID: 32336 RVA: 0x002FDE35 File Offset: 0x002FC035
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

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x06007E51 RID: 32337 RVA: 0x002FDE45 File Offset: 0x002FC045
	// (set) Token: 0x06007E52 RID: 32338 RVA: 0x002FDE52 File Offset: 0x002FC052
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

	// Token: 0x06007E53 RID: 32339 RVA: 0x002FDE60 File Offset: 0x002FC060
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

	// Token: 0x06007E54 RID: 32340 RVA: 0x002FDF9F File Offset: 0x002FC19F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleSelectionChangedEvent()
	{
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this, this.selectedIndex);
		}
	}

	// Token: 0x06007E55 RID: 32341 RVA: 0x002FDFBC File Offset: 0x002FC1BC
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

	// Token: 0x06007E56 RID: 32342 RVA: 0x002FE061 File Offset: 0x002FC261
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleLeftArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleLeft();
	}

	// Token: 0x06007E57 RID: 32343 RVA: 0x002FE06A File Offset: 0x002FC26A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleRightArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleRight();
	}

	// Token: 0x06007E58 RID: 32344 RVA: 0x002FE074 File Offset: 0x002FC274
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

	// Token: 0x06007E59 RID: 32345 RVA: 0x002FE0CC File Offset: 0x002FC2CC
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

	// Token: 0x06007E5A RID: 32346 RVA: 0x002FE12E File Offset: 0x002FC32E
	public void SetIndex(int newIndex)
	{
		if (this.SelectedIndex != newIndex)
		{
			this.SelectedIndex = newIndex;
			this.HandleSelectionChangedEvent();
		}
	}

	// Token: 0x06007E5B RID: 32347 RVA: 0x002FE146 File Offset: 0x002FC346
	public void ClearItems()
	{
		this.items.Clear();
		this.SelectedIndex = 0;
		this.MaxCount = 0;
		this.IsDirty = true;
	}

	// Token: 0x06007E5C RID: 32348 RVA: 0x002FE168 File Offset: 0x002FC368
	public int AddItem(string item)
	{
		this.items.Add(item);
		this.MaxCount = this.items.Count;
		this.IsDirty = true;
		return this.items.Count - 1;
	}

	// Token: 0x04005D92 RID: 23954
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController leftArrow;

	// Token: 0x04005D93 RID: 23955
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController rightArrow;

	// Token: 0x04005D94 RID: 23956
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController textArea;

	// Token: 0x04005D95 RID: 23957
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblTitle;

	// Token: 0x04005D96 RID: 23958
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblSelected;

	// Token: 0x04005D97 RID: 23959
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView clickable;

	// Token: 0x04005D98 RID: 23960
	public XUiC_OptionsSelector.BoundsHandlingTypes BoundsHandling = XUiC_OptionsSelector.BoundsHandlingTypes.Wrap;

	// Token: 0x04005D99 RID: 23961
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = 1;

	// Token: 0x04005D9A RID: 23962
	public int MaxCount = 1;

	// Token: 0x04005D9B RID: 23963
	public int Step = 1;

	// Token: 0x04005D9C RID: 23964
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> items = new List<string>();

	// Token: 0x02000FA1 RID: 4001
	public enum BoundsHandlingTypes
	{
		// Token: 0x04005D9E RID: 23966
		Clamp,
		// Token: 0x04005D9F RID: 23967
		Wrap
	}
}
