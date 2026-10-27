using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003F7 RID: 1015
[Preserve]
public struct EntityActivationCommand
{
	// Token: 0x06001E91 RID: 7825 RVA: 0x000B9C5C File Offset: 0x000B7E5C
	public EntityActivationCommand(string _commandId, string _icon, string _eventName = null, string _customCommandText = null)
	{
		this.commandId = _commandId;
		this.icon = _icon;
		this.eventName = _eventName;
		this.iconColor = Color.white;
		this.activateTime = -1f;
		this.enabled = true;
		this.commandText = this.commandId;
		if (!string.IsNullOrEmpty(_customCommandText))
		{
			this.commandText = _customCommandText;
		}
		this.commandText = Localization.Get("entitycommand_" + this.commandText, false, null);
	}

	// Token: 0x04001483 RID: 5251
	public string commandId;

	// Token: 0x04001484 RID: 5252
	public string icon;

	// Token: 0x04001485 RID: 5253
	public Color iconColor;

	// Token: 0x04001486 RID: 5254
	public bool enabled;

	// Token: 0x04001487 RID: 5255
	public string eventName;

	// Token: 0x04001488 RID: 5256
	public float activateTime;

	// Token: 0x04001489 RID: 5257
	public string commandText;
}
