using System;
using UnityEngine.Scripting;

// Token: 0x02000F93 RID: 3987
[Preserve]
public class XUiC_MapEnterWaypoint : XUiController
{
	// Token: 0x06007DA0 RID: 32160 RVA: 0x002FC498 File Offset: 0x002FA698
	public override void Init()
	{
		base.Init();
		this.txtInput = (XUiC_TextInput)this.windowGroup.Controller.GetChildById("waypointInput");
		if (this.txtInput != null)
		{
			this.txtInput.Text = string.Empty;
		}
		if (this.txtInput != null)
		{
			this.txtInput.OnSubmitHandler += this.waypointOnSubmitHandler;
		}
	}

	// Token: 0x06007DA1 RID: 32161 RVA: 0x002FC502 File Offset: 0x002FA702
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnInputAbortedHandler(XUiController _sender)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).closeAllPopups();
	}

	// Token: 0x06007DA2 RID: 32162 RVA: 0x002FC523 File Offset: 0x002FA723
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnSubmitHandler(XUiController _sender, string _text)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).OnWaypointCreated(_text);
	}

	// Token: 0x06007DA3 RID: 32163 RVA: 0x002FC545 File Offset: 0x002FA745
	public void Show(Vector2i _position)
	{
		XUiV_Window window = this.xui.GetWindow("mapAreaEnterWaypointName");
		window.Position = _position;
		window.IsVisible = true;
		this.txtInput.Text = string.Empty;
		this.txtInput.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x04005D3F RID: 23871
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;
}
