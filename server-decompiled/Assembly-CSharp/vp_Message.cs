using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x0200157F RID: 5503
[Preserve]
public class vp_Message : vp_Event
{
	// Token: 0x0600AC2E RID: 44078 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600AC2F RID: 44079 RVA: 0x0040160D File Offset: 0x003FF80D
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AC30 RID: 44080 RVA: 0x004039FC File Offset: 0x00401BFC
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

	// Token: 0x0600AC31 RID: 44081 RVA: 0x00403A8F File Offset: 0x00401C8F
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		this.Send = (vp_Message.Sender)Delegate.Combine(this.Send, (vp_Message.Sender)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m));
		base.Refresh();
	}

	// Token: 0x0600AC32 RID: 44082 RVA: 0x00403AC1 File Offset: 0x00401CC1
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007F9B RID: 32667
	[Preserve]
	public vp_Message.Sender Send;

	// Token: 0x02001580 RID: 5504
	// (Invoke) Token: 0x0600AC34 RID: 44084
	[Preserve]
	public delegate void Sender();
}
