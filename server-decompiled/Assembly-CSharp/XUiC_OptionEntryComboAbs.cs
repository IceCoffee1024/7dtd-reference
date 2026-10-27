using System;
using UnityEngine.Scripting;

// Token: 0x02000DAE RID: 3502
[Preserve]
public abstract class XUiC_OptionEntryComboAbs : XUiC_OptionEntryAbs
{
	// Token: 0x060069D9 RID: 27097 RVA: 0x0029DAC4 File Offset: 0x0029BCC4
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

	// Token: 0x060069DA RID: 27098
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void immediatelyApplyCurrentSelection();

	// Token: 0x060069DB RID: 27099 RVA: 0x0029DADA File Offset: 0x0029BCDA
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionEntryComboAbs()
	{
	}
}
