using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001542 RID: 5442
[Preserve]
public class vp_Message : vp_Event
{
	// Token: 0x0600AA7E RID: 43646 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600AA7F RID: 43647 RVA: 0x003F7479 File Offset: 0x003F5679
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AA80 RID: 43648 RVA: 0x003F9868 File Offset: 0x003F7A68
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_Fields = new FieldInfo[] { base.GetType().GetField("Send") };
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[] { base.GetType().GetMethod("Empty") };
		this.m_DelegateTypes = new Type[] { typeof(vp_Message.Sender) };
		this.Prefixes = new Dictionary<string, int> { { "OnMessage_", 0 } };
		this.Send = new vp_Message.Sender(vp_Message.Empty);
	}

	// Token: 0x0600AA81 RID: 43649 RVA: 0x003F98FB File Offset: 0x003F7AFB
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		this.Send = (vp_Message.Sender)Delegate.Combine(this.Send, (vp_Message.Sender)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m));
		base.Refresh();
	}

	// Token: 0x0600AA82 RID: 43650 RVA: 0x003F992D File Offset: 0x003F7B2D
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007E3F RID: 32319
	[Preserve]
	public vp_Message.Sender Send;

	// Token: 0x02001543 RID: 5443
	// (Invoke) Token: 0x0600AA84 RID: 43652
	[Preserve]
	public delegate void Sender();
}
