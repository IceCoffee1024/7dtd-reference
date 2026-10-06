using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001583 RID: 5507
public class vp_Message<V, VResult> : vp_Message
{
	// Token: 0x0600AC40 RID: 44096 RVA: 0x00403BE8 File Offset: 0x00401DE8
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static TResult Empty<T, TResult>(T value)
	{
		return default(TResult);
	}

	// Token: 0x0600AC41 RID: 44097 RVA: 0x00403AD8 File Offset: 0x00401CD8
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AC42 RID: 44098 RVA: 0x00403C00 File Offset: 0x00401E00
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_Fields = new FieldInfo[] { base.GetType().GetField("Send") };
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[] { base.GetStaticGenericMethod(base.GetType(), "Empty", this.m_ArgumentType, this.m_ReturnType) };
		this.m_DelegateTypes = new Type[] { typeof(vp_Message<, >.Sender<, >) };
		this.Prefixes = new Dictionary<string, int> { { "OnMessage_", 0 } };
		if (this.m_DefaultMethods[0] != null)
		{
			base.SetFieldToLocalMethod(this.m_Fields[0], this.m_DefaultMethods[0], base.MakeGenericType(this.m_DelegateTypes[0]));
		}
	}

	// Token: 0x0600AC43 RID: 44099 RVA: 0x00403CC2 File Offset: 0x00401EC2
	[Preserve]
	public override void Register(object t, string m, int v)
	{
		if (m == null)
		{
			return;
		}
		base.AddExternalMethodToField(t, this.m_Fields[0], m, base.MakeGenericType(this.m_DelegateTypes[0]));
		base.Refresh();
	}

	// Token: 0x0600AC44 RID: 44100 RVA: 0x00403AC1 File Offset: 0x00401CC1
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007F9D RID: 32669
	[Preserve]
	public new vp_Message<V, VResult>.Sender<V, VResult> Send;

	// Token: 0x02001584 RID: 5508
	// (Invoke) Token: 0x0600AC46 RID: 44102
	[Preserve]
	public delegate TResult Sender<T, TResult>(T value);
}
