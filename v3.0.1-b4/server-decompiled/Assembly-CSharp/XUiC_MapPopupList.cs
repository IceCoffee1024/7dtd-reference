using System;
using UnityEngine.Scripting;

// Token: 0x02000F72 RID: 3954
[Preserve]
public class XUiC_MapPopupList : XUiController
{
	// Token: 0x06007CC0 RID: 31936 RVA: 0x002F5770 File Offset: 0x002F3970
	public override void Init()
	{
		base.Init();
		this.children[0].OnPress += this.onPressEntry1;
		this.children[1].OnPress += this.onPressEntry2;
	}

	// Token: 0x06007CC1 RID: 31937 RVA: 0x002F57BD File Offset: 0x002F39BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPressEntry1(XUiController _sender, int _mouseButton)
	{
		XUiC_MapArea xuiC_MapArea = this.xui.GetWindow("mapArea").Controller as XUiC_MapArea;
		xuiC_MapArea.OnSetWaypoint();
		xuiC_MapArea.GetChildById("mapView").SelectCursorElement(true, false);
	}

	// Token: 0x06007CC2 RID: 31938 RVA: 0x002F57F4 File Offset: 0x002F39F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPressEntry2(XUiController _sender, int _mouseButton)
	{
		if (_sender is XUiC_MapPopupEntry)
		{
			XUiV_Window window = this.xui.GetWindow("mapAreaSetWaypoint");
			XUiV_Window window2 = this.xui.GetWindow("mapAreaChooseWaypoint");
			int num = 46;
			window2.Position = window.Position + new Vector2i(199, -num);
			if (window2.Position.y < 0)
			{
				window2.Position = new Vector2i(window2.Position.x, window2.Position.y + window2.Size.y);
			}
			window2.IsVisible = true;
			window2.Controller.GetChildByType<XUiC_MapSubPopupList>().ResetList();
		}
	}
}
