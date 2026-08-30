using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	// Token: 0x02000300 RID: 768
	[UsedByNativeCode]
	[Serializable]
	public abstract class UnityEventBase : ISerializationCallbackReceiver
	{
		// Token: 0x06001FB8 RID: 8120 RVA: 0x00034864 File Offset: 0x00032A64
		protected UnityEventBase()
		{
			this.m_Calls = new InvokableCallList();
			this.m_PersistentCalls = new PersistentCallGroup();
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0003488B File Offset: 0x00032A8B
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.DirtyPersistentCalls();
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0003488B File Offset: 0x00032A8B
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.DirtyPersistentCalls();
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00034898 File Offset: 0x00032A98
		protected MethodInfo FindMethod_Impl(string name, object targetObj)
		{
			return this.FindMethod_Impl(name, targetObj.GetType());
		}

		// Token: 0x06001FBC RID: 8124
		protected abstract MethodInfo FindMethod_Impl(string name, Type targetObjType);

		// Token: 0x06001FBD RID: 8125
		internal abstract BaseInvokableCall GetDelegate(object target, MethodInfo theFunction);

		// Token: 0x06001FBE RID: 8126 RVA: 0x000348B8 File Offset: 0x00032AB8
		internal MethodInfo FindMethod(PersistentCall call)
		{
			Type type = typeof(Object);
			bool flag = !string.IsNullOrEmpty(call.arguments.unityObjectArgumentAssemblyTypeName);
			if (flag)
			{
				type = Type.GetType(call.arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object);
			}
			Type type2 = ((call.target != null) ? call.target.GetType() : Type.GetType(call.targetAssemblyTypeName, false));
			return this.FindMethod(call.methodName, type2, call.mode, type);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x00034948 File Offset: 0x00032B48
		internal MethodInfo FindMethod(string name, Type listenerType, PersistentListenerMode mode, Type argumentType)
		{
			MethodInfo methodInfo;
			switch (mode)
			{
			case PersistentListenerMode.EventDefined:
				methodInfo = this.FindMethod_Impl(name, listenerType);
				break;
			case PersistentListenerMode.Void:
				methodInfo = UnityEventBase.GetValidMethodInfo(listenerType, name, new Type[0]);
				break;
			case PersistentListenerMode.Object:
				methodInfo = UnityEventBase.GetValidMethodInfo(listenerType, name, new Type[] { argumentType ?? typeof(Object) });
				break;
			case PersistentListenerMode.Int:
				methodInfo = UnityEventBase.GetValidMethodInfo(listenerType, name, new Type[] { typeof(int) });
				break;
			case PersistentListenerMode.Float:
				methodInfo = UnityEventBase.GetValidMethodInfo(listenerType, name, new Type[] { typeof(float) });
				break;
			case PersistentListenerMode.String:
				methodInfo = UnityEventBase.GetValidMethodInfo(listenerType, name, new Type[] { typeof(string) });
				break;
			case PersistentListenerMode.Bool:
				methodInfo = UnityEventBase.GetValidMethodInfo(listenerType, name, new Type[] { typeof(bool) });
				break;
			default:
				methodInfo = null;
				break;
			}
			return methodInfo;
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x00034A40 File Offset: 0x00032C40
		internal int GetCallsCount()
		{
			return this.m_Calls.Count;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x00034A60 File Offset: 0x00032C60
		public int GetPersistentEventCount()
		{
			return this.m_PersistentCalls.Count;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x00034A80 File Offset: 0x00032C80
		public Object GetPersistentTarget(int index)
		{
			PersistentCall listener = this.m_PersistentCalls.GetListener(index);
			return (listener != null) ? listener.target : null;
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x00034AAC File Offset: 0x00032CAC
		public string GetPersistentMethodName(int index)
		{
			PersistentCall listener = this.m_PersistentCalls.GetListener(index);
			return (listener != null) ? listener.methodName : string.Empty;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x00034ADB File Offset: 0x00032CDB
		private void DirtyPersistentCalls()
		{
			this.m_Calls.ClearPersistent();
			this.m_CallsDirty = true;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x00034AF4 File Offset: 0x00032CF4
		private void RebuildPersistentCallsIfNeeded()
		{
			bool callsDirty = this.m_CallsDirty;
			if (callsDirty)
			{
				this.m_PersistentCalls.Initialize(this.m_Calls, this);
				this.m_CallsDirty = false;
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00034B28 File Offset: 0x00032D28
		public void SetPersistentListenerState(int index, UnityEventCallState state)
		{
			PersistentCall listener = this.m_PersistentCalls.GetListener(index);
			bool flag = listener != null;
			if (flag)
			{
				listener.callState = state;
			}
			this.DirtyPersistentCalls();
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00034B5C File Offset: 0x00032D5C
		public UnityEventCallState GetPersistentListenerState(int index)
		{
			bool flag = index < 0 || index > this.m_PersistentCalls.Count;
			if (flag)
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of the {1} persistent listeners.", index, this.GetPersistentEventCount()));
			}
			return this.m_PersistentCalls.GetListener(index).callState;
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00034BB9 File Offset: 0x00032DB9
		protected void AddListener(object targetObj, MethodInfo method)
		{
			this.m_Calls.AddListener(this.GetDelegate(targetObj, method));
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x00034BD0 File Offset: 0x00032DD0
		internal void AddCall(BaseInvokableCall call)
		{
			this.m_Calls.AddListener(call);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x00034BE0 File Offset: 0x00032DE0
		protected void RemoveListener(object targetObj, MethodInfo method)
		{
			this.m_Calls.RemoveListener(targetObj, method);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x00034BF1 File Offset: 0x00032DF1
		public void RemoveAllListeners()
		{
			this.m_Calls.Clear();
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x00034C00 File Offset: 0x00032E00
		internal List<BaseInvokableCall> PrepareInvoke()
		{
			this.RebuildPersistentCallsIfNeeded();
			return this.m_Calls.PrepareInvoke();
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x00034C24 File Offset: 0x00032E24
		protected void Invoke(object[] parameters)
		{
			List<BaseInvokableCall> list = this.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Invoke(parameters);
			}
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x00034C5C File Offset: 0x00032E5C
		public override string ToString()
		{
			return base.ToString() + " " + base.GetType().FullName;
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x00034C8C File Offset: 0x00032E8C
		public static MethodInfo GetValidMethodInfo(object obj, string functionName, Type[] argumentTypes)
		{
			return UnityEventBase.GetValidMethodInfo(obj.GetType(), functionName, argumentTypes);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x00034CAC File Offset: 0x00032EAC
		public static MethodInfo GetValidMethodInfo(Type objectType, string functionName, Type[] argumentTypes)
		{
			while (objectType != typeof(object) && objectType != null)
			{
				MethodInfo method = objectType.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, argumentTypes, null);
				bool flag = method != null;
				if (flag)
				{
					ParameterInfo[] parameters = method.GetParameters();
					bool flag2 = true;
					int num = 0;
					foreach (ParameterInfo parameterInfo in parameters)
					{
						Type type = argumentTypes[num];
						Type parameterType = parameterInfo.ParameterType;
						flag2 = type.IsPrimitive == parameterType.IsPrimitive;
						bool flag3 = !flag2;
						if (flag3)
						{
							break;
						}
						num++;
					}
					bool flag4 = flag2;
					if (flag4)
					{
						return method;
					}
				}
				objectType = objectType.BaseType;
			}
			return null;
		}

		// Token: 0x04000A80 RID: 2688
		private InvokableCallList m_Calls;

		// Token: 0x04000A81 RID: 2689
		[SerializeField]
		[FormerlySerializedAs("m_PersistentListeners")]
		private PersistentCallGroup m_PersistentCalls;

		// Token: 0x04000A82 RID: 2690
		private bool m_CallsDirty = true;
	}
}
