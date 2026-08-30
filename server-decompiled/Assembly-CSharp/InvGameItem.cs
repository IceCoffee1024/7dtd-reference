using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000045 RID: 69
[Serializable]
public class InvGameItem
{
	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000189 RID: 393 RVA: 0x0000F85D File Offset: 0x0000DA5D
	public int baseItemID
	{
		get
		{
			return this.mBaseItemID;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600018A RID: 394 RVA: 0x0000F865 File Offset: 0x0000DA65
	public InvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600018B RID: 395 RVA: 0x0000F886 File Offset: 0x0000DA86
	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600018C RID: 396 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				num = 0f;
				break;
			case InvGameItem.Quality.Cursed:
				num = -1f;
				break;
			case InvGameItem.Quality.Damaged:
				num = 0.25f;
				break;
			case InvGameItem.Quality.Worn:
				num = 0.9f;
				break;
			case InvGameItem.Quality.Sturdy:
				num = 1f;
				break;
			case InvGameItem.Quality.Polished:
				num = 1.1f;
				break;
			case InvGameItem.Quality.Improved:
				num = 1.25f;
				break;
			case InvGameItem.Quality.Crafted:
				num = 1.5f;
				break;
			case InvGameItem.Quality.Superior:
				num = 1.75f;
				break;
			case InvGameItem.Quality.Enchanted:
				num = 2f;
				break;
			case InvGameItem.Quality.Epic:
				num = 2.5f;
				break;
			case InvGameItem.Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600018D RID: 397 RVA: 0x0000F988 File Offset: 0x0000DB88
	public Color color
	{
		get
		{
			Color color = Color.white;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				color = new Color(0.4f, 0.2f, 0.2f);
				break;
			case InvGameItem.Quality.Cursed:
				color = Color.red;
				break;
			case InvGameItem.Quality.Damaged:
				color = new Color(0.4f, 0.4f, 0.4f);
				break;
			case InvGameItem.Quality.Worn:
				color = new Color(0.7f, 0.7f, 0.7f);
				break;
			case InvGameItem.Quality.Sturdy:
				color = new Color(1f, 1f, 1f);
				break;
			case InvGameItem.Quality.Polished:
				color = NGUIMath.HexToColor(3774856959U);
				break;
			case InvGameItem.Quality.Improved:
				color = NGUIMath.HexToColor(2480359935U);
				break;
			case InvGameItem.Quality.Crafted:
				color = NGUIMath.HexToColor(1325334783U);
				break;
			case InvGameItem.Quality.Superior:
				color = NGUIMath.HexToColor(12255231U);
				break;
			case InvGameItem.Quality.Enchanted:
				color = NGUIMath.HexToColor(1937178111U);
				break;
			case InvGameItem.Quality.Epic:
				color = NGUIMath.HexToColor(2516647935U);
				break;
			case InvGameItem.Quality.Legendary:
				color = NGUIMath.HexToColor(4287627519U);
				break;
			}
			return color;
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000FAA8 File Offset: 0x0000DCA8
	public InvGameItem(int id)
	{
		this.mBaseItemID = id;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000FAC5 File Offset: 0x0000DCC5
	public InvGameItem(int id, InvBaseItem bi)
	{
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000FAEC File Offset: 0x0000DCEC
	public List<InvStat> CalculateStats()
	{
		List<InvStat> list = new List<InvStat>();
		if (this.baseItem != null)
		{
			float statMultiplier = this.statMultiplier;
			List<InvStat> stats = this.baseItem.stats;
			int i = 0;
			int count = stats.Count;
			while (i < count)
			{
				InvStat invStat = stats[i];
				int num = Mathf.RoundToInt(statMultiplier * (float)invStat.amount);
				if (num != 0)
				{
					bool flag = false;
					int j = 0;
					int count2 = list.Count;
					while (j < count2)
					{
						InvStat invStat2 = list[j];
						if (invStat2.id == invStat.id && invStat2.modifier == invStat.modifier)
						{
							invStat2.amount += num;
							flag = true;
							break;
						}
						j++;
					}
					if (!flag)
					{
						list.Add(new InvStat
						{
							id = invStat.id,
							amount = num,
							modifier = invStat.modifier
						});
					}
				}
				i++;
			}
			list.Sort(new Comparison<InvStat>(InvStat.CompareArmor));
		}
		return list;
	}

	// Token: 0x04000224 RID: 548
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public int mBaseItemID;

	// Token: 0x04000225 RID: 549
	public InvGameItem.Quality quality = InvGameItem.Quality.Sturdy;

	// Token: 0x04000226 RID: 550
	public int itemLevel = 1;

	// Token: 0x04000227 RID: 551
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public InvBaseItem mBaseItem;

	// Token: 0x02000046 RID: 70
	public enum Quality
	{
		// Token: 0x04000229 RID: 553
		Broken,
		// Token: 0x0400022A RID: 554
		Cursed,
		// Token: 0x0400022B RID: 555
		Damaged,
		// Token: 0x0400022C RID: 556
		Worn,
		// Token: 0x0400022D RID: 557
		Sturdy,
		// Token: 0x0400022E RID: 558
		Polished,
		// Token: 0x0400022F RID: 559
		Improved,
		// Token: 0x04000230 RID: 560
		Crafted,
		// Token: 0x04000231 RID: 561
		Superior,
		// Token: 0x04000232 RID: 562
		Enchanted,
		// Token: 0x04000233 RID: 563
		Epic,
		// Token: 0x04000234 RID: 564
		Legendary,
		// Token: 0x04000235 RID: 565
		_LastDoNotUse
	}
}
