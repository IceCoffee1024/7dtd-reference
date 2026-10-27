using System;
using UnityEngine.Scripting;

// Token: 0x02001050 RID: 4176
[Preserve]
public class XUiC_SignEditorControl : XUiController
{
	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060083A9 RID: 33705 RVA: 0x003244A2 File Offset: 0x003226A2
	// (set) Token: 0x060083AA RID: 33706 RVA: 0x003244AA File Offset: 0x003226AA
	public object defaultValue
	{
		get
		{
			return this._defaultValue;
		}
		set
		{
			this.label.EventOnPress = true;
			this._defaultValue = value;
		}
	}

	// Token: 0x060083AB RID: 33707 RVA: 0x003244C0 File Offset: 0x003226C0
	public override void Init()
	{
		base.Init();
		this.label = base.GetChildById("label").ViewComponent as XUiV_Label;
		this.label.Controller.OnRightPress += delegate(XUiController sender, int button)
		{
			this.SetDefault();
		};
		this.label.EventOnPress = false;
		this.control = base.GetChildById("value");
		if (this.control == null)
		{
			this.control = base.GetChildById("valueA");
			this.controlB = base.GetChildById("valueB");
		}
	}

	// Token: 0x060083AC RID: 33708 RVA: 0x00324554 File Offset: 0x00322754
	public virtual void SetDefault()
	{
		if (this.defaultValue == null)
		{
			return;
		}
		XUiController xuiController = this.control;
		XUiC_ComboBoxInt xuiC_ComboBoxInt = xuiController as XUiC_ComboBoxInt;
		if (xuiC_ComboBoxInt != null)
		{
			long value = xuiC_ComboBoxInt.Value;
			xuiC_ComboBoxInt.Value = (long)((int)this.defaultValue);
			xuiC_ComboBoxInt.TriggerValueChangedEvent(value);
			return;
		}
		XUiC_ComboBoxFloat xuiC_ComboBoxFloat = xuiController as XUiC_ComboBoxFloat;
		if (xuiC_ComboBoxFloat != null)
		{
			double value2 = xuiC_ComboBoxFloat.Value;
			xuiC_ComboBoxFloat.Value = (double)((float)this.defaultValue);
			xuiC_ComboBoxFloat.TriggerValueChangedEvent(value2);
			return;
		}
		XUiC_TextInput xuiC_TextInput = xuiController as XUiC_TextInput;
		if (xuiC_TextInput == null)
		{
			return;
		}
		XUiC_TextInput xuiC_TextInput2 = this.controlB as XUiC_TextInput;
		if (xuiC_TextInput2 != null)
		{
			ValueTuple<float, float> valueTuple = (ValueTuple<float, float>)this.defaultValue;
			xuiC_TextInput.Text = valueTuple.Item1.ToString();
			xuiC_TextInput.TriggerOnChangeHandler(false);
			xuiC_TextInput2.Text = valueTuple.Item2.ToString();
			xuiC_TextInput2.TriggerOnChangeHandler(false);
			return;
		}
		xuiC_TextInput.Text = this.defaultValue.ToString();
		xuiC_TextInput.TriggerOnChangeHandler(false);
	}

	// Token: 0x040061D0 RID: 25040
	public XUiV_Label label;

	// Token: 0x040061D1 RID: 25041
	public XUiController control;

	// Token: 0x040061D2 RID: 25042
	public XUiController controlB;

	// Token: 0x040061D3 RID: 25043
	[PublicizedFrom(EAccessModifier.Private)]
	public object _defaultValue;
}
