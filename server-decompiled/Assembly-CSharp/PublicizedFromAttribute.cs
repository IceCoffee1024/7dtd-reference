using System;
using System.Runtime.InteropServices;

// Token: 0x02001DF9 RID: 7673
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public class PublicizedFromAttribute : Attribute
{
	// Token: 0x0600E107 RID: 57607 RVA: 0x00502D37 File Offset: 0x00500F37
	public PublicizedFromAttribute(EAccessModifier _originalAccessModifier)
	{
		this.OriginalAccessModifier = _originalAccessModifier;
	}

	// Token: 0x0400AF2A RID: 44842
	public readonly EAccessModifier OriginalAccessModifier;
}
