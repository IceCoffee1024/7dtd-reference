using System;
using UnityEngine.Scripting;

// Token: 0x02000F8B RID: 3979
[Preserve]
public class XUiC_MapPopupList : XUiController
{
	// Token: 0x06007DB2 RID: 32178 RVA: 0x002F9F44 File Offset: 0x002F8144
	public override void Init()
	{
		base.Init();
		this.children[0].OnPress += this.onPressEntry1;
		this.children[1].OnPress += this.onPressEntry2;
	}

	// Token: 0x06007DB3 RID: 32179 RVA: 0x002F9F91 File Offset: 0x002F8191
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPressEntry1(XUiController _sender, int _mouseButton)
	{
		XUiC_MapArea xuiC_MapArea = this.xui.GetWindow("mapArea").Controller as XUiC_MapArea;
		xuiC_MapArea.OnSetWaypoint();
		xuiC_MapArea.GetChildById("mapView").SelectCursorElement(true, false);
	}

	// Token: 0x06007DB4 RID: 32180 RVA: 0x002F9FC8 File Offset: 0x002F81C8
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
