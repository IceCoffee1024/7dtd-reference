using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F2A RID: 3882
[Preserve]
public class XUiC_DemoWindow : XUiController
{
	// Token: 0x06007946 RID: 31046 RVA: 0x002DF209 File Offset: 0x002DD409
	public override void Init()
	{
		base.Init();
		XUiC_DemoWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007947 RID: 31047 RVA: 0x002A0EBB File Offset: 0x0029F0BB
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x06007948 RID: 31048 RVA: 0x002A0ECE File Offset: 0x0029F0CE
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x06007949 RID: 31049 RVA: 0x002D5E2F File Offset: 0x002D402F
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x0600794A RID: 31050 RVA: 0x002DF224 File Offset: 0x002DD424
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

	// Token: 0x040059EE RID: 23022
	public static string ID = "";
}
