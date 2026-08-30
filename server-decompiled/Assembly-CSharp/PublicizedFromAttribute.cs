using System;
using System.Runtime.InteropServices;

// Token: 0x02001D97 RID: 7575
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public class PublicizedFromAttribute : Attribute
{
	// Token: 0x0600DE7A RID: 56954 RVA: 0x004FA5E3 File Offset: 0x004F87E3
	public PublicizedFromAttribute(EAccessModifier _originalAccessModifier)
	{
		this.OriginalAccessModifier = _originalAccessModifier;
	}

	// Token: 0x0400AD68 RID: 44392
	public readonly EAccessModifier OriginalAccessModifier;
}
