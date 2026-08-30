using System;
using UnityEngine.Scripting;

// Token: 0x02001031 RID: 4145
[Preserve]
public class XUiC_SignEditorControl : XUiController
{
	// Token: 0x17000F4A RID: 3914
	// (get) Token: 0x06008348 RID: 33608 RVA: 0x0031F6E6 File Offset: 0x0031D8E6
	// (set) Token: 0x06008349 RID: 33609 RVA: 0x0031F6EE File Offset: 0x0031D8EE
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

	// Token: 0x0600834A RID: 33610 RVA: 0x0031F704 File Offset: 0x0031D904
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

	// Token: 0x0600834B RID: 33611 RVA: 0x0031F798 File Offset: 0x0031D998
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

	// Token: 0x04006167 RID: 24935
	public XUiV_Label label;

	// Token: 0x04006168 RID: 24936
	public XUiController control;

	// Token: 0x04006169 RID: 24937
	public XUiController controlB;

	// Token: 0x0400616A RID: 24938
	[PublicizedFrom(EAccessModifier.Private)]
	public object _defaultValue;
}
