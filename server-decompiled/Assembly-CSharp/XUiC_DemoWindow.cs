using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F1B RID: 3867
[Preserve]
public class XUiC_DemoWindow : XUiController
{
	// Token: 0x0600794B RID: 31051 RVA: 0x002DBE6D File Offset: 0x002DA06D
	public override void Init()
	{
		base.Init();
		XUiC_DemoWindow.ID = base.WindowGroup.Id;
	}

	// Token: 0x0600794C RID: 31052 RVA: 0x0029DB2B File Offset: 0x0029BD2B
	public override void OnOpen()
	{
		base.OnOpen();
		GameManager.Instance.Pause(true);
	}

	// Token: 0x0600794D RID: 31053 RVA: 0x0029DB3E File Offset: 0x0029BD3E
	public override void OnClose()
	{
		base.OnClose();
		GameManager.Instance.Pause(false);
	}

	// Token: 0x0600794E RID: 31054 RVA: 0x002D2ABB File Offset: 0x002D0CBB
	public override void Update(float _dt)
	{
		base.Update(_dt);
	}

	// Token: 0x0600794F RID: 31055 RVA: 0x002DBE88 File Offset: 0x002DA088
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

	// Token: 0x040059B6 RID: 22966
	public static string ID = "";
}
