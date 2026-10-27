using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001593 RID: 5523
[Preserve]
public class vp_Message<V> : vp_Message
{
	// Token: 0x0600AC8F RID: 44175 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x0600AC90 RID: 44176 RVA: 0x00405EDC File Offset: 0x004040DC
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AC91 RID: 44177 RVA: 0x00405EE8 File Offset: 0x004040E8
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_Fields = new FieldInfo[] { base.GetType().GetField("Send") };
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[] { base.GetStaticGenericMethod(base.GetType(), "Empty", this.m_ArgumentType, typeof(void)) };
		this.m_DelegateTypes = new Type[] { typeof(vp_Message<>.Sender<>) };
		this.Prefixes = new Dictionary<string, int> { { "OnMessage_", 0 } };
		this.Send = new vp_Message<V>.Sender<V>(vp_Message<V>.Empty<V>);
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x0600AC92 RID: 44178 RVA: 0x00405FC0 File Offset: 0x004041C0
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		if (m == null)
		{
			return;
		}
		base.AddExternalMethodToField(t, this.m_Fields[v], m, base.MakeGenericType(this.m_DelegateTypes[v]));
		base.Refresh();
	}

	// Token: 0x0600AC93 RID: 44179 RVA: 0x00405EC5 File Offset: 0x004040C5
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007FDD RID: 32733
	[Preserve]
	public new vp_Message<V>.Sender<V> Send;

	// Token: 0x02001594 RID: 5524
	// (Invoke) Token: 0x0600AC95 RID: 44181
	[Preserve]
	public delegate void Sender<T>(T value);
}
