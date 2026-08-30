using System;
using UnityEngine.Scripting;

// Token: 0x02001033 RID: 4147
[Preserve]
public class XUiC_SignLayerType : XUiC_SignGridEntry
{
	// Token: 0x0600838C RID: 33676 RVA: 0x003220B8 File Offset: 0x003202B8
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x0600838D RID: 33677 RVA: 0x003220C0 File Offset: 0x003202C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600838E RID: 33678 RVA: 0x003220CF File Offset: 0x003202CF
	public override bool ParseAttribute(string name, string value)
	{
		return base.ParseAttribute(name, value);
	}
}
