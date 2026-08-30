using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001074 RID: 4212
[Preserve]
public class XUiC_TraderItemList : XUiController
{
	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x060085AE RID: 34222 RVA: 0x0033088E File Offset: 0x0032EA8E
	// (set) Token: 0x060085AF RID: 34223 RVA: 0x00330896 File Offset: 0x0032EA96
	public ItemStack CurrentItem
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x060085B0 RID: 34224 RVA: 0x0033089F File Offset: 0x0032EA9F
	// (set) Token: 0x060085B1 RID: 34225 RVA: 0x003308A7 File Offset: 0x0032EAA7
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

	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x060085B2 RID: 34226 RVA: 0x003308B0 File Offset: 0x0032EAB0
	// (set) Token: 0x060085B3 RID: 34227 RVA: 0x003308B8 File Offset: 0x0032EAB8
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x060085B4 RID: 34228 RVA: 0x003308C1 File Offset: 0x0032EAC1
	// (set) Token: 0x060085B5 RID: 34229 RVA: 0x003308CC File Offset: 0x0032EACC
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

	// Token: 0x060085B6 RID: 34230 RVA: 0x00330A08 File Offset: 0x0032EC08
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

	// Token: 0x060085B7 RID: 34231 RVA: 0x00330A8C File Offset: 0x0032EC8C
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

	// Token: 0x060085B8 RID: 34232 RVA: 0x00330AE7 File Offset: 0x0032ECE7
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.ClearSelection();
		this.IsDirty = true;
	}

	// Token: 0x060085B9 RID: 34233 RVA: 0x00330B17 File Offset: 0x0032ED17
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
		this.ClearSelection();
	}

	// Token: 0x060085BA RID: 34234 RVA: 0x00330B40 File Offset: 0x0032ED40
	public void ClearSelection()
	{
		this.CurrentItem = null;
		this.selectedIndex = -1;
		this.SelectedEntry = null;
		this.InfoWindow.SetItemStack(null, false);
	}

	// Token: 0x060085BB RID: 34235 RVA: 0x00330B64 File Offset: 0x0032ED64
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

	// Token: 0x04006348 RID: 25416
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemEntry selectedEntry;

	// Token: 0x04006349 RID: 25417
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = -1;

	// Token: 0x0400634A RID: 25418
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> items = new List<ItemStack>();

	// Token: 0x0400634B RID: 25419
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> indexList = new List<int>();

	// Token: 0x0400634C RID: 25420
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400634D RID: 25421
	public int Length;

	// Token: 0x0400634E RID: 25422
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x0400634F RID: 25423
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006350 RID: 25424
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006351 RID: 25425
	[PublicizedFrom(EAccessModifier.Private)]
	public string category;

	// Token: 0x04006352 RID: 25426
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TraderItemEntry> entryList = new List<XUiC_TraderItemEntry>();
}
