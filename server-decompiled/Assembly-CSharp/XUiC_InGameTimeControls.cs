using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F66 RID: 3942
[Preserve]
public class XUiC_InGameTimeControls : XUiController
{
	// Token: 0x17000E7A RID: 3706
	// (get) Token: 0x06007B1C RID: 31516 RVA: 0x002E8C1B File Offset: 0x002E6E1B
	public static bool HasWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.World != null;
		}
	}

	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x06007B1D RID: 31517 RVA: 0x002EA187 File Offset: 0x002E8387
	public static int CurrentSpeed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameStats.GetInt(EnumGameStats.TimeOfDayIncPerSec);
		}
	}

	// Token: 0x06007B1E RID: 31518 RVA: 0x002EA190 File Offset: 0x002E8390
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "is_paused")
		{
			_value = (XUiC_InGameTimeControls.CurrentSpeed == 0).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007B1F RID: 31519 RVA: 0x002EA1C8 File Offset: 0x002E83C8
	public override void Init()
	{
		base.Init();
		GameStats.OnChangedDelegates += this.onGameStatsChanged;
		XUiController childById = base.GetChildById("cbxDay");
		this.cbxDay = ((childById != null) ? childById.GetChildByType<XUiC_ComboBoxInt>() : null);
		if (this.cbxDay != null)
		{
			this.cbxDay.OnValueChanged += delegate(XUiController _, long _, long _newValue)
			{
				this.setDay((int)_newValue);
			};
		}
		XUiController childById2 = base.GetChildById("cbxTime");
		this.cbxTime = ((childById2 != null) ? childById2.GetChildByType<XUiC_ComboBoxFloat>() : null);
		if (this.cbxTime != null)
		{
			this.cbxTime.OnValueChanged += delegate(XUiController _, double _, double _newValue)
			{
				this.setTime((float)_newValue);
			};
			this.cbxTime.CustomValueFormatter = delegate(double _value)
			{
				int num = (int)_value;
				int num2 = (int)((_value - (double)num) * 60.0);
				return string.Format("{0:00}:{1:00}", num, num2);
			};
		}
		XUiController childById3 = base.GetChildById("cbxSpeed");
		this.cbxSpeed = ((childById3 != null) ? childById3.GetChildByType<XUiC_ComboBoxInt>() : null);
		if (this.cbxSpeed != null)
		{
			this.cbxSpeed.OnValueChanged += delegate(XUiController _, long _, long _newValue)
			{
				this.setSpeed((int)_newValue);
			};
		}
		XUiController childById4 = base.GetChildById("btnTimeSkipBack");
		XUiV_Button xuiV_Button = ((childById4 != null) ? childById4.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button != null)
		{
			xuiV_Button.Controller.OnPress += this.btnTimeSkipBackPressed;
		}
		XUiController childById5 = base.GetChildById("btnTimeSlower");
		XUiV_Button xuiV_Button2 = ((childById5 != null) ? childById5.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button2 != null)
		{
			xuiV_Button2.Controller.OnPress += this.btnTimeSlowerPressed;
		}
		XUiController childById6 = base.GetChildById("btnTimePlayPause");
		XUiV_Button xuiV_Button3 = ((childById6 != null) ? childById6.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button3 != null)
		{
			xuiV_Button3.Controller.OnPress += this.btnTimePlayPausePressed;
		}
		XUiController childById7 = base.GetChildById("btnTimeFaster");
		XUiV_Button xuiV_Button4 = ((childById7 != null) ? childById7.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button4 != null)
		{
			xuiV_Button4.Controller.OnPress += this.btnTimeFasterPressed;
		}
		XUiController childById8 = base.GetChildById("btnTimeSkipForward");
		XUiV_Button xuiV_Button5 = ((childById8 != null) ? childById8.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button5 != null)
		{
			xuiV_Button5.Controller.OnPress += this.btnTimeSkipForwardPressed;
		}
	}

	// Token: 0x06007B20 RID: 31520 RVA: 0x002EA3E0 File Offset: 0x002E85E0
	public override void Cleanup()
	{
		GameStats.OnChangedDelegates -= this.onGameStatsChanged;
		base.Cleanup();
	}

	// Token: 0x06007B21 RID: 31521 RVA: 0x002EA3FC File Offset: 0x002E85FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void onGameStatsChanged(EnumGameStats _stat, object _value)
	{
		if (_stat != EnumGameStats.TimeOfDayIncPerSec)
		{
			return;
		}
		int num = (int)_value;
		if (num > 0)
		{
			this.lastNonZeroSpeed = num;
		}
	}

	// Token: 0x06007B22 RID: 31522 RVA: 0x002EA424 File Offset: 0x002E8624
	[PublicizedFrom(EAccessModifier.Private)]
	public void setDay(int _day)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		ulong num = GameManager.Instance.World.worldTime % 24000UL;
		GameManager.Instance.World.SetTimeJump(num + (ulong)((long)(_day - 1) * 24000L), true);
	}

	// Token: 0x06007B23 RID: 31523 RVA: 0x002EA474 File Offset: 0x002E8674
	[PublicizedFrom(EAccessModifier.Private)]
	public void setTime(float _dayTime)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		ulong num = GameManager.Instance.World.worldTime / 24000UL;
		int num2 = Mathf.Clamp((int)(_dayTime * 1000f), 0, 23999);
		GameManager.Instance.World.SetTimeJump(num * 24000UL + (ulong)((long)num2), true);
	}

	// Token: 0x06007B24 RID: 31524 RVA: 0x002EA4D4 File Offset: 0x002E86D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setSpeed(int _value)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		GameStats.Set(EnumGameStats.TimeOfDayIncPerSec, _value);
		base.RefreshBindings();
	}

	// Token: 0x06007B25 RID: 31525 RVA: 0x002EA4F4 File Offset: 0x002E86F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTimeSkipBackPressed(XUiController _sender, int _mouseButton)
	{
		float num = GameManager.Instance.World.worldTime % 24000UL / 1000f - 0.050000004f;
		for (int i = this.skipHours.Length - 1; i >= 0; i--)
		{
			float num2 = this.skipHours[i];
			if (num2 < num)
			{
				this.setTime(num2);
				return;
			}
		}
	}

	// Token: 0x06007B26 RID: 31526 RVA: 0x002EA550 File Offset: 0x002E8750
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTimeSlowerPressed(XUiController _sender, int _mouseButton)
	{
		this.setSpeed((int)MathUtils.ToPreviousPowerOfTwo((uint)XUiC_InGameTimeControls.CurrentSpeed, false));
	}

	// Token: 0x06007B27 RID: 31527 RVA: 0x002EA563 File Offset: 0x002E8763
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTimePlayPausePressed(XUiController _sender, int _mouseButton)
	{
		this.setSpeed((XUiC_InGameTimeControls.CurrentSpeed == 0) ? this.lastNonZeroSpeed : 0);
	}

	// Token: 0x06007B28 RID: 31528 RVA: 0x002EA57B File Offset: 0x002E877B
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTimeFasterPressed(XUiController _sender, int _mouseButton)
	{
		this.setSpeed((int)MathUtils.ToNextPowerOfTwo((uint)XUiC_InGameTimeControls.CurrentSpeed, false));
	}

	// Token: 0x06007B29 RID: 31529 RVA: 0x002EA590 File Offset: 0x002E8790
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnTimeSkipForwardPressed(XUiController _sender, int _mouseButton)
	{
		float num = GameManager.Instance.World.worldTime % 24000UL / 1000f + 0.050000004f;
		for (int i = 0; i < this.skipHours.Length; i++)
		{
			float num2 = this.skipHours[i];
			if (num2 > num)
			{
				this.setTime(num2);
				return;
			}
		}
	}

	// Token: 0x06007B2A RID: 31530 RVA: 0x002EA5EC File Offset: 0x002E87EC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.cbxDay != null)
		{
			this.cbxDay.Value = (long)(XUiC_InGameTimeControls.HasWorld ? (GameManager.Instance.World.worldTime / 24000UL + 1UL) : 0UL);
		}
		if (this.cbxTime != null)
		{
			this.cbxTime.Value = (double)(XUiC_InGameTimeControls.HasWorld ? (GameManager.Instance.World.worldTime % 24000UL / 1000f) : 0f);
		}
		if (this.cbxSpeed != null)
		{
			this.cbxSpeed.Value = (long)XUiC_InGameTimeControls.CurrentSpeed;
		}
		base.RefreshBindings();
	}

	// Token: 0x04005B67 RID: 23399
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly float[] skipHours = new float[] { 0f, 4f, 12f, 22f };

	// Token: 0x04005B68 RID: 23400
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxDay;

	// Token: 0x04005B69 RID: 23401
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxTime;

	// Token: 0x04005B6A RID: 23402
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt cbxSpeed;

	// Token: 0x04005B6B RID: 23403
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastNonZeroSpeed = 1;
}
