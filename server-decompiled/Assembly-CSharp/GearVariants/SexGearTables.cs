using System;
using System.Collections.Generic;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x020016CA RID: 5834
	[Serializable]
	public class SexGearTables
	{
		// Token: 0x0600B5C4 RID: 46532 RVA: 0x00439A4B File Offset: 0x00437C4B
		public StringTable2D GetTable(GearVariantMatrixSO.GearPart part)
		{
			switch (part)
			{
			case GearVariantMatrixSO.GearPart.Head:
				return this.head;
			case GearVariantMatrixSO.GearPart.Hands:
				return this.hands;
			case GearVariantMatrixSO.GearPart.Feet:
				return this.feet;
			default:
				return this.hands;
			}
		}

		// Token: 0x0600B5C5 RID: 46533 RVA: 0x00439A7C File Offset: 0x00437C7C
		public void EnsureShapes()
		{
			while (this.gearGuids.Count < this.gearPaths.Count)
			{
				this.gearGuids.Add(string.Empty);
			}
			if (this.gearGuids.Count > this.gearPaths.Count)
			{
				this.gearGuids.RemoveRange(this.gearPaths.Count, this.gearGuids.Count - this.gearPaths.Count);
			}
			this.head.EnsureShapeSymmetric(this.gearPaths, this.gearGuids);
			this.hands.EnsureShapeSymmetric(this.gearPaths, this.gearGuids);
			this.feet.EnsureShapeSymmetric(this.gearPaths, this.gearGuids);
		}

		// Token: 0x0600B5C6 RID: 46534 RVA: 0x00439B40 File Offset: 0x00437D40
		public void ApplyUnifiedReorder(IList<string> newOrder)
		{
			if (newOrder == null)
			{
				return;
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
			for (int i = 0; i < this.gearPaths.Count; i++)
			{
				dictionary[this.gearPaths[i]] = i;
			}
			int[] array = new int[newOrder.Count];
			for (int j = 0; j < newOrder.Count; j++)
			{
				int num;
				if (!dictionary.TryGetValue(newOrder[j], out num))
				{
					num = Mathf.Clamp(j, 0, this.gearPaths.Count - 1);
				}
				array[j] = num;
			}
			List<string> list = new List<string>(newOrder);
			List<string> list2 = new List<string>(list.Count);
			for (int k = 0; k < list.Count; k++)
			{
				int num2 = array[k];
				string text = ((num2 >= 0 && num2 < this.gearGuids.Count) ? this.gearGuids[num2] : string.Empty);
				list2.Add(text);
			}
			this.gearPaths = list;
			this.gearGuids = list2;
			this.head.ApplyReorder(array);
			this.hands.ApplyReorder(array);
			this.feet.ApplyReorder(array);
		}

		// Token: 0x040087DC RID: 34780
		public List<string> gearPaths = new List<string>();

		// Token: 0x040087DD RID: 34781
		public List<string> gearGuids = new List<string>();

		// Token: 0x040087DE RID: 34782
		public StringTable2D head = new StringTable2D();

		// Token: 0x040087DF RID: 34783
		public StringTable2D hands = new StringTable2D();

		// Token: 0x040087E0 RID: 34784
		public StringTable2D feet = new StringTable2D();
	}
}
