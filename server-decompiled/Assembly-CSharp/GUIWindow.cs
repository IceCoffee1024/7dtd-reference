using System;
using UnityEngine;

// Token: 0x02001234 RID: 4660
public abstract class GUIWindow
{
	// Token: 0x0600948C RID: 38028 RVA: 0x00383D2F File Offset: 0x00381F2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public GUIWindow(string _id)
	{
		this.Id = _id;
	}

	// Token: 0x0600948D RID: 38029 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnGUI()
	{
	}

	// Token: 0x0600948E RID: 38030 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Update()
	{
	}

	// Token: 0x0600948F RID: 38031 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnOpen()
	{
	}

	// Token: 0x06009490 RID: 38032 RVA: 0x00383D49 File Offset: 0x00381F49
	public virtual void OnClose()
	{
		Action onWindowClose = this.OnWindowClose;
		if (onWindowClose != null)
		{
			onWindowClose();
		}
		this.OnWindowClose = null;
	}

	// Token: 0x06009491 RID: 38033 RVA: 0x00383D63 File Offset: 0x00381F63
	public virtual PlayerActionsBase GetActionSet()
	{
		return this.playerUI.playerInput.GUIActions;
	}

	// Token: 0x06009492 RID: 38034 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool HasActionSet()
	{
		return true;
	}

	// Token: 0x06009493 RID: 38035 RVA: 0x00383D78 File Offset: 0x00381F78
	public override bool Equals(object _obj)
	{
		GUIWindow guiwindow = _obj as GUIWindow;
		return guiwindow != null && guiwindow.Id.Equals(this.Id);
	}

	// Token: 0x06009494 RID: 38036 RVA: 0x00383DA2 File Offset: 0x00381FA2
	public override int GetHashCode()
	{
		return this.Id.GetHashCode();
	}

	// Token: 0x06009495 RID: 38037 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup()
	{
	}

	// Token: 0x06009496 RID: 38038 RVA: 0x00383DB0 File Offset: 0x00381FB0
	public static Matrix4x4 UiScaleMatrix(out float _targetScale, out float _actualScale, float _x = 0f, float _y = 0f, float _clampMin = 0.4f, float _clampMax = 2f)
	{
		_targetScale = (float)Screen.height / 1080f;
		_targetScale *= GameOptionsManager.GetActiveUiScale();
		_actualScale = Utils.FastClamp(_targetScale, _clampMin, _clampMax);
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.TRS(new Vector3(_x, _y), Quaternion.identity, new Vector3(_actualScale, _actualScale, 1f));
		return matrix;
	}

	// Token: 0x04006F52 RID: 28498
	public readonly string Id;

	// Token: 0x04006F53 RID: 28499
	public bool bActionSetEnabled;

	// Token: 0x04006F54 RID: 28500
	public bool isShowing;

	// Token: 0x04006F55 RID: 28501
	public bool isModal;

	// Token: 0x04006F56 RID: 28502
	public bool alwaysUsesMouseCursor;

	// Token: 0x04006F57 RID: 28503
	public bool isEscClosable;

	// Token: 0x04006F58 RID: 28504
	public bool isInputActive;

	// Token: 0x04006F59 RID: 28505
	public GUIWindowManager windowManager;

	// Token: 0x04006F5A RID: 28506
	public LocalPlayerUI playerUI;

	// Token: 0x04006F5B RID: 28507
	public string openWindowOnEsc = string.Empty;

	// Token: 0x04006F5C RID: 28508
	public Action OnWindowClose;
}
