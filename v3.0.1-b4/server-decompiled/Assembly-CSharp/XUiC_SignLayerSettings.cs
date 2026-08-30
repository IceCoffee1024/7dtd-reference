using System;
using UnityEngine.Scripting;

// Token: 0x02001021 RID: 4129
[Preserve]
public abstract class XUiC_SignLayerSettings : XUiController
{
	// Token: 0x060082EC RID: 33516
	public abstract void SetLayer(SignData.SignLayer layer);

	// Token: 0x060082ED RID: 33517 RVA: 0x0031E488 File Offset: 0x0031C688
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetDefaultValue(string id, object value)
	{
		XUiC_SignEditorControl xuiC_SignEditorControl = base.GetChildById(id) as XUiC_SignEditorControl;
		if (xuiC_SignEditorControl != null)
		{
			xuiC_SignEditorControl.defaultValue = value;
		}
	}

	// Token: 0x060082EE RID: 33518 RVA: 0x00088DE1 File Offset: 0x00086FE1
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignLayerSettings()
	{
	}

	// Token: 0x04006101 RID: 24833
	public Action<string, bool> OnPreLayerSettingsChanged;

	// Token: 0x04006102 RID: 24834
	public Action OnLayerSettingsChanged;
}
