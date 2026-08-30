using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200108D RID: 4237
[Preserve]
public class XUiC_TraderItemList : XUiController
{
	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x060086A0 RID: 34464 RVA: 0x003350B6 File Offset: 0x003332B6
	// (set) Token: 0x060086A1 RID: 34465 RVA: 0x003350BE File Offset: 0x003332BE
	public ItemStack CurrentItem
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FB0 RID: 4016
	// (get) Token: 0x060086A2 RID: 34466 RVA: 0x003350C7 File Offset: 0x003332C7
	// (set) Token: 0x060086A3 RID: 34467 RVA: 0x003350CF File Offset: 0x003332CF
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

	// Token: 0x17000FB1 RID: 4017
	// (get) Token: 0x060086A4 RID: 34468 RVA: 0x003350D8 File Offset: 0x003332D8
	// (set) Token: 0x060086A5 RID: 34469 RVA: 0x003350E0 File Offset: 0x003332E0
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x060086A6 RID: 34470 RVA: 0x003350E9 File Offset: 0x003332E9
	// (set) Token: 0x060086A7 RID: 34471 RVA: 0x003350F4 File Offset: 0x003332F4
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

	// Token: 0x060086A8 RID: 34472 RVA: 0x00335230 File Offset: 0x00333430
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

	// Token: 0x060086A9 RID: 34473 RVA: 0x003352B4 File Offset: 0x003334B4
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

	// Token: 0x060086AA RID: 34474 RVA: 0x0033530F File Offset: 0x0033350F
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.ClearSelection();
		this.IsDirty = true;
	}

	// Token: 0x060086AB RID: 34475 RVA: 0x0033533F File Offset: 0x0033353F
	public override void OnClose()
	{
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
		this.ClearSelection();
	}

	// Token: 0x060086AC RID: 34476 RVA: 0x00335368 File Offset: 0x00333568
	public void ClearSelection()
	{
		this.CurrentItem = null;
		this.selectedIndex = -1;
		this.SelectedEntry = null;
		this.InfoWindow.SetItemStack(null, false);
	}

	// Token: 0x060086AD RID: 34477 RVA: 0x0033538C File Offset: 0x0033358C
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

	// Token: 0x0400640F RID: 25615
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TraderItemEntry selectedEntry;

	// Token: 0x04006410 RID: 25616
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedIndex = -1;

	// Token: 0x04006411 RID: 25617
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> items = new List<ItemStack>();

	// Token: 0x04006412 RID: 25618
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> indexList = new List<int>();

	// Token: 0x04006413 RID: 25619
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006414 RID: 25620
	public int Length;

	// Token: 0x04006415 RID: 25621
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showFavorites;

	// Token: 0x04006416 RID: 25622
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 selectedColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04006417 RID: 25623
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04006418 RID: 25624
	[PublicizedFrom(EAccessModifier.Private)]
	public string category;

	// Token: 0x04006419 RID: 25625
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TraderItemEntry> entryList = new List<XUiC_TraderItemEntry>();
}
