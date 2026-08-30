using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x0200155F RID: 5471
[Preserve]
public class vp_Message<V> : vp_Message
{
	// Token: 0x0600AB84 RID: 43908 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x0600AB85 RID: 43909 RVA: 0x003FEC84 File Offset: 0x003FCE84
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AB86 RID: 43910 RVA: 0x003FEC90 File Offset: 0x003FCE90
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

	// Token: 0x0600AB87 RID: 43911 RVA: 0x003FED68 File Offset: 0x003FCF68
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

	// Token: 0x0600AB88 RID: 43912 RVA: 0x003FEC6D File Offset: 0x003FCE6D
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007F19 RID: 32537
	[Preserve]
	public new vp_Message<V>.Sender<V> Send;

	// Token: 0x02001560 RID: 5472
	// (Invoke) Token: 0x0600AB8A RID: 43914
	[Preserve]
	public delegate void Sender<T>(T value);
}
