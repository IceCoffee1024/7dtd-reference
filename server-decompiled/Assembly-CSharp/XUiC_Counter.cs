using System;
using UnityEngine.Scripting;

// Token: 0x02000F06 RID: 3846
[Preserve]
public class XUiC_Counter : XUiController
{
	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x0600789E RID: 30878 RVA: 0x002D831C File Offset: 0x002D651C
	// (remove) Token: 0x0600789F RID: 30879 RVA: 0x002D8354 File Offset: 0x002D6554
	public event XUiEvent_OnCountChanged OnCountChanged;

	// Token: 0x17000E1E RID: 3614
	// (get) Token: 0x060078A0 RID: 30880 RVA: 0x002D8389 File Offset: 0x002D6589
	// (set) Token: 0x060078A1 RID: 30881 RVA: 0x002D8391 File Offset: 0x002D6591
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

	// Token: 0x060078A2 RID: 30882 RVA: 0x002D83A4 File Offset: 0x002D65A4
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

	// Token: 0x060078A3 RID: 30883 RVA: 0x002D84B8 File Offset: 0x002D66B8
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

	// Token: 0x060078A4 RID: 30884 RVA: 0x002D84F4 File Offset: 0x002D66F4
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

	// Token: 0x060078A5 RID: 30885 RVA: 0x002D8590 File Offset: 0x002D6790
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleMaxCountOnPress(XUiController _sender, int _mouseButton)
	{
		this.Count = this.MaxCount;
		this.HandleCountChangedEvent();
	}

	// Token: 0x060078A6 RID: 30886 RVA: 0x002D85A4 File Offset: 0x002D67A4
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

	// Token: 0x060078A7 RID: 30887 RVA: 0x002D8651 File Offset: 0x002D6851
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

	// Token: 0x060078A8 RID: 30888 RVA: 0x002D8681 File Offset: 0x002D6881
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

	// Token: 0x060078A9 RID: 30889 RVA: 0x002D86B6 File Offset: 0x002D68B6
	public void SetToMaxCount()
	{
		this.Count = this.MaxCount;
		this.HandleStepClamping();
		this.HandleCountChangedEvent();
		this.ForceTextRefresh();
	}

	// Token: 0x060078AA RID: 30890 RVA: 0x002D86D6 File Offset: 0x002D68D6
	public void SetCount(int count)
	{
		if (this.Count != count)
		{
			this.Count = count;
			this.HandleCountChangedEvent();
		}
	}

	// Token: 0x060078AB RID: 30891 RVA: 0x002D86F0 File Offset: 0x002D68F0
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

	// Token: 0x060078AC RID: 30892 RVA: 0x002D8728 File Offset: 0x002D6928
	public void ForceTextRefresh()
	{
		((XUiV_Label)this.counter.ViewComponent).Text = (this.textInput.Text = this.count.ToString());
	}

	// Token: 0x060078AD RID: 30893 RVA: 0x002D8763 File Offset: 0x002D6963
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TextInput_HandleInputDeselected(XUiController _sender, bool _selected)
	{
		if (!_selected)
		{
			this.ForceTextRefresh();
		}
	}

	// Token: 0x0400592C RID: 22828
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countUp;

	// Token: 0x0400592D RID: 22829
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countDown;

	// Token: 0x0400592E RID: 22830
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countMax;

	// Token: 0x0400592F RID: 22831
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController counter;

	// Token: 0x04005930 RID: 22832
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_TextInput textInput;

	// Token: 0x04005931 RID: 22833
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x04005932 RID: 22834
	public int MaxCount = 1;

	// Token: 0x04005933 RID: 22835
	public int Step = 1;

	// Token: 0x04005934 RID: 22836
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiEvent_InputOnChangedEventHandler textInputChangedDelegate;
}
