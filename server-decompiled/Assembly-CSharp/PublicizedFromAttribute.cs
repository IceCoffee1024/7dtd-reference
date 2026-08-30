using System;
using System.Runtime.InteropServices;

// Token: 0x02001D70 RID: 7536
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public class PublicizedFromAttribute : Attribute
{
	// Token: 0x0600DCD9 RID: 56537 RVA: 0x004F3B6F File Offset: 0x004F1D6F
	public PublicizedFromAttribute(EAccessModifier _originalAccessModifier)
	{
		this.OriginalAccessModifier = _originalAccessModifier;
	}

	// Token: 0x0400AC31 RID: 44081
	public readonly EAccessModifier OriginalAccessModifier;
}
