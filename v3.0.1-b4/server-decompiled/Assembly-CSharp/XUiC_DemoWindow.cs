using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F04 RID: 3844
[Preserve]
public class XUiC_DemoWindow : XUiController
{
	// Token: 0x06007893 RID: 30867 RVA: 0x002D7EE1 File Offset: 0x002D60E1
	public override void Init()
	{
		base.Init();
		XUiC_DemoWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007894 RID: 30868 RVA: 0x0029A9CB File Offset: 0x00298BCB
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x06007895 RID: 30869 RVA: 0x0029A9DE File Offset: 0x00298BDE
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x06007896 RID: 30870 RVA: 0x002CF437 File Offset: 0x002CD637
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x06007897 RID: 30871 RVA: 0x002D7EFC File Offset: 0x002D60FC
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

	// Token: 0x04005904 RID: 22788
	public static string ID = "";
}
