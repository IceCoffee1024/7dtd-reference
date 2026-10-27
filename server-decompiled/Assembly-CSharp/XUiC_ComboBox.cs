using System;
using UnityEngine.Scripting;

// Token: 0x02000E92 RID: 3730
[Preserve]
public abstract class XUiC_ComboBox<TValue> : XUiC_ComboBoxBase
{
	// Token: 0x140000AA RID: 170
	// (add) Token: 0x060072B8 RID: 29368 RVA: 0x002C01CC File Offset: 0x002BE3CC
	// (remove) Token: 0x060072B9 RID: 29369 RVA: 0x002C0204 File Offset: 0x002BE404
	public event XUiC_ComboBox<TValue>.XUiEvent_ValueChanged OnValueChanged;

	// Token: 0x17000D4B RID: 3403
	// (get) Token: 0x060072BA RID: 29370
	// (set) Token: 0x060072BB RID: 29371
	public abstract TValue Value { get; set; }

	// Token: 0x060072BC RID: 29372 RVA: 0x002C0239 File Offset: 0x002BE439
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

	// Token: 0x060072BD RID: 29373 RVA: 0x002C0268 File Offset: 0x002BE468
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

	// Token: 0x060072BE RID: 29374 RVA: 0x002C02B4 File Offset: 0x002BE4B4
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

	// Token: 0x060072BF RID: 29375 RVA: 0x002C0300 File Offset: 0x002BE500
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

	// Token: 0x060072C0 RID: 29376
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract bool isDifferentValue(TValue _oldVal, TValue _currentValue);

	// Token: 0x060072C1 RID: 29377 RVA: 0x002C0348 File Offset: 0x002BE548
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ComboBox()
	{
	}

	// Token: 0x0400566C RID: 22124
	public XUiC_ComboBox<TValue>.CustomValueFormatterDelegate CustomValueFormatter;

	// Token: 0x0400566D RID: 22125
	[PublicizedFrom(EAccessModifier.Protected)]
	public TValue currentValue;

	// Token: 0x02000E93 RID: 3731
	// (Invoke) Token: 0x060072C3 RID: 29379
	public delegate void XUiEvent_ValueChanged(XUiController _sender, TValue _oldValue, TValue _newValue);

	// Token: 0x02000E94 RID: 3732
	// (Invoke) Token: 0x060072C7 RID: 29383
	public delegate string CustomValueFormatterDelegate(TValue _value);
}
