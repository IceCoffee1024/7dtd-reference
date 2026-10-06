using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001581 RID: 5505
[Preserve]
public class vp_Message<V> : vp_Message
{
	// Token: 0x0600AC37 RID: 44087 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x0600AC38 RID: 44088 RVA: 0x00403AD8 File Offset: 0x00401CD8
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AC39 RID: 44089 RVA: 0x00403AE4 File Offset: 0x00401CE4
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

	// Token: 0x0600AC3A RID: 44090 RVA: 0x00403BBC File Offset: 0x00401DBC
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

	// Token: 0x0600AC3B RID: 44091 RVA: 0x00403AC1 File Offset: 0x00401CC1
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007F9C RID: 32668
	[Preserve]
	public new vp_Message<V>.Sender<V> Send;

	// Token: 0x02001582 RID: 5506
	// (Invoke) Token: 0x0600AC3D RID: 44093
	[Preserve]
	public delegate void Sender<T>(T value);
}
