using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600016B RID: 363 RVA: 0x0000F15F File Offset: 0x0000D35F
	public List<InvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000F188 File Offset: 0x0000D388
	public InvGameItem GetItem(int slot)
	{
		if (slot >= this.items.Count)
		{
			return null;
		}
		return this.mItems[slot];
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000F1A6 File Offset: 0x0000D3A6
	public InvGameItem Replace(int slot, InvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			InvGameItem invGameItem = this.items[slot];
			this.mItems[slot] = item;
			return invGameItem;
		}
		return item;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000F1CC File Offset: 0x0000D3CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (this.template != null)
		{
			int num = 0;
			Bounds bounds = default(Bounds);
			for (int i = 0; i < this.maxRows; i++)
			{
				for (int j = 0; j < this.maxColumns; j++)
				{
					GameObject gameObject = base.gameObject.AddChild(this.template);
					gameObject.transform.localPosition = new Vector3((float)this.padding + ((float)j + 0.5f) * (float)this.spacing, (float)(-(float)this.padding) - ((float)i + 0.5f) * (float)this.spacing, 0f);
					UIStorageSlot component = gameObject.GetComponent<UIStorageSlot>();
					if (component != null)
					{
						component.storage = this;
						component.slot = num;
					}
					bounds.Encapsulate(new Vector3((float)this.padding * 2f + (float)((j + 1) * this.spacing), (float)(-(float)this.padding) * 2f - (float)((i + 1) * this.spacing), 0f));
					if (++num >= this.maxItemCount)
					{
						if (this.background != null)
						{
							this.background.transform.localScale = bounds.size;
						}
						return;
					}
				}
			}
			if (this.background != null)
			{
				this.background.transform.localScale = bounds.size;
			}
		}
	}

	// Token: 0x040001FB RID: 507
	public int maxItemCount = 8;

	// Token: 0x040001FC RID: 508
	public int maxRows = 4;

	// Token: 0x040001FD RID: 509
	public int maxColumns = 4;

	// Token: 0x040001FE RID: 510
	public GameObject template;

	// Token: 0x040001FF RID: 511
	public UIWidget background;

	// Token: 0x04000200 RID: 512
	public int spacing = 128;

	// Token: 0x04000201 RID: 513
	public int padding = 10;

	// Token: 0x04000202 RID: 514
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<InvGameItem> mItems = new List<InvGameItem>();
}
