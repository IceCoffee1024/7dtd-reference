using System;
using UnityEngine.Scripting;

// Token: 0x02000FA3 RID: 4003
[Preserve]
public class XUiC_MapEnterWaypoint : XUiController
{
	// Token: 0x06007DEF RID: 32239 RVA: 0x002FDFB4 File Offset: 0x002FC1B4
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

	// Token: 0x06007DF0 RID: 32240 RVA: 0x002FE01E File Offset: 0x002FC21E
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnInputAbortedHandler(XUiController _sender)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).closeAllPopups();
	}

	// Token: 0x06007DF1 RID: 32241 RVA: 0x002FE03F File Offset: 0x002FC23F
	[PublicizedFrom(EAccessModifier.Private)]
	public void waypointOnSubmitHandler(XUiController _sender, string _text)
	{
		((XUiC_MapArea)this.xui.GetWindow("mapArea").Controller).OnWaypointCreated(_text);
	}

	// Token: 0x06007DF2 RID: 32242 RVA: 0x002FE061 File Offset: 0x002FC261
	public void Show(Vector2i _position)
	{
		XUiV_Window window = this.xui.GetWindow("mapAreaEnterWaypointName");
		window.Position = _position;
		window.IsVisible = true;
		this.txtInput.Text = string.Empty;
		this.txtInput.SelectOrVirtualKeyboard(true);
	}

	// Token: 0x04005D7A RID: 23930
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;
}
