using System;
using UnityEngine.Scripting;

// Token: 0x02001040 RID: 4160
[Preserve]
public class XUiC_SignEditorControl : XUiController
{
	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x06008357 RID: 33623 RVA: 0x0032279E File Offset: 0x0032099E
	// (set) Token: 0x06008358 RID: 33624 RVA: 0x003227A6 File Offset: 0x003209A6
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

	// Token: 0x06008359 RID: 33625 RVA: 0x003227BC File Offset: 0x003209BC
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

	// Token: 0x0600835A RID: 33626 RVA: 0x00322850 File Offset: 0x00320A50
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

	// Token: 0x04006192 RID: 24978
	public XUiV_Label label;

	// Token: 0x04006193 RID: 24979
	public XUiController control;

	// Token: 0x04006194 RID: 24980
	public XUiController controlB;

	// Token: 0x04006195 RID: 24981
	[PublicizedFrom(EAccessModifier.Private)]
	public object _defaultValue;
}
