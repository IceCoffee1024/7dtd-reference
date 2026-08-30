using System;
using UnityEngine.Scripting;

// Token: 0x02000F6B RID: 3947
[Preserve]
public class XUiC_MapEnterWaypoint : XUiController
{
	// Token: 0x06007C9E RID: 31902 RVA: 0x002F4C20 File Offset: 0x002F2E20
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

	// Token: 0x06007C9F RID: 31903 RVA: 0x002F4C8A File Offset: 0x002F2E8A
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnInputAbortedHandler(XUiController _sender)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).closeAllPopups();
	}

	// Token: 0x06007CA0 RID: 31904 RVA: 0x002F4CAB File Offset: 0x002F2EAB
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnSubmitHandler(XUiController _sender, string _text)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).OnWaypointCreated(_text);
	}

	// Token: 0x06007CA1 RID: 31905 RVA: 0x002F4CCD File Offset: 0x002F2ECD
	public void Show(Vector2i _position)
	{
		XUiV_Window window = this.xui.GetWindow("mapAreaEnterWaypointName");
		window.Position = _position;
		window.IsVisible = true;
		this.txtInput.Text = string.Empty;
		this.txtInput.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x04005C4F RID: 23631
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;
}
