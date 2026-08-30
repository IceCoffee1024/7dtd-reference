using System;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	// Token: 0x020002FD RID: 765
	[Serializable]
	internal class PersistentCall : ISerializationCallbackReceiver
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x00033F6C File Offset: 0x0003216C
		public Object target
		{
			get
			{
				return this.m_Target;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x00033F84 File Offset: 0x00032184
		public string targetAssemblyTypeName
		{
			get
			{
				bool flag = string.IsNullOrEmpty(this.m_TargetAssemblyTypeName) && this.m_Target != null;
				if (flag)
				{
					this.m_TargetAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_Target.GetType().AssemblyQualifiedName);
				}
				return this.m_TargetAssemblyTypeName;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x00033FDC File Offset: 0x000321DC
		public string methodName
		{
			get
			{
				return this.m_MethodName;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x00033FF4 File Offset: 0x000321F4
		// (set) Token: 0x06001F92 RID: 8082 RVA: 0x0003400C File Offset: 0x0003220C
		public PersistentListenerMode mode
		{
			get
			{
				return this.m_Mode;
			}
			set
			{
				this.m_Mode = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x00034018 File Offset: 0x00032218
		public ArgumentCache arguments
		{
			get
			{
				return this.m_Arguments;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001F94 RID: 8084 RVA: 0x00034030 File Offset: 0x00032230
		// (set) Token: 0x06001F95 RID: 8085 RVA: 0x00034048 File Offset: 0x00032248
		public UnityEventCallState callState
		{
			get
			{
				return this.m_CallState;
			}
			set
			{
				this.m_CallState = value;
			}
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00034054 File Offset: 0x00032254
		public bool IsValid()
		{
			return !string.IsNullOrEmpty(this.targetAssemblyTypeName) && !string.IsNullOrEmpty(this.methodName);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x00034084 File Offset: 0x00032284
		public BaseInvokableCall GetRuntimeCall(UnityEventBase theEvent)
		{
			bool flag = this.m_CallState == UnityEventCallState.Off || theEvent == null;
			BaseInvokableCall baseInvokableCall;
			if (flag)
			{
				baseInvokableCall = null;
			}
			else
			{
				MethodInfo methodInfo = theEvent.FindMethod(this);
				bool flag2 = methodInfo == null;
				if (flag2)
				{
					baseInvokableCall = null;
				}
				else
				{
					bool flag3 = !methodInfo.IsStatic && this.target == null;
					if (flag3)
					{
						baseInvokableCall = null;
					}
					else
					{
						Object @object = (methodInfo.IsStatic ? null : this.target);
						switch (this.m_Mode)
						{
						case PersistentListenerMode.EventDefined:
							baseInvokableCall = theEvent.GetDelegate(@object, methodInfo);
							break;
						case PersistentListenerMode.Void:
							baseInvokableCall = new InvokableCall(@object, methodInfo);
							break;
						case PersistentListenerMode.Object:
							baseInvokableCall = PersistentCall.GetObjectCall(@object, methodInfo, this.m_Arguments);
							break;
						case PersistentListenerMode.Int:
							baseInvokableCall = new CachedInvokableCall<int>(@object, methodInfo, this.m_Arguments.intArgument);
							break;
						case PersistentListenerMode.Float:
							baseInvokableCall = new CachedInvokableCall<float>(@object, methodInfo, this.m_Arguments.floatArgument);
							break;
						case PersistentListenerMode.String:
							baseInvokableCall = new CachedInvokableCall<string>(@object, methodInfo, this.m_Arguments.stringArgument);
							break;
						case PersistentListenerMode.Bool:
							baseInvokableCall = new CachedInvokableCall<bool>(@object, methodInfo, this.m_Arguments.boolArgument);
							break;
						default:
							baseInvokableCall = null;
							break;
						}
					}
				}
			}
			return baseInvokableCall;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000341B0 File Offset: 0x000323B0
		private static BaseInvokableCall GetObjectCall(Object target, MethodInfo method, ArgumentCache arguments)
		{
			Type type = typeof(Object);
			bool flag = !string.IsNullOrEmpty(arguments.unityObjectArgumentAssemblyTypeName);
			if (flag)
			{
				type = Type.GetType(arguments.unityObjectArgumentAssemblyTypeName, false) ?? typeof(Object);
			}
			Type typeFromHandle = typeof(CachedInvokableCall<>);
			Type type2 = typeFromHandle.MakeGenericType(new Type[] { type });
			ConstructorInfo constructor = type2.GetConstructor(new Type[]
			{
				typeof(Object),
				typeof(MethodInfo),
				type
			});
			Object @object = arguments.unityObjectArgument;
			bool flag2 = @object != null && !type.IsAssignableFrom(@object.GetType());
			if (flag2)
			{
				@object = null;
			}
			return constructor.Invoke(new object[] { target, method, @object }) as BaseInvokableCall;
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x00034291 File Offset: 0x00032491
		public void RegisterPersistentListener(Object ttarget, Type targetType, string mmethodName)
		{
			this.m_Target = ttarget;
			this.m_TargetAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(targetType.AssemblyQualifiedName);
			this.m_MethodName = mmethodName;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000342B3 File Offset: 0x000324B3
		public void UnregisterPersistentListener()
		{
			this.m_MethodName = string.Empty;
			this.m_Target = null;
			this.m_TargetAssemblyTypeName = string.Empty;
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000342D3 File Offset: 0x000324D3
		public void OnBeforeSerialize()
		{
			this.m_TargetAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_TargetAssemblyTypeName);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000342D3 File Offset: 0x000324D3
		public void OnAfterDeserialize()
		{
			this.m_TargetAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_TargetAssemblyTypeName);
		}

		// Token: 0x04000A75 RID: 2677
		[FormerlySerializedAs("instance")]
		[SerializeField]
		private Object m_Target;

		// Token: 0x04000A76 RID: 2678
		[SerializeField]
		private string m_TargetAssemblyTypeName;

		// Token: 0x04000A77 RID: 2679
		[FormerlySerializedAs("methodName")]
		[SerializeField]
		private string m_MethodName;

		// Token: 0x04000A78 RID: 2680
		[FormerlySerializedAs("mode")]
		[SerializeField]
		private PersistentListenerMode m_Mode = PersistentListenerMode.EventDefined;

		// Token: 0x04000A79 RID: 2681
		[FormerlySerializedAs("arguments")]
		[SerializeField]
		private ArgumentCache m_Arguments = new ArgumentCache();

		// Token: 0x04000A7A RID: 2682
		[FormerlySerializedAs("enabled")]
		[FormerlySerializedAs("m_Enabled")]
		[SerializeField]
		private UnityEventCallState m_CallState = UnityEventCallState.RuntimeOnly;
	}
}
