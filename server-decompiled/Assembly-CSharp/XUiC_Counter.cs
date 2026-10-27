using System;
using UnityEngine.Scripting;

// Token: 0x02000F24 RID: 3876
[Preserve]
public class XUiC_Counter : XUiController
{
	// Token: 0x140000CF RID: 207
	// (add) Token: 0x060078DC RID: 30940 RVA: 0x002DCFA0 File Offset: 0x002DB1A0
	// (remove) Token: 0x060078DD RID: 30941 RVA: 0x002DCFD8 File Offset: 0x002DB1D8
	public event XUiEvent_OnCountChanged OnCountChanged;

	// Token: 0x17000E45 RID: 3653
	// (get) Token: 0x060078DE RID: 30942 RVA: 0x002DD00D File Offset: 0x002DB20D
	// (set) Token: 0x060078DF RID: 30943 RVA: 0x002DD015 File Offset: 0x002DB215
	public int Count
	{
		get
		{
			return this.count;
		}
		set
		{
			this.count = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x060078E0 RID: 30944 RVA: 0x002DD028 File Offset: 0x002DB228
	public override void Init()
	{
		base.Init();
		this.countUp = base.GetChildById("countUp");
		this.countDown = base.GetChildById("countDown");
		this.countMax = base.GetChildById("countMax");
		this.counter = base.GetChildById("text");
		this.countUp.OnPress += this.HandleCountUpOnPress;
		this.countDown.OnPress += this.HandleCountDownOnPress;
		this.countMax.OnPress += this.HandleMaxCountOnPress;
		this.countUp.OnHold += delegate(XUiController _sender, EHoldType _event, float _duration, float _timedEvent)
		{
			if (_event == EHoldType.HoldTimed)
			{
				this.HandleCountUpOnPress(_sender, -1);
			}
		};
		this.countDown.OnHold += delegate(XUiController _sender, EHoldType _event, float _duration, float _timedEvent)
		{
			if (_event == EHoldType.HoldTimed)
			{
				this.HandleCountDownOnPress(_sender, -1);
			}
		};
		this.textInputChangedDelegate = new XUiEvent_InputOnChangedEventHandler(this.TextInput_OnChangeHandler);
		this.textInput = base.GetChildByType<XUiC_TextInput>();
		this.textInput.OnChangeHandler += this.textInputChangedDelegate;
		this.textInput.OnInputSelectedHandler += this.TextInput_HandleInputDeselected;
	}

	// Token: 0x060078E1 RID: 30945 RVA: 0x002DD13C File Offset: 0x002DB33C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleCountChangedEvent()
	{
		if (this.OnCountChanged != null)
		{
			OnCountChangedEventArgs e = new OnCountChangedEventArgs();
			e.Count = this.Count;
			this.OnCountChanged(this, e);
			base.RefreshBindings();
		}
	}

	// Token: 0x060078E2 RID: 30946 RVA: 0x002DD178 File Offset: 0x002DB378
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TextInput_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		int num = 0;
		if (int.TryParse(_text, out num))
		{
			this.Count = num;
			if (this.Count > this.MaxCount)
			{
				this.Count = this.MaxCount;
				((XUiC_TextInput)_sender).Text = this.Count.ToString();
			}
			else if (this.Count <= 0)
			{
				this.Count = this.Step;
				((XUiC_TextInput)_sender).Text = this.Count.ToString();
			}
			this.HandleCountChangedEvent();
			return;
		}
		this.Count = this.Step;
		this.HandleCountChangedEvent();
	}

	// Token: 0x060078E3 RID: 30947 RVA: 0x002DD214 File Offset: 0x002DB414
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleMaxCountOnPress(XUiController _sender, int _mouseButton)
	{
		this.Count = this.MaxCount;
		this.HandleCountChangedEvent();
	}

	// Token: 0x060078E4 RID: 30948 RVA: 0x002DD228 File Offset: 0x002DB428
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.textInput.OnChangeHandler -= this.textInputChangedDelegate;
			((XUiV_Label)this.counter.ViewComponent).Text = (this.textInput.Text = ((this.textInput.Text == "") ? "" : ((this.Count > 0) ? this.Count.ToString() : "-")));
			this.textInput.OnChangeHandler += this.textInputChangedDelegate;
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060078E5 RID: 30949 RVA: 0x002DD2D5 File Offset: 0x002DB4D5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleCountDownOnPress(XUiController _sender, int _mouseButton)
	{
		if (this.Count > 1)
		{
			this.Count -= this.Step;
			this.HandleStepClamping();
		}
		this.HandleCountChangedEvent();
		this.ForceTextRefresh();
	}

	// Token: 0x060078E6 RID: 30950 RVA: 0x002DD305 File Offset: 0x002DB505
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleCountUpOnPress(XUiController _sender, int _mouseButton)
	{
		if (this.Count < this.MaxCount)
		{
			this.Count += this.Step;
			this.HandleStepClamping();
			this.HandleCountChangedEvent();
			this.ForceTextRefresh();
		}
	}

	// Token: 0x060078E7 RID: 30951 RVA: 0x002DD33A File Offset: 0x002DB53A
	public void SetToMaxCount()
	{
		this.Count = this.MaxCount;
		this.HandleStepClamping();
		this.HandleCountChangedEvent();
		this.ForceTextRefresh();
	}

	// Token: 0x060078E8 RID: 30952 RVA: 0x002DD35A File Offset: 0x002DB55A
	public void SetCount(int count)
	{
		if (this.Count != count)
		{
			this.Count = count;
			this.HandleCountChangedEvent();
		}
	}

	// Token: 0x060078E9 RID: 30953 RVA: 0x002DD374 File Offset: 0x002DB574
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleStepClamping()
	{
		if (this.Step > 1)
		{
			int num = this.Count % this.Step;
			if (num != 0)
			{
				this.Count -= num;
			}
		}
	}

	// Token: 0x060078EA RID: 30954 RVA: 0x002DD3AC File Offset: 0x002DB5AC
	public void ForceTextRefresh()
	{
		((XUiV_Label)this.counter.ViewComponent).Text = (this.textInput.Text = this.count.ToString());
	}

	// Token: 0x060078EB RID: 30955 RVA: 0x002DD3E7 File Offset: 0x002DB5E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TextInput_HandleInputDeselected(XUiController _sender, bool _selected)
	{
		if (!_selected)
		{
			this.ForceTextRefresh();
		}
	}

	// Token: 0x04005997 RID: 22935
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countUp;

	// Token: 0x04005998 RID: 22936
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countDown;

	// Token: 0x04005999 RID: 22937
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countMax;

	// Token: 0x0400599A RID: 22938
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController counter;

	// Token: 0x0400599B RID: 22939
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_TextInput textInput;

	// Token: 0x0400599C RID: 22940
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x0400599D RID: 22941
	public int MaxCount = 1;

	// Token: 0x0400599E RID: 22942
	public int Step = 1;

	// Token: 0x0400599F RID: 22943
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiEvent_InputOnChangedEventHandler textInputChangedDelegate;
}
