using System;
using UnityEngine.Scripting;

// Token: 0x02000F15 RID: 3861
[Preserve]
public class XUiC_Counter : XUiController
{
	// Token: 0x140000CF RID: 207
	// (add) Token: 0x06007899 RID: 30873 RVA: 0x002DB6B0 File Offset: 0x002D98B0
	// (remove) Token: 0x0600789A RID: 30874 RVA: 0x002DB6E8 File Offset: 0x002D98E8
	public event XUiEvent_OnCountChanged OnCountChanged;

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x0600789B RID: 30875 RVA: 0x002DB71D File Offset: 0x002D991D
	// (set) Token: 0x0600789C RID: 30876 RVA: 0x002DB725 File Offset: 0x002D9925
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

	// Token: 0x0600789D RID: 30877 RVA: 0x002DB738 File Offset: 0x002D9938
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

	// Token: 0x0600789E RID: 30878 RVA: 0x002DB84C File Offset: 0x002D9A4C
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

	// Token: 0x0600789F RID: 30879 RVA: 0x002DB888 File Offset: 0x002D9A88
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

	// Token: 0x060078A0 RID: 30880 RVA: 0x002DB924 File Offset: 0x002D9B24
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleMaxCountOnPress(XUiController _sender, int _mouseButton)
	{
		this.Count = this.MaxCount;
		this.HandleCountChangedEvent();
	}

	// Token: 0x060078A1 RID: 30881 RVA: 0x002DB938 File Offset: 0x002D9B38
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

	// Token: 0x060078A2 RID: 30882 RVA: 0x002DB9E5 File Offset: 0x002D9BE5
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

	// Token: 0x060078A3 RID: 30883 RVA: 0x002DBA15 File Offset: 0x002D9C15
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

	// Token: 0x060078A4 RID: 30884 RVA: 0x002DBA4A File Offset: 0x002D9C4A
	public void SetToMaxCount()
	{
		this.Count = this.MaxCount;
		this.HandleStepClamping();
		this.HandleCountChangedEvent();
		this.ForceTextRefresh();
	}

	// Token: 0x060078A5 RID: 30885 RVA: 0x002DBA6A File Offset: 0x002D9C6A
	public void SetCount(int count)
	{
		if (this.Count != count)
		{
			this.Count = count;
			this.HandleCountChangedEvent();
		}
	}

	// Token: 0x060078A6 RID: 30886 RVA: 0x002DBA84 File Offset: 0x002D9C84
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

	// Token: 0x060078A7 RID: 30887 RVA: 0x002DBABC File Offset: 0x002D9CBC
	public void ForceTextRefresh()
	{
		((XUiV_Label)this.counter.ViewComponent).Text = (this.textInput.Text = this.count.ToString());
	}

	// Token: 0x060078A8 RID: 30888 RVA: 0x002DBAF7 File Offset: 0x002D9CF7
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TextInput_HandleInputDeselected(XUiController _sender, bool _selected)
	{
		if (!_selected)
		{
			this.ForceTextRefresh();
		}
	}

	// Token: 0x04005964 RID: 22884
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countUp;

	// Token: 0x04005965 RID: 22885
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countDown;

	// Token: 0x04005966 RID: 22886
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController countMax;

	// Token: 0x04005967 RID: 22887
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController counter;

	// Token: 0x04005968 RID: 22888
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_TextInput textInput;

	// Token: 0x04005969 RID: 22889
	[PublicizedFrom(EAccessModifier.Private)]
	public int count = 1;

	// Token: 0x0400596A RID: 22890
	public int MaxCount = 1;

	// Token: 0x0400596B RID: 22891
	public int Step = 1;

	// Token: 0x0400596C RID: 22892
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiEvent_InputOnChangedEventHandler textInputChangedDelegate;
}
