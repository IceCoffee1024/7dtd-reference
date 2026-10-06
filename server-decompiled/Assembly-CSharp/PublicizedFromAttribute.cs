using System;
using System.Runtime.InteropServices;

// Token: 0x02001DC9 RID: 7625
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public class PublicizedFromAttribute : Attribute
{
	// Token: 0x0600DFF3 RID: 57331 RVA: 0x004FF19F File Offset: 0x004FD39F
	public PublicizedFromAttribute(EAccessModifier _originalAccessModifier)
	{
		this.OriginalAccessModifier = _originalAccessModifier;
	}

	// Token: 0x0400AE41 RID: 44609
	public readonly EAccessModifier OriginalAccessModifier;
}
