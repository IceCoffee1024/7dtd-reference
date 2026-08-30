using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000F87 RID: 3975
[Preserve]
public class XUiC_OptionsSelector : XUiController
{
	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06007D5B RID: 32091 RVA: 0x002F95EC File Offset: 0x002F77EC
	// (remove) Token: 0x06007D5C RID: 32092 RVA: 0x002F9624 File Offset: 0x002F7824
	public event XUiEvent_OnOptionSelectionChanged OnSelectionChanged;

	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06007D5D RID: 32093 RVA: 0x002F9659 File Offset: 0x002F7859
	// (set) Token: 0x06007D5E RID: 32094 RVA: 0x002F9661 File Offset: 0x002F7861
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

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06007D5F RID: 32095 RVA: 0x002F9671 File Offset: 0x002F7871
	// (set) Token: 0x06007D60 RID: 32096 RVA: 0x002F967E File Offset: 0x002F787E
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

	// Token: 0x06007D61 RID: 32097 RVA: 0x002F968C File Offset: 0x002F788C
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

	// Token: 0x06007D62 RID: 32098 RVA: 0x002F97CB File Offset: 0x002F79CB
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleSelectionChangedEvent()
	{
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this, this.selectedIndex);
		}
	}

	// Token: 0x06007D63 RID: 32099 RVA: 0x002F97E8 File Offset: 0x002F79E8
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

	// Token: 0x06007D64 RID: 32100 RVA: 0x002F988D File Offset: 0x002F7A8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleLeftArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleLeft();
	}

	// Token: 0x06007D65 RID: 32101 RVA: 0x002F9896 File Offset: 0x002F7A96
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleRightArrowOnPress(XUiController _sender, int _mouseButton)
	{
		this.CycleRight();
	}

	// Token: 0x06007D66 RID: 32102 RVA: 0x002F98A0 File Offset: 0x002F7AA0
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

	// Token: 0x06007D67 RID: 32103 RVA: 0x002F98F8 File Offset: 0x002F7AF8
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

	// Token: 0x06007D68 RID: 32104 RVA: 0x002F995A File Offset: 0x002F7B5A
	public void SetIndex(int newIndex)
	{
		if (this.SelectedIndex != newIndex)
		{
			this.SelectedIndex = newIndex;
			this.HandleSelectionChangedEvent();
		}
	}

	// Token: 0x06007D69 RID: 32105 RVA: 0x002F9972 File Offset: 0x002F7B72
	public void ClearItems()
	{
		this.items.Clear();
		this.SelectedIndex = 0;
		this.MaxCount = 0;
		this.IsDirty = true;
	}

	// Token: 0x06007D6A RID: 32106 RVA: 0x002F9994 File Offset: 0x002F7B94
	public int AddItem(string item)
	{
		this.items.Add(item);
		this.MaxCount = this.items.Count;
		this.IsDirty = true;
		return this.items.Count - 1;
	}

	// Token: 0x04005CCB RID: 23755
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController leftArrow;

	// Token: 0x04005CCC RID: 23756
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController rightArrow;

	// Token: 0x04005CCD RID: 23757
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController textArea;

	// Token: 0x04005CCE RID: 23758
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblTitle;

	// Token: 0x04005CCF RID: 23759
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_Label lblSelected;

	// Token: 0x04005CD0 RID: 23760
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView clickable;

	// Token: 0x04005CD1 RID: 23761
	public XUiC_OptionsSelector.BoundsHandlingTypes BoundsHandling = XUiC_OptionsSelector.BoundsHandlingTypes.Wrap;

	// Token: 0x04005CD2 RID: 23762
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = 1;

	// Token: 0x04005CD3 RID: 23763
	public int MaxCount = 1;

	// Token: 0x04005CD4 RID: 23764
	public int Step = 1;

	// Token: 0x04005CD5 RID: 23765
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> items = new List<string>();

	// Token: 0x02000F88 RID: 3976
	public enum BoundsHandlingTypes
	{
		// Token: 0x04005CD7 RID: 23767
		Clamp,
		// Token: 0x04005CD8 RID: 23768
		Wrap
	}
}
