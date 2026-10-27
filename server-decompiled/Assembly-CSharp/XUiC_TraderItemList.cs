using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010AE RID: 4270
[Preserve]
public class XUiC_TraderItemList : XUiController
{
	// Token: 0x17000FEF RID: 4079
	// (get) Token: 0x06008742 RID: 34626 RVA: 0x0033AF52 File Offset: 0x00339152
	// (set) Token: 0x06008743 RID: 34627 RVA: 0x0033AF5A File Offset: 0x0033915A
	public ItemStack CurrentItem
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FF0 RID: 4080
	// (get) Token: 0x06008744 RID: 34628 RVA: 0x0033AF63 File Offset: 0x00339163
	// (set) Token: 0x06008745 RID: 34629 RVA: 0x0033AF6B File Offset: 0x0033916B
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

	// Token: 0x17000FF1 RID: 4081
	// (get) Token: 0x06008746 RID: 34630 RVA: 0x0033AF74 File Offset: 0x00339174
	// (set) Token: 0x06008747 RID: 34631 RVA: 0x0033AF7C File Offset: 0x0033917C
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000FF2 RID: 4082
	// (get) Token: 0x06008748 RID: 34632 RVA: 0x0033AF85 File Offset: 0x00339185
	// (set) Token: 0x06008749 RID: 34633 RVA: 0x0033AF90 File Offset: 0x00339190
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

	// Token: 0x0600874A RID: 34634 RVA: 0x0033B0CC File Offset: 0x003392CC
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

	// Token: 0x0600874B RID: 34635 RVA: 0x0033B150 File Offset: 0x00339350
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

	// Token: 0x0600874C RID: 34636 RVA: 0x0033B1AB File Offset: 0x003393AB
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.ClearSelection();
		this.IsDirty = true;
	}

	// Token: 0x0600874D RID: 34637 RVA: 0x0033B1DB File Offset: 0x003393DB
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
		this.ClearSelection();
	}

	// Token: 0x0600874E RID: 34638 RVA: 0x0033B204 File Offset: 0x00339404
	public void ClearSelection()
	{
		this.CurrentItem = null;
		this.selectedIndex = -1;
		this.SelectedEntry = null;
		this.InfoWindow.SetItemStack(null, false);
	}

	// Token: 0x0600874F RID: 34639 RVA: 0x0033B228 File Offset: 0x00339428
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

	// Token: 0x04006490 RID: 25744
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemEntry selectedEntry;

	// Token: 0x04006491 RID: 25745
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = -1;

	// Token: 0x04006492 RID: 25746
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> items = new List<ItemStack>();

	// Token: 0x04006493 RID: 25747
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> indexList = new List<int>();

	// Token: 0x04006494 RID: 25748
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006495 RID: 25749
	public int Length;

	// Token: 0x04006496 RID: 25750
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x04006497 RID: 25751
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006498 RID: 25752
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006499 RID: 25753
	[PublicizedFrom(EAccessModifier.Private)]
	public string category;

	// Token: 0x0400649A RID: 25754
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TraderItemEntry> entryList = new List<XUiC_TraderItemEntry>();
}
