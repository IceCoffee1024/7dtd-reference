using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200172E RID: 5934
	public struct KeyCode
	{
		// Token: 0x0600B950 RID: 47440 RVA: 0x00445EC2 File Offset: 0x004440C2
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

		// Token: 0x04008A4C RID: 35404
		public readonly int FilmMfcCode;

		// Token: 0x04008A4D RID: 35405
		public readonly int FilmType;

		// Token: 0x04008A4E RID: 35406
		public readonly int Prefix;

		// Token: 0x04008A4F RID: 35407
		public readonly int Count;

		// Token: 0x04008A50 RID: 35408
		public readonly int PerfOffset;

		// Token: 0x04008A51 RID: 35409
		public readonly int PerfsPerFrame;

		// Token: 0x04008A52 RID: 35410
		public readonly int PerfsPerCount;
	}
}
