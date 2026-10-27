using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

// Token: 0x02001595 RID: 5525
public class vp_Message<V, VResult> : vp_Message
{
	// Token: 0x0600AC98 RID: 44184 RVA: 0x00405FEC File Offset: 0x004041EC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public static TResult Empty<T, TResult>(T value)
	{
		return default(TResult);
	}

	// Token: 0x0600AC99 RID: 44185 RVA: 0x00405EDC File Offset: 0x004040DC
	[Preserve]
	public vp_Message(string name)
		: base(name)
	{
	}

	// Token: 0x0600AC9A RID: 44186 RVA: 0x00406004 File Offset: 0x00404204
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

	// Token: 0x0600AC9B RID: 44187 RVA: 0x004060C6 File Offset: 0x004042C6
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

	// Token: 0x0600AC9C RID: 44188 RVA: 0x00405EC5 File Offset: 0x004040C5
	[Preserve]
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.Refresh();
	}

	// Token: 0x04007FDE RID: 32734
	[Preserve]
	public new vp_Message<V, VResult>.Sender<V, VResult> Send;

	// Token: 0x02001596 RID: 5526
	// (Invoke) Token: 0x0600AC9E RID: 44190
	[Preserve]
	public delegate TResult Sender<T, TResult>(T value);
}
