using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x0200155D RID: 5469
[Preserve]
public class vp_Message : vp_Event
{
	// Token: 0x0600AB7B RID: 43899 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600AB7C RID: 43900 RVA: 0x003FC7B9 File Offset: 0x003FA9B9
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x0600AB7D RID: 43901 RVA: 0x003FEBA8 File Offset: 0x003FCDA8
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

	// Token: 0x0600AB7E RID: 43902 RVA: 0x003FEC3B File Offset: 0x003FCE3B
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		this.Send = (vp_Message.Sender)Delegate.Combine(this.Send, (vp_Message.Sender)Delegate.CreateDelegate(this.m_DelegateTypes[v], t, m));
		base.Refresh();
	}

	// Token: 0x0600AB7F RID: 43903 RVA: 0x003FEC6D File Offset: 0x003FCE6D
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007F18 RID: 32536
	[Preserve]
	public vp_Message.Sender Send;

	// Token: 0x0200155E RID: 5470
	// (Invoke) Token: 0x0600AB81 RID: 43905
	[Preserve]
	public delegate void Sender();
}
