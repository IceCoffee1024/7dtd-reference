using System;
using UnityEngine;

// Token: 0x02001200 RID: 4608
public abstract class GUIWindow
{
	// Token: 0x060092FA RID: 37626 RVA: 0x0037A20B File Offset: 0x0037840B
	[PublicizedFrom(EAccessModifier.Protected)]
	public GUIWindow(string _id)
	{
		this.Id = _id;
	}

	// Token: 0x060092FB RID: 37627 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnGUI()
	{
	}

	// Token: 0x060092FC RID: 37628 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x060092FD RID: 37629 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnOpen()
	{
	}

	// Token: 0x060092FE RID: 37630 RVA: 0x0037A225 File Offset: 0x00378425
	public virtual void OnClose()
	{
		Action onWindowClose = this.OnWindowClose;
		if (onWindowClose != null)
		{
			onWindowClose();
		}
		this.OnWindowClose = null;
	}

	// Token: 0x060092FF RID: 37631 RVA: 0x0037A23F File Offset: 0x0037843F
	public virtual PlayerActionsBase GetActionSet()
	{
		return this.playerUI.playerInput.GUIActions;
	}

	// Token: 0x06009300 RID: 37632 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool HasActionSet()
	{
		return true;
	}

	// Token: 0x06009301 RID: 37633 RVA: 0x0037A254 File Offset: 0x00378454
	public override bool Equals(object _obj)
	{
		GUIWindow guiwindow = _obj as GUIWindow;
		return guiwindow != null && guiwindow.Id.Equals(this.Id);
	}

	// Token: 0x06009302 RID: 37634 RVA: 0x0037A27E File Offset: 0x0037847E
	public override int GetHashCode()
	{
		return this.Id.GetHashCode();
	}

	// Token: 0x06009303 RID: 37635 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06009304 RID: 37636 RVA: 0x0037A28C File Offset: 0x0037848C
	public static Matrix4x4 UiScaleMatrix(out float _targetScale, out float _actualScale, float _x = 0f, float _y = 0f, float _clampMin = 0.4f, float _clampMax = 2f)
	{
		_targetScale = (float)Screen.height / 1080f;
		_targetScale *= GameOptionsManager.GetActiveUiScale();
		_actualScale = Utils.FastClamp(_targetScale, _clampMin, _clampMax);
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(new Vector3(_x, _y), Quaternion.identity, new Vector3(_actualScale, _actualScale, 1f));
		return matrix;
	}

	// Token: 0x04006E0F RID: 28175
	public readonly string Id;

	// Token: 0x04006E10 RID: 28176
	public bool bActionSetEnabled;

	// Token: 0x04006E11 RID: 28177
	public bool isShowing;

	// Token: 0x04006E12 RID: 28178
	public bool isModal;

	// Token: 0x04006E13 RID: 28179
	public bool alwaysUsesMouseCursor;

	// Token: 0x04006E14 RID: 28180
	public bool isEscClosable;

	// Token: 0x04006E15 RID: 28181
	public bool isInputActive;

	// Token: 0x04006E16 RID: 28182
	public GUIWindowManager windowManager;

	// Token: 0x04006E17 RID: 28183
	public LocalPlayerUI playerUI;

	// Token: 0x04006E18 RID: 28184
	public string openWindowOnEsc = string.Empty;

	// Token: 0x04006E19 RID: 28185
	public Action OnWindowClose;
}
