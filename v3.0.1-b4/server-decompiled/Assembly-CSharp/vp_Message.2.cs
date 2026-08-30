using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001544 RID: 5444
[Preserve]
public class vp_Message<V> : vp_Message
{
	// Token: 0x0600AA87 RID: 43655 RVA: 0x000027FC File Offset: 0x000009FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty<T>(T value)
	{
	}

	// Token: 0x0600AA88 RID: 43656 RVA: 0x003F9944 File Offset: 0x003F7B44
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AA89 RID: 43657 RVA: 0x003F9950 File Offset: 0x003F7B50
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

	// Token: 0x0600AA8A RID: 43658 RVA: 0x003F9A28 File Offset: 0x003F7C28
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

	// Token: 0x0600AA8B RID: 43659 RVA: 0x003F992D File Offset: 0x003F7B2D
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007E40 RID: 32320
	[Preserve]
	public new vp_Message<V>.Sender<V> Send;

	// Token: 0x02001545 RID: 5445
	// (Invoke) Token: 0x0600AA8D RID: 43661
	[Preserve]
	public delegate void Sender<T>(T value);
}
