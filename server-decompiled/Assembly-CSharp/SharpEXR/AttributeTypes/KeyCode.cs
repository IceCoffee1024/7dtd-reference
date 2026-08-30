using System;

namespace SharpEXR.AttributeTypes
{
	// Token: 0x020016E3 RID: 5859
	public struct KeyCode
	{
		// Token: 0x0600B725 RID: 46885 RVA: 0x0043EEFE File Offset: 0x0043D0FE
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

		// Token: 0x040088D4 RID: 35028
		public readonly int FilmMfcCode;

		// Token: 0x040088D5 RID: 35029
		public readonly int FilmType;

		// Token: 0x040088D6 RID: 35030
		public readonly int Prefix;

		// Token: 0x040088D7 RID: 35031
		public readonly int Count;

		// Token: 0x040088D8 RID: 35032
		public readonly int PerfOffset;

		// Token: 0x040088D9 RID: 35033
		public readonly int PerfsPerFrame;

		// Token: 0x040088DA RID: 35034
		public readonly int PerfsPerCount;
	}
}
