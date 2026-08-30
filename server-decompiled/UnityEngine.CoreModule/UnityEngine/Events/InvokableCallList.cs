using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002FF RID: 767
	internal class InvokableCallList
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x0003465C File Offset: 0x0003285C
		public int Count
		{
			get
			{
				return this.m_PersistentCalls.Count + this.m_RuntimeCalls.Count;
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x00034685 File Offset: 0x00032885
		public void AddPersistentInvokableCall(BaseInvokableCall call)
		{
			this.m_PersistentCalls.Add(call);
			this.m_NeedsUpdate = true;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x0003469C File Offset: 0x0003289C
		public void AddListener(BaseInvokableCall call)
		{
			this.m_RuntimeCalls.Add(call);
			this.m_NeedsUpdate = true;
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000346B4 File Offset: 0x000328B4
		public void RemoveListener(object targetObj, MethodInfo method)
		{
			List<BaseInvokableCall> list = new List<BaseInvokableCall>();
			for (int i = 0; i < this.m_RuntimeCalls.Count; i++)
			{
				bool flag = this.m_RuntimeCalls[i].Find(targetObj, method);
				if (flag)
				{
					list.Add(this.m_RuntimeCalls[i]);
				}
			}
			this.m_RuntimeCalls.RemoveAll(new Predicate<BaseInvokableCall>(list.Contains));
			List<BaseInvokableCall> list2 = new List<BaseInvokableCall>(this.m_PersistentCalls.Count + this.m_RuntimeCalls.Count);
			list2.AddRange(this.m_PersistentCalls);
			list2.AddRange(this.m_RuntimeCalls);
			this.m_ExecutingCalls = list2;
			this.m_NeedsUpdate = false;
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x00034770 File Offset: 0x00032970
		public void Clear()
		{
			this.m_RuntimeCalls.Clear();
			List<BaseInvokableCall> list = new List<BaseInvokableCall>(this.m_PersistentCalls);
			this.m_ExecutingCalls = list;
			this.m_NeedsUpdate = false;
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000347A4 File Offset: 0x000329A4
		public void ClearPersistent()
		{
			this.m_PersistentCalls.Clear();
			List<BaseInvokableCall> list = new List<BaseInvokableCall>(this.m_RuntimeCalls);
			this.m_ExecutingCalls = list;
			this.m_NeedsUpdate = false;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000347D8 File Offset: 0x000329D8
		public List<BaseInvokableCall> PrepareInvoke()
		{
			bool needsUpdate = this.m_NeedsUpdate;
			if (needsUpdate)
			{
				this.m_ExecutingCalls.Clear();
				this.m_ExecutingCalls.AddRange(this.m_PersistentCalls);
				this.m_ExecutingCalls.AddRange(this.m_RuntimeCalls);
				this.m_NeedsUpdate = false;
			}
			return this.m_ExecutingCalls;
		}

		// Token: 0x04000A7C RID: 2684
		private readonly List<BaseInvokableCall> m_PersistentCalls = new List<BaseInvokableCall>();

		// Token: 0x04000A7D RID: 2685
		private readonly List<BaseInvokableCall> m_RuntimeCalls = new List<BaseInvokableCall>();

		// Token: 0x04000A7E RID: 2686
		private List<BaseInvokableCall> m_ExecutingCalls = new List<BaseInvokableCall>();

		// Token: 0x04000A7F RID: 2687
		private bool m_NeedsUpdate = true;
	}
}
