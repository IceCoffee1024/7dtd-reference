using System;
using UnityEngine.Scripting;

// Token: 0x02000EEF RID: 3823
[Preserve]
public class XUiC_Counter : XUiController
{
	// Token: 0x140000CF RID: 207
	// (add) Token: 0x060077E6 RID: 30694 RVA: 0x002D4390 File Offset: 0x002D2590
	// (remove) Token: 0x060077E7 RID: 30695 RVA: 0x002D43C8 File Offset: 0x002D25C8
	public event XUiEvent_OnCountChanged OnCountChanged;

	// Token: 0x17000E04 RID: 3588
	// (get) Token: 0x060077E8 RID: 30696 RVA: 0x002D43FD File Offset: 0x002D25FD
	// (set) Token: 0x060077E9 RID: 30697 RVA: 0x002D4405 File Offset: 0x002D2605
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

	// Token: 0x060077EA RID: 30698 RVA: 0x002D4418 File Offset: 0x002D2618
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

	// Token: 0x060077EB RID: 30699 RVA: 0x002D452C File Offset: 0x002D272C
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

	// Token: 0x060077EC RID: 30700 RVA: 0x002D4568 File Offset: 0x002D2768
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

	// Token: 0x060077ED RID: 30701 RVA: 0x002D4604 File Offset: 0x002D2804
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleMaxCountOnPress(XUiController _sender, int _mouseButton)
	{
		this.Count = this.MaxCount;
		this.HandleCountChangedEvent();
	}

	// Token: 0x060077EE RID: 30702 RVA: 0x002D4618 File Offset: 0x002D2818
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

	// Token: 0x060077EF RID: 30703 RVA: 0x002D46C5 File Offset: 0x002D28C5
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

	// Token: 0x060077F0 RID: 30704 RVA: 0x002D46F5 File Offset: 0x002D28F5
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

	// Token: 0x060077F1 RID: 30705 RVA: 0x002D472A File Offset: 0x002D292A
	public void SetToMaxCount()
	{
		this.Count = this.MaxCount;
		this.HandleStepClamping();
		this.HandleCountChangedEvent();
		this.ForceTextRefresh();
	}

	// Token: 0x060077F2 RID: 30706 RVA: 0x002D474A File Offset: 0x002D294A
	public void SetCount(int count)
	{
		if (this.Count != count)
		{
			this.Count = count;
			this.HandleCountChangedEvent();
		}
	}

	// Token: 0x060077F3 RID: 30707 RVA: 0x002D4764 File Offset: 0x002D2964
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

	// Token: 0x060077F4 RID: 30708 RVA: 0x002D479C File Offset: 0x002D299C
	public void ForceTextRefresh()
	{
		((XUiV_Label)this.counter.ViewComponent).Text = (this.textInput.Text = this.count.ToString());
	}

	// Token: 0x060077F5 RID: 30709 RVA: 0x002D47D7 File Offset: 0x002D29D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TextInput_HandleInputDeselected(XUiController _sender, bool _selected)
	{
		if (!_selected)
		{
			this.ForceTextRefresh();
		}
	}

	// Token: 0x0400587A RID: 22650
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countUp;

	// Token: 0x0400587B RID: 22651
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countDown;

	// Token: 0x0400587C RID: 22652
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countMax;

	// Token: 0x0400587D RID: 22653
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController counter;

	// Token: 0x0400587E RID: 22654
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_TextInput textInput;

	// Token: 0x0400587F RID: 22655
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x04005880 RID: 22656
	public int MaxCount = 1;

	// Token: 0x04005881 RID: 22657
	public int Step = 1;

	// Token: 0x04005882 RID: 22658
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiEvent_InputOnChangedEventHandler textInputChangedDelegate;
}
