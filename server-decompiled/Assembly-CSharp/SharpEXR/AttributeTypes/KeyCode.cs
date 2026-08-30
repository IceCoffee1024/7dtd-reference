using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016BD RID: 5821
	public struct KeyCode
	{
		// Token: 0x0600B5A2 RID: 46498 RVA: 0x00439186 File Offset: 0x00437386
		public KeyCode(int filmMfcCode, int filmType, int prefix, int count, int perfOffset, int perfsPerFrame, int perfsPerCount)
		{
			this.FilmMfcCode = filmMfcCode;
			this.FilmType = filmType;
			this.Prefix = prefix;
			this.Count = count;
			this.PerfOffset = perfOffset;
			this.PerfsPerFrame = perfsPerFrame;
			this.PerfsPerCount = perfsPerCount;
		}

		// Token: 0x040087B8 RID: 34744
		public readonly int FilmMfcCode;

		// Token: 0x040087B9 RID: 34745
		public readonly int FilmType;

		// Token: 0x040087BA RID: 34746
		public readonly int Prefix;

		// Token: 0x040087BB RID: 34747
		public readonly int Count;

		// Token: 0x040087BC RID: 34748
		public readonly int PerfOffset;

		// Token: 0x040087BD RID: 34749
		public readonly int PerfsPerFrame;

		// Token: 0x040087BE RID: 34750
		public readonly int PerfsPerCount;
	}
}
