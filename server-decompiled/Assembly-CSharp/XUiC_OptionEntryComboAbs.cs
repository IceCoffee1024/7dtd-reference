using System;
using UnityEngine.Scripting;

// Token: 0x02000DA3 RID: 3491
[Preserve]
public abstract class XUiC_OptionEntryComboAbs : XUiC_OptionEntryAbs
{
	// Token: 0x060069B4 RID: 27060 RVA: 0x0029C59C File Offset: 0x0029A79C
	[XuiBindEvent("OnValueChangedGeneric", "comboGeneric")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnComboValueChanged(XUiController _sender)
	{
		if (base.ApplyImmediately)
		{
			this.immediatelyApplyCurrentSelection();
		}
		base.invokeValueChanged();
	}

	// Token: 0x060069B5 RID: 27061
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void immediatelyApplyCurrentSelection();

	// Token: 0x060069B6 RID: 27062 RVA: 0x0029C5B2 File Offset: 0x0029A7B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryComboAbs()
	{
	}
}
