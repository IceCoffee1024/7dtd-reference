using System;
using UnityEngine.Scripting;

// Token: 0x0200104C RID: 4172
[Preserve]
public class XUiC_SignLayerType : XUiC_SignGridEntry
{
	// Token: 0x0600847E RID: 33918 RVA: 0x003268E0 File Offset: 0x00324AE0
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600847F RID: 33919 RVA: 0x003268E8 File Offset: 0x00324AE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008480 RID: 33920 RVA: 0x003268F7 File Offset: 0x00324AF7
	public override bool ParseAttribute(string name, string value)
	{
		return base.ParseAttribute(name, value);
	}
}
