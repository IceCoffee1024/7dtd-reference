using System;

namespace SharpEXR
{
	// Token: 0x02001719 RID: 5913
	public struct EXRVersion
	{
		// Token: 0x0600B8AD RID: 47277 RVA: 0x004446F0 File Offset: 0x004428F0
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

		// Token: 0x0600B8AE RID: 47278 RVA: 0x004447A8 File Offset: 0x004429A8
		public EXRVersion(int value)
		{
			this.Value = (EXRVersionFlags)value;
			this.Verify();
		}

		// Token: 0x0600B8AF RID: 47279 RVA: 0x004447B7 File Offset: 0x004429B7
		[PublicizedFrom(EAccessModifier.Private)]
		public void Verify()
		{
			if (this.IsSinglePartTiled && (this.IsMultiPart || this.HasNonImageParts))
			{
				throw new EXRFormatException("Invalid or corrupt EXR version: Version's single part bit was set, but multi part and/or non image data bits were also set.");
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x0600B8B0 RID: 47280 RVA: 0x004447DC File Offset: 0x004429DC
		public int Version
		{
			get
			{
				return (int)(this.Value & (EXRVersionFlags)255);
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x0600B8B1 RID: 47281 RVA: 0x004447EA File Offset: 0x004429EA
		public bool IsSinglePartTiled
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.IsSinglePartTiled);
			}
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600B8B2 RID: 47282 RVA: 0x00444806 File Offset: 0x00442A06
		public bool HasLongNames
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.LongNames);
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x0600B8B3 RID: 47283 RVA: 0x00444822 File Offset: 0x00442A22
		public bool HasNonImageParts
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.NonImageParts);
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x0600B8B4 RID: 47284 RVA: 0x0044483E File Offset: 0x00442A3E
		public bool IsMultiPart
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.MultiPart);
			}
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x0600B8B5 RID: 47285 RVA: 0x0044485A File Offset: 0x00442A5A
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

		// Token: 0x040089F0 RID: 35312
		public readonly EXRVersionFlags Value;
	}
}
