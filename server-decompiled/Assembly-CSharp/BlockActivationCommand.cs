using System;
using UnityEngine;

// Token: 0x02000105 RID: 261
public struct BlockActivationCommand
{
	// Token: 0x06000650 RID: 1616 RVA: 0x0002CCE5 File Offset: 0x0002AEE5
	public BlockActivationCommand(string _text, string _icon, bool _enabled, bool _highlighted = false, string _eventName = null)
	{
		this.text = _text;
		this.icon = _icon;
		this.enabled = _enabled;
		this.highlighted = _highlighted;
		this.eventName = _eventName;
		this.iconColor = Color.white;
		this.activateTime = -1f;
	}

	// Token: 0x040006FF RID: 1791
	public string text;

	// Token: 0x04000700 RID: 1792
	public string icon;

	// Token: 0x04000701 RID: 1793
	public Color iconColor;

	// Token: 0x04000702 RID: 1794
	public bool enabled;

	// Token: 0x04000703 RID: 1795
	public bool highlighted;

	// Token: 0x04000704 RID: 1796
	public string eventName;

	// Token: 0x04000705 RID: 1797
	public float activateTime;

	// Token: 0x04000706 RID: 1798
	public static readonly BlockActivationCommand[] Empty = Array.Empty<BlockActivationCommand>();
}
