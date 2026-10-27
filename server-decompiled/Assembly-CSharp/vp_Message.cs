using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001591 RID: 5521
[Preserve]
public class vp_Message : vp_Event
{
	// Token: 0x0600AC86 RID: 44166 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600AC87 RID: 44167 RVA: 0x00403A11 File Offset: 0x00401C11
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AC88 RID: 44168 RVA: 0x00405E00 File Offset: 0x00404000
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

	// Token: 0x0600AC89 RID: 44169 RVA: 0x00405E93 File Offset: 0x00404093
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		this.Send = (vp_Message.Sender)Delegate.Combine(this.Send, (vp_Message.Sender)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m));
		base.Refresh();
	}

	// Token: 0x0600AC8A RID: 44170 RVA: 0x00405EC5 File Offset: 0x004040C5
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007FDC RID: 32732
	[Preserve]
	public vp_Message.Sender Send;

	// Token: 0x02001592 RID: 5522
	// (Invoke) Token: 0x0600AC8C RID: 44172
	[Preserve]
	public delegate void Sender();
}
