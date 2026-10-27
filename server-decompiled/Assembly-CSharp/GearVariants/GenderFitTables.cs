using System;
using System.Collections.Generic;

namespace GearVariants
{
	// Token: 0x0200174A RID: 5962
	[Serializable]
	public class GenderFitTables
	{
		// Token: 0x0600B9A6 RID: 47526 RVA: 0x00447068 File Offset: 0x00445268
		public PartFitTable GetTable(GearFitMatrixSO.GearPart part)
		{
			PartFitTable partFitTable;
			switch (part)
			{
			case GearFitMatrixSO.GearPart.Head:
				partFitTable = this.head;
				break;
			case GearFitMatrixSO.GearPart.Hands:
				partFitTable = this.hands;
				break;
			case GearFitMatrixSO.GearPart.Feet:
				partFitTable = this.feet;
				break;
			default:
				partFitTable = this.hands;
				break;
			}
			return partFitTable;
		}

		// Token: 0x0600B9A7 RID: 47527 RVA: 0x004470AC File Offset: 0x004452AC
		public bool IsKnownGear(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (this._known == null)
			{
				this._known = new HashSet<string>(this.gearPaths, StringComparer.Ordinal);
			}
			return this._known.Contains(path);
		}

		// Token: 0x0600B9A8 RID: 47528 RVA: 0x004470E2 File Offset: 0x004452E2
		public void InvalidateIndexes()
		{
			this._known = null;
			this.head.InvalidateIndex();
			this.hands.InvalidateIndex();
			this.feet.InvalidateIndex();
		}

		// Token: 0x0600B9A9 RID: 47529 RVA: 0x0044710C File Offset: 0x0044530C
		public void EnsureRegistry()
		{
			while (this.gearGuids.Count < this.gearPaths.Count)
			{
				this.gearGuids.Add(string.Empty);
			}
			if (this.gearGuids.Count > this.gearPaths.Count)
			{
				this.gearGuids.RemoveRange(this.gearPaths.Count, this.gearGuids.Count - this.gearPaths.Count);
			}
		}

		// Token: 0x04008A9A RID: 35482
		public List<string> gearPaths = new List<string>();

		// Token: 0x04008A9B RID: 35483
		public List<string> gearGuids = new List<string>();

		// Token: 0x04008A9C RID: 35484
		public PartFitTable head = new PartFitTable();

		// Token: 0x04008A9D RID: 35485
		public PartFitTable hands = new PartFitTable();

		// Token: 0x04008A9E RID: 35486
		public PartFitTable feet = new PartFitTable();

		// Token: 0x04008A9F RID: 35487
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public HashSet<string> _known;
	}
}
