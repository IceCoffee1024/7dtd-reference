using System;
using UnityEngine.Scripting;

// Token: 0x02000E63 RID: 3683
[Preserve]
public abstract class XUiC_ComboBox<TValue> : XUiC_ComboBoxBase
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x060071FB RID: 29179 RVA: 0x002B838C File Offset: 0x002B658C
	// (remove) Token: 0x060071FC RID: 29180 RVA: 0x002B83C4 File Offset: 0x002B65C4
	public event XUiC_ComboBox<TValue>.XUiEvent_ValueChanged OnValueChanged;

	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x060071FD RID: 29181
	// (set) Token: 0x060071FE RID: 29182
	public abstract TValue Value { get; set; }

	// Token: 0x060071FF RID: 29183 RVA: 0x002B83F9 File Offset: 0x002B65F9
	public void TriggerValueChangedEvent(TValue _oldVal)
	{
		base.UpdateIndexMarkerStates();
		base.invokeValueChangedGeneric();
		XUiC_ComboBox<TValue>.XUiEvent_ValueChanged onValueChanged = this.OnValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(this, _oldVal, this.currentValue);
		}
		this.IsDirty = true;
	}

	// Token: 0x06007200 RID: 29184 RVA: 0x002B8428 File Offset: 0x002B6628
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryPageUp()
	{
		bool flag = false;
		if (base.Enabled)
		{
			TValue tvalue = this.currentValue;
			this.ForwardPressed();
			this.UpdateLabel();
			flag = this.isDifferentValue(tvalue, this.currentValue);
			if (flag)
			{
				this.TriggerValueChangedEvent(tvalue);
			}
			this.IsDirty = true;
		}
		return flag;
	}

	// Token: 0x06007201 RID: 29185 RVA: 0x002B8474 File Offset: 0x002B6674
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryPageDown()
	{
		bool flag = false;
		if (base.Enabled)
		{
			TValue tvalue = this.currentValue;
			this.BackPressed();
			this.UpdateLabel();
			flag = this.isDifferentValue(tvalue, this.currentValue);
			if (flag)
			{
				this.TriggerValueChangedEvent(tvalue);
			}
			this.IsDirty = true;
		}
		return flag;
	}

	// Token: 0x06007202 RID: 29186 RVA: 0x002B84C0 File Offset: 0x002B66C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setRelativeValue(float _value)
	{
		if (!base.Enabled)
		{
			return;
		}
		TValue tvalue = this.currentValue;
		this.RelativeValue = (double)_value;
		this.UpdateLabel();
		if (this.isDifferentValue(tvalue, this.currentValue))
		{
			this.TriggerValueChangedEvent(tvalue);
		}
		this.IsDirty = true;
	}

	// Token: 0x06007203 RID: 29187
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isDifferentValue(TValue _oldVal, TValue _currentValue);

	// Token: 0x06007204 RID: 29188 RVA: 0x002B8508 File Offset: 0x002B6708
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBox()
	{
	}

	// Token: 0x0400556C RID: 21868
	public XUiC_ComboBox<TValue>.CustomValueFormatterDelegate CustomValueFormatter;

	// Token: 0x0400556D RID: 21869
	[PublicizedFrom(EAccessModifier.Protected)]
	public TValue currentValue;

	// Token: 0x02000E64 RID: 3684
	// (Invoke) Token: 0x06007206 RID: 29190
	public delegate void XUiEvent_ValueChanged(XUiController _sender, TValue _oldValue, TValue _newValue);

	// Token: 0x02000E65 RID: 3685
	// (Invoke) Token: 0x0600720A RID: 29194
	public delegate string CustomValueFormatterDelegate(TValue _value);
}
