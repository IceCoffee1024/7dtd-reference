using System;
using UnityEngine;

// Token: 0x02001246 RID: 4678
public abstract class GUIWindow
{
	// Token: 0x060094E3 RID: 38115 RVA: 0x0038612B File Offset: 0x0038432B
	[PublicizedFrom(EAccessModifier.Protected)]
	public GUIWindow(string _id)
	{
		this.Id = _id;
	}

	// Token: 0x060094E4 RID: 38116 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnGUI()
	{
	}

	// Token: 0x060094E5 RID: 38117 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060094E6 RID: 38118 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnOpen()
	{
	}

	// Token: 0x060094E7 RID: 38119 RVA: 0x00386145 File Offset: 0x00384345
	public virtual void OnClose()
	{
		Action onWindowClose = this.OnWindowClose;
		if (onWindowClose != null)
		{
			onWindowClose();
		}
		this.OnWindowClose = null;
	}

	// Token: 0x060094E8 RID: 38120 RVA: 0x0038615F File Offset: 0x0038435F
	public virtual PlayerActionsBase GetActionSet()
	{
		return this.playerUI.playerInput.GUIActions;
	}

	// Token: 0x060094E9 RID: 38121 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool HasActionSet()
	{
		return true;
	}

	// Token: 0x060094EA RID: 38122 RVA: 0x00386174 File Offset: 0x00384374
	public override bool Equals(object _obj)
	{
		GUIWindow guiwindow = _obj as GUIWindow;
		return guiwindow != null && guiwindow.Id.Equals(this.Id);
	}

	// Token: 0x060094EB RID: 38123 RVA: 0x0038619E File Offset: 0x0038439E
	public override int GetHashCode()
	{
		return this.Id.GetHashCode();
	}

	// Token: 0x060094EC RID: 38124 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x060094ED RID: 38125 RVA: 0x003861AC File Offset: 0x003843AC
	public static Matrix4x4 UiScaleMatrix(out float _targetScale, out float _actualScale, float _x = 0f, float _y = 0f, float _clampMin = 0.4f, float _clampMax = 2f)
	{
		_targetScale = (float)Screen.height / 1080f;
		_targetScale *= GameOptionsManager.GetActiveUiScale();
		_actualScale = Utils.FastClamp(_targetScale, _clampMin, _clampMax);
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(new Vector3(_x, _y), Quaternion.identity, new Vector3(_actualScale, _actualScale, 1f));
		return matrix;
	}

	// Token: 0x04006F93 RID: 28563
	public readonly string Id;

	// Token: 0x04006F94 RID: 28564
	public bool bActionSetEnabled;

	// Token: 0x04006F95 RID: 28565
	public bool isShowing;

	// Token: 0x04006F96 RID: 28566
	public bool isModal;

	// Token: 0x04006F97 RID: 28567
	public bool alwaysUsesMouseCursor;

	// Token: 0x04006F98 RID: 28568
	public bool isEscClosable;

	// Token: 0x04006F99 RID: 28569
	public bool isInputActive;

	// Token: 0x04006F9A RID: 28570
	public GUIWindowManager windowManager;

	// Token: 0x04006F9B RID: 28571
	public LocalPlayerUI playerUI;

	// Token: 0x04006F9C RID: 28572
	public string openWindowOnEsc = string.Empty;

	// Token: 0x04006F9D RID: 28573
	public Action OnWindowClose;
}
