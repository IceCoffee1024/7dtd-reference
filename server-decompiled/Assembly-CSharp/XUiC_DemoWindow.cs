using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F39 RID: 3897
[Preserve]
public class XUiC_DemoWindow : XUiController
{
	// Token: 0x06007989 RID: 31113 RVA: 0x002E0AF9 File Offset: 0x002DECF9
	public override void Init()
	{
		base.Init();
		XUiC_DemoWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x0600798A RID: 31114 RVA: 0x002A23E3 File Offset: 0x002A05E3
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x0600798B RID: 31115 RVA: 0x002A23F6 File Offset: 0x002A05F6
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x0600798C RID: 31116 RVA: 0x002D7387 File Offset: 0x002D5587
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x0600798D RID: 31117 RVA: 0x002E0B14 File Offset: 0x002DED14
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "is_xbox")
		{
			value = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent().ToString();
			return true;
		}
		if (!(bindingName == "is_ps5"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = (DeviceFlag.PS5.IsCurrent() || (DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent()).ToString();
		return true;
	}

	// Token: 0x04005A21 RID: 23073
	public static string ID = "";
}
