using System;
using UnityEngine.Scripting;

// Token: 0x02000E78 RID: 3704
[Preserve]
public abstract class XUiC_ComboBox<TValue> : XUiC_ComboBoxBase
{
	// Token: 0x140000AB RID: 171
	// (add) Token: 0x0600729B RID: 29339 RVA: 0x002BB8B0 File Offset: 0x002B9AB0
	// (remove) Token: 0x0600729C RID: 29340 RVA: 0x002BB8E8 File Offset: 0x002B9AE8
	public event XUiC_ComboBox<TValue>.XUiEvent_ValueChanged OnValueChanged;

	// Token: 0x17000D2A RID: 3370
	// (get) Token: 0x0600729D RID: 29341
	// (set) Token: 0x0600729E RID: 29342
	public abstract TValue Value { get; set; }

	// Token: 0x0600729F RID: 29343 RVA: 0x002BB91D File Offset: 0x002B9B1D
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

	// Token: 0x060072A0 RID: 29344 RVA: 0x002BB94C File Offset: 0x002B9B4C
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

	// Token: 0x060072A1 RID: 29345 RVA: 0x002BB998 File Offset: 0x002B9B98
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

	// Token: 0x060072A2 RID: 29346 RVA: 0x002BB9E4 File Offset: 0x002B9BE4
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

	// Token: 0x060072A3 RID: 29347
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isDifferentValue(TValue _oldVal, TValue _currentValue);

	// Token: 0x060072A4 RID: 29348 RVA: 0x002BBA2C File Offset: 0x002B9C2C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBox()
	{
	}

	// Token: 0x04005610 RID: 22032
	public XUiC_ComboBox<TValue>.CustomValueFormatterDelegate CustomValueFormatter;

	// Token: 0x04005611 RID: 22033
	[PublicizedFrom(EAccessModifier.Protected)]
	public TValue currentValue;

	// Token: 0x02000E79 RID: 3705
	// (Invoke) Token: 0x060072A6 RID: 29350
	public delegate void XUiEvent_ValueChanged(XUiController _sender, TValue _oldValue, TValue _newValue);

	// Token: 0x02000E7A RID: 3706
	// (Invoke) Token: 0x060072AA RID: 29354
	public delegate string CustomValueFormatterDelegate(TValue _value);
}
