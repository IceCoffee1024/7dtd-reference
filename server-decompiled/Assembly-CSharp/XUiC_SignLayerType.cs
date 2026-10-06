using System;
using UnityEngine.Scripting;

// Token: 0x0200105B RID: 4187
[Preserve]
public class XUiC_SignLayerType : XUiC_SignGridEntry
{
	// Token: 0x060084A7 RID: 33959 RVA: 0x0032A2C8 File Offset: 0x003284C8
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060084A8 RID: 33960 RVA: 0x0032A2D0 File Offset: 0x003284D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060084A9 RID: 33961 RVA: 0x0032A2DF File Offset: 0x003284DF
	public override bool ParseAttribute(string name, string value)
	{
		return base.ParseAttribute(name, value);
	}
}
