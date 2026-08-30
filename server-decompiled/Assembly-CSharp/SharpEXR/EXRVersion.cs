using System;

namespace SharpEXR
{
	// Token: 0x020016CE RID: 5838
	public struct EXRVersion
	{
		// Token: 0x0600B682 RID: 46722 RVA: 0x0043D72C File Offset: 0x0043B92C
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

		// Token: 0x0600B683 RID: 46723 RVA: 0x0043D7E4 File Offset: 0x0043B9E4
		public EXRVersion(int value)
		{
			this.Value = (EXRVersionFlags)value;
			this.Verify();
		}

		// Token: 0x0600B684 RID: 46724 RVA: 0x0043D7F3 File Offset: 0x0043B9F3
		[PublicizedFrom(EAccessModifier.Private)]
		public void Verify()
		{
			if (this.IsSinglePartTiled && (this.IsMultiPart || this.HasNonImageParts))
			{
				throw new EXRFormatException("Invalid or corrupt EXR version: Version's single part bit was set, but multi part and/or non image data bits were also set.");
			}
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x0600B685 RID: 46725 RVA: 0x0043D818 File Offset: 0x0043BA18
		public int Version
		{
			get
			{
				return (int)(this.Value & (EXRVersionFlags)255);
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x0600B686 RID: 46726 RVA: 0x0043D826 File Offset: 0x0043BA26
		public bool IsSinglePartTiled
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.IsSinglePartTiled);
			}
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x0600B687 RID: 46727 RVA: 0x0043D842 File Offset: 0x0043BA42
		public bool HasLongNames
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.LongNames);
			}
		}

		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x0600B688 RID: 46728 RVA: 0x0043D85E File Offset: 0x0043BA5E
		public bool HasNonImageParts
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.NonImageParts);
			}
		}

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x0600B689 RID: 46729 RVA: 0x0043D87A File Offset: 0x0043BA7A
		public bool IsMultiPart
		{
			get
			{
				return this.Value.HasFlag(EXRVersionFlags.MultiPart);
			}
		}

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x0600B68A RID: 46730 RVA: 0x0043D896 File Offset: 0x0043BA96
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

		// Token: 0x04008878 RID: 34936
		public readonly EXRVersionFlags Value;
	}
}
