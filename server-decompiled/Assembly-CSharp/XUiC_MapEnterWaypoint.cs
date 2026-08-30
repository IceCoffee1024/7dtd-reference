using System;
using UnityEngine.Scripting;

// Token: 0x02000F84 RID: 3972
[Preserve]
public class XUiC_MapEnterWaypoint : XUiController
{
	// Token: 0x06007D90 RID: 32144 RVA: 0x002F93F4 File Offset: 0x002F75F4
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

	// Token: 0x06007D91 RID: 32145 RVA: 0x002F945E File Offset: 0x002F765E
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnInputAbortedHandler(XUiController _sender)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).closeAllPopups();
	}

	// Token: 0x06007D92 RID: 32146 RVA: 0x002F947F File Offset: 0x002F767F
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnSubmitHandler(XUiController _sender, string _text)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).OnWaypointCreated(_text);
	}

	// Token: 0x06007D93 RID: 32147 RVA: 0x002F94A1 File Offset: 0x002F76A1
	public void Show(Vector2i _position)
	{
		XUiV_Window window = this.xui.GetWindow("mapAreaEnterWaypointName");
		window.Position = _position;
		window.IsVisible = true;
		this.txtInput.Text = string.Empty;
		this.txtInput.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x04005D16 RID: 23830
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;
}
