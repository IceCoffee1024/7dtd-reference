using System;
using System.Collections.Generic;
using UnityEngine;

namespace GearVariants
{
	// Token: 0x0200174D RID: 5965
	[Serializable]
	public class SexGearTables
	{
		// Token: 0x0600B9B2 RID: 47538 RVA: 0x0044768F File Offset: 0x0044588F
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

		// Token: 0x0600B9B3 RID: 47539 RVA: 0x004476C0 File Offset: 0x004458C0
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

		// Token: 0x0600B9B4 RID: 47540 RVA: 0x00447784 File Offset: 0x00445984
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

		// Token: 0x04008AA6 RID: 35494
		public List<string> gearPaths = new List<string>();

		// Token: 0x04008AA7 RID: 35495
		public List<string> gearGuids = new List<string>();

		// Token: 0x04008AA8 RID: 35496
		public StringTable2D head = new StringTable2D();

		// Token: 0x04008AA9 RID: 35497
		public StringTable2D hands = new StringTable2D();

		// Token: 0x04008AAA RID: 35498
		public StringTable2D feet = new StringTable2D();
	}
}
