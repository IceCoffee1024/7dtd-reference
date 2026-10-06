using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x0200170F RID: 5903
	public struct KeyCode
	{
		// Token: 0x0600B87C RID: 47228 RVA: 0x0044341E File Offset: 0x0044161E
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

		// Token: 0x04008999 RID: 35225
		public readonly int FilmMfcCode;

		// Token: 0x0400899A RID: 35226
		public readonly int FilmType;

		// Token: 0x0400899B RID: 35227
		public readonly int Prefix;

		// Token: 0x0400899C RID: 35228
		public readonly int Count;

		// Token: 0x0400899D RID: 35229
		public readonly int PerfOffset;

		// Token: 0x0400899E RID: 35230
		public readonly int PerfsPerFrame;

		// Token: 0x0400899F RID: 35231
		public readonly int PerfsPerCount;
	}
}
