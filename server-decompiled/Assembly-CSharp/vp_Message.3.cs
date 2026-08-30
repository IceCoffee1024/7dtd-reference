using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001561 RID: 5473
public class vp_Message<V, VResult> : vp_Message
{
	// Token: 0x0600AB8D RID: 43917 RVA: 0x003FED94 File Offset: 0x003FCF94
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static TResult Empty<T, TResult>(T value)
	{
		return default(TResult);
	}

	// Token: 0x0600AB8E RID: 43918 RVA: 0x003FEC84 File Offset: 0x003FCE84
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AB8F RID: 43919 RVA: 0x003FEDAC File Offset: 0x003FCFAC
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

	// Token: 0x0600AB90 RID: 43920 RVA: 0x003FEE6E File Offset: 0x003FD06E
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

	// Token: 0x0600AB91 RID: 43921 RVA: 0x003FEC6D File Offset: 0x003FCE6D
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007F1A RID: 32538
	[Preserve]
	public new vp_Message<V, VResult>.Sender<V, VResult> Send;

	// Token: 0x02001562 RID: 5474
	// (Invoke) Token: 0x0600AB93 RID: 43923
	[Preserve]
	public delegate TResult Sender<T, TResult>(T value);
}
