using System;
using UnityEngine.Scripting;

// Token: 0x0200106B RID: 4203
[Preserve]
public class XUiC_SignLayerType : XUiC_SignGridEntry
{
	// Token: 0x060084F9 RID: 34041 RVA: 0x0032BFCC File Offset: 0x0032A1CC
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060084FA RID: 34042 RVA: 0x0032BFD4 File Offset: 0x0032A1D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060084FB RID: 34043 RVA: 0x0032BFE3 File Offset: 0x0032A1E3
	public override bool ParseAttribute(string name, string value)
	{
		return base.ParseAttribute(name, value);
	}
}
