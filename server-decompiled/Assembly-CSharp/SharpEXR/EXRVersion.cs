using System;

namespace SharpEXR
{
	// Token: 0x020016A8 RID: 5800
	public struct EXRVersion
	{
		// Token: 0x0600B4FF RID: 46335 RVA: 0x004379B4 File Offset: 0x00435BB4
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

		// Token: 0x0600B500 RID: 46336 RVA: 0x00437A6C File Offset: 0x00435C6C
		public EXRVersion(int value)
		{
			this.Value = (EXRVersionFlags)value;
			this.Verify();
		}

		// Token: 0x0600B501 RID: 46337 RVA: 0x00437A7B File Offset: 0x00435C7B
		[PublicizedFrom(EAccessModifier.Private)]
		public void Verify()
		{
			if (this.IsSinglePartTiled && (this.IsMultiPart || this.HasNonImageParts))
			{
				throw new EXRFormatException("Invalid or corrupt EXR version: Version's single part bit was set, but multi part and/or non image data bits were also set.");
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x0600B502 RID: 46338 RVA: 0x00437AA0 File Offset: 0x00435CA0
		public int Version
		{
			get
			{
				return (int)(this.Value & (EXRVersionFlags)255);
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x0600B503 RID: 46339 RVA: 0x00437AAE File Offset: 0x00435CAE
		public bool IsSinglePartTiled
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.IsSinglePartTiled);
			}
		}

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x0600B504 RID: 46340 RVA: 0x00437ACA File Offset: 0x00435CCA
		public bool HasLongNames
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.LongNames);
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x0600B505 RID: 46341 RVA: 0x00437AE6 File Offset: 0x00435CE6
		public bool HasNonImageParts
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.NonImageParts);
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x0600B506 RID: 46342 RVA: 0x00437B02 File Offset: 0x00435D02
		public bool IsMultiPart
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.MultiPart);
			}
		}

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x0600B507 RID: 46343 RVA: 0x00437B1E File Offset: 0x00435D1E
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

		// Token: 0x0400875C RID: 34652
		public readonly EXRVersionFlags Value;
	}
}
