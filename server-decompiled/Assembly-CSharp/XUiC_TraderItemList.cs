using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200109E RID: 4254
[Preserve]
public class XUiC_TraderItemList : XUiController
{
	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x060086EF RID: 34543 RVA: 0x00338ECE File Offset: 0x003370CE
	// (set) Token: 0x060086F0 RID: 34544 RVA: 0x00338ED6 File Offset: 0x003370D6
	public ItemStack CurrentItem
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FE5 RID: 4069
	// (get) Token: 0x060086F1 RID: 34545 RVA: 0x00338EDF File Offset: 0x003370DF
	// (set) Token: 0x060086F2 RID: 34546 RVA: 0x00338EE7 File Offset: 0x003370E7
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			this.page = value;
		}
	}

	// Token: 0x17000FE6 RID: 4070
	// (get) Token: 0x060086F3 RID: 34547 RVA: 0x00338EF0 File Offset: 0x003370F0
	// (set) Token: 0x060086F4 RID: 34548 RVA: 0x00338EF8 File Offset: 0x003370F8
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x060086F5 RID: 34549 RVA: 0x00338F01 File Offset: 0x00337101
	// (set) Token: 0x060086F6 RID: 34550 RVA: 0x00338F0C File Offset: 0x0033710C
	public XUiC_TraderItemEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = true;
				for (int i = 0; i < this.indexList.Count; i++)
				{
					if (this.selectedEntry.SlotIndex == this.indexList[i])
					{
						this.selectedIndex = i % this.entryList.Count;
						break;
					}
				}
				this.InfoWindow.ViewComponent.IsVisible = true;
				this.InfoWindow.SetItemStack(this.selectedEntry, false);
				this.CurrentItem = this.selectedEntry.Item;
				return;
			}
			if (this.selectedIndex >= 0)
			{
				if (this.entryList[this.selectedIndex].Item != null)
				{
					this.SelectedEntry = this.entryList[this.selectedIndex];
				}
				else if (this.entryList[0].Item != null)
				{
					this.SelectedEntry = this.entryList[0];
				}
			}
			if (this.SelectedEntry == null)
			{
				this.selectedIndex = -1;
				this.InfoWindow.SetItemStack(null, false);
				this.CurrentItem = null;
			}
		}
	}

	// Token: 0x060086F7 RID: 34551 RVA: 0x00339048 File Offset: 0x00337248
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiController xuiController = this.children[i];
			if (xuiController is XUiC_TraderItemEntry)
			{
				this.entryList.Add((XUiC_TraderItemEntry)xuiController);
			}
		}
		XUiV_Grid xuiV_Grid = (XUiV_Grid)base.ViewComponent;
		if (xuiV_Grid != null)
		{
			this.Length = xuiV_Grid.Columns * xuiV_Grid.Rows;
		}
		this.InfoWindow = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
	}

	// Token: 0x060086F8 RID: 34552 RVA: 0x003390CC File Offset: 0x003372CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TraderItemEntry xuiC_TraderItemEntry = _sender as XUiC_TraderItemEntry;
		if (xuiC_TraderItemEntry != null)
		{
			this.SelectedEntry = xuiC_TraderItemEntry;
			if (InputUtils.ShiftKeyPressed)
			{
				xuiC_TraderItemEntry.InfoWindow.BuySellCounter.SetToMaxCount();
				return;
			}
			xuiC_TraderItemEntry.InfoWindow.BuySellCounter.Count = xuiC_TraderItemEntry.Item.itemValue.ItemClass.EconomicBundleSize;
		}
	}

	// Token: 0x060086F9 RID: 34553 RVA: 0x00339127 File Offset: 0x00337327
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.ClearSelection();
		this.IsDirty = true;
	}

	// Token: 0x060086FA RID: 34554 RVA: 0x00339157 File Offset: 0x00337357
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
		this.ClearSelection();
	}

	// Token: 0x060086FB RID: 34555 RVA: 0x00339180 File Offset: 0x00337380
	public void ClearSelection()
	{
		this.CurrentItem = null;
		this.selectedIndex = -1;
		this.SelectedEntry = null;
		this.InfoWindow.SetItemStack(null, false);
	}

	// Token: 0x060086FC RID: 34556 RVA: 0x003391A4 File Offset: 0x003373A4
	public void SetItems(ItemStack[] _stackList, List<int> _indexList)
	{
		if (_stackList == null)
		{
			return;
		}
		this.items.Clear();
		this.items.AddRange(_stackList);
		this.indexList.Clear();
		this.indexList.AddRange(_indexList);
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		for (int i = 0; i < this.Length; i++)
		{
			int num = i + this.Length * this.page;
			this.entryList[i].OnPress -= this.OnPressEntry;
			this.entryList[i].InfoWindow = childByType;
			if (num < this.items.Count)
			{
				this.entryList[i].SlotIndex = _indexList[num];
				this.entryList[i].Item = _stackList[num];
				this.entryList[i].OnPress += this.OnPressEntry;
				this.entryList[i].ViewComponent.SoundPlayOnClick = true;
			}
			else
			{
				this.entryList[i].Item = null;
				this.entryList[i].ViewComponent.SoundPlayOnClick = false;
			}
		}
		if (this.CurrentItem != null)
		{
			ItemValue itemValue = this.CurrentItem.itemValue;
			XUiC_TraderItemEntry xuiC_TraderItemEntry = this.SelectedEntry;
			ItemValue itemValue2;
			if (xuiC_TraderItemEntry == null)
			{
				itemValue2 = null;
			}
			else
			{
				ItemStack item = xuiC_TraderItemEntry.Item;
				itemValue2 = ((item != null) ? item.itemValue : null);
			}
			if (itemValue.Equals(itemValue2))
			{
				return;
			}
		}
		this.ClearSelection();
	}

	// Token: 0x04006451 RID: 25681
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemEntry selectedEntry;

	// Token: 0x04006452 RID: 25682
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = -1;

	// Token: 0x04006453 RID: 25683
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> items = new List<ItemStack>();

	// Token: 0x04006454 RID: 25684
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> indexList = new List<int>();

	// Token: 0x04006455 RID: 25685
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006456 RID: 25686
	public int Length;

	// Token: 0x04006457 RID: 25687
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x04006458 RID: 25688
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006459 RID: 25689
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400645A RID: 25690
	[PublicizedFrom(EAccessModifier.Private)]
	public string category;

	// Token: 0x0400645B RID: 25691
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TraderItemEntry> entryList = new List<XUiC_TraderItemEntry>();
}
