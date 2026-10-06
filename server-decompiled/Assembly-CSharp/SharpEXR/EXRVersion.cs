using System;

namespace SharpEXR
{
	// Token: 0x020016FA RID: 5882
	public struct EXRVersion
	{
		// Token: 0x0600B7D9 RID: 47065 RVA: 0x00441C4C File Offset: 0x0043FE4C
		public EXRVersion(int version, bool multiPart, bool longNames, bool nonImageParts, bool isSingleTiled = false)
		{
			this.Value = (EXRVersionFlags)(version & 255);
			if (version == 1)
			{
				if (multiPart || nonImageParts)
				{
					throw new EXRFormatException("Invalid or corrupt EXR version: Version 1 EXR files cannot be multi part or have non image parts.");
				}
				if (isSingleTiled)
				{
					this.Value |= EXRVersionFlags.IsSinglePartTiled;
				}
				if (longNames)
				{
					this.Value |= EXRVersionFlags.LongNames;
				}
			}
			else
			{
				if (isSingleTiled)
				{
					this.Value |= EXRVersionFlags.IsSinglePartTiled;
				}
				if (longNames)
				{
					this.Value |= EXRVersionFlags.LongNames;
				}
				if (nonImageParts)
				{
					this.Value |= EXRVersionFlags.NonImageParts;
				}
				if (multiPart)
				{
					this.Value |= EXRVersionFlags.MultiPart;
				}
			}
			this.Verify();
		}

		// Token: 0x0600B7DA RID: 47066 RVA: 0x00441D04 File Offset: 0x0043FF04
		public EXRVersion(int value)
		{
			this.Value = (EXRVersionFlags)value;
			this.Verify();
		}

		// Token: 0x0600B7DB RID: 47067 RVA: 0x00441D13 File Offset: 0x0043FF13
		[PublicizedFrom(EAccessModifier.Private)]
		public void Verify()
		{
			if (this.IsSinglePartTiled && (this.IsMultiPart || this.HasNonImageParts))
			{
				throw new EXRFormatException("Invalid or corrupt EXR version: Version's single part bit was set, but multi part and/or non image data bits were also set.");
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x0600B7DC RID: 47068 RVA: 0x00441D38 File Offset: 0x0043FF38
		public int Version
		{
			get
			{
				return (int)(this.Value & (EXRVersionFlags)255);
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x0600B7DD RID: 47069 RVA: 0x00441D46 File Offset: 0x0043FF46
		public bool IsSinglePartTiled
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.IsSinglePartTiled);
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x0600B7DE RID: 47070 RVA: 0x00441D62 File Offset: 0x0043FF62
		public bool HasLongNames
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.LongNames);
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x0600B7DF RID: 47071 RVA: 0x00441D7E File Offset: 0x0043FF7E
		public bool HasNonImageParts
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.NonImageParts);
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x0600B7E0 RID: 47072 RVA: 0x00441D9A File Offset: 0x0043FF9A
		public bool IsMultiPart
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.MultiPart);
			}
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x0600B7E1 RID: 47073 RVA: 0x00441DB6 File Offset: 0x0043FFB6
		public int MaxNameLength
		{
			get
			{
				if (!this.HasLongNames)
				{
					return 31;
				}
				return 255;
			}
		}

		// Token: 0x0400893D RID: 35133
		public readonly EXRVersionFlags Value;
	}
}
