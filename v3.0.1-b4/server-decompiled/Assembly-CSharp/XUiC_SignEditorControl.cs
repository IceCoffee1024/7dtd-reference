using System;
using UnityEngine.Scripting;

// Token: 0x02001018 RID: 4120
[Preserve]
public class XUiC_SignEditorControl : XUiController
{
	// Token: 0x17000F11 RID: 3857
	// (get) Token: 0x06008256 RID: 33366 RVA: 0x0031AEBE File Offset: 0x003190BE
	// (set) Token: 0x06008257 RID: 33367 RVA: 0x0031AEC6 File Offset: 0x003190C6
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

	// Token: 0x06008258 RID: 33368 RVA: 0x0031AEDC File Offset: 0x003190DC
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

	// Token: 0x06008259 RID: 33369 RVA: 0x0031AF70 File Offset: 0x00319170
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

	// Token: 0x040060A0 RID: 24736
	public XUiV_Label label;

	// Token: 0x040060A1 RID: 24737
	public XUiController control;

	// Token: 0x040060A2 RID: 24738
	public XUiController controlB;

	// Token: 0x040060A3 RID: 24739
	[PublicizedFrom(EAccessModifier.Private)]
	public object _defaultValue;
}
