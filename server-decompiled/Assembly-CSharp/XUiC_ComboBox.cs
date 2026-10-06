using System;
using UnityEngine.Scripting;

// Token: 0x02000E87 RID: 3719
[Preserve]
public abstract class XUiC_ComboBox<TValue> : XUiC_ComboBoxBase
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x06007293 RID: 29331 RVA: 0x002BEC60 File Offset: 0x002BCE60
	// (remove) Token: 0x06007294 RID: 29332 RVA: 0x002BEC98 File Offset: 0x002BCE98
	public event XUiC_ComboBox<TValue>.XUiEvent_ValueChanged OnValueChanged;

	// Token: 0x17000D45 RID: 3397
	// (get) Token: 0x06007295 RID: 29333
	// (set) Token: 0x06007296 RID: 29334
	public abstract TValue Value { get; set; }

	// Token: 0x06007297 RID: 29335 RVA: 0x002BECCD File Offset: 0x002BCECD
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

	// Token: 0x06007298 RID: 29336 RVA: 0x002BECFC File Offset: 0x002BCEFC
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

	// Token: 0x06007299 RID: 29337 RVA: 0x002BED48 File Offset: 0x002BCF48
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

	// Token: 0x0600729A RID: 29338 RVA: 0x002BED94 File Offset: 0x002BCF94
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

	// Token: 0x0600729B RID: 29339
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isDifferentValue(TValue _oldVal, TValue _currentValue);

	// Token: 0x0600729C RID: 29340 RVA: 0x002BEDDC File Offset: 0x002BCFDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBox()
	{
	}

	// Token: 0x04005647 RID: 22087
	public XUiC_ComboBox<TValue>.CustomValueFormatterDelegate CustomValueFormatter;

	// Token: 0x04005648 RID: 22088
	[PublicizedFrom(EAccessModifier.Protected)]
	public TValue currentValue;

	// Token: 0x02000E88 RID: 3720
	// (Invoke) Token: 0x0600729E RID: 29342
	public delegate void XUiEvent_ValueChanged(XUiController _sender, TValue _oldValue, TValue _newValue);

	// Token: 0x02000E89 RID: 3721
	// (Invoke) Token: 0x060072A2 RID: 29346
	public delegate string CustomValueFormatterDelegate(TValue _value);
}
