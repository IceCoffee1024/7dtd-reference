using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001546 RID: 5446
public class vp_Message<V, VResult> : vp_Message
{
	// Token: 0x0600AA90 RID: 43664 RVA: 0x003F9A54 File Offset: 0x003F7C54
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static TResult Empty<T, TResult>(T value)
	{
		return default(TResult);
	}

	// Token: 0x0600AA91 RID: 43665 RVA: 0x003F9944 File Offset: 0x003F7B44
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AA92 RID: 43666 RVA: 0x003F9A6C File Offset: 0x003F7C6C
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

	// Token: 0x0600AA93 RID: 43667 RVA: 0x003F9B2E File Offset: 0x003F7D2E
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

	// Token: 0x0600AA94 RID: 43668 RVA: 0x003F992D File Offset: 0x003F7B2D
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007E41 RID: 32321
	[Preserve]
	public new vp_Message<V, VResult>.Sender<V, VResult> Send;

	// Token: 0x02001547 RID: 5447
	// (Invoke) Token: 0x0600AA96 RID: 43670
	[Preserve]
	public delegate TResult Sender<T, TResult>(T value);
}
