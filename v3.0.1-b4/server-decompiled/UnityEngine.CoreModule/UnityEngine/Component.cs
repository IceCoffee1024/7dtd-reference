using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x0200023C RID: 572
	[RequiredByNativeCode]
	[NativeClass("Unity::Component")]
	[NativeHeader("Runtime/Export/Scripting/Component.bindings.h")]
	public class Component : Object
	{
		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001872 RID: 6258
		public extern Transform transform
		{
			[FreeFunction("GetTransform", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001873 RID: 6259
		public extern GameObject gameObject
		{
			[FreeFunction("GetGameObject", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00028A2C File Offset: 0x00026C2C
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponent(Type type)
		{
			return this.gameObject.GetComponent(type);
		}

		// Token: 0x06001875 RID: 6261
		[FreeFunction(HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void GetComponentFastPath(Type type, IntPtr oneFurtherThanResultValue);

		// Token: 0x06001876 RID: 6262 RVA: 0x00028A4C File Offset: 0x00026C4C
		[SecuritySafeCritical]
		public unsafe T GetComponent<T>()
		{
			CastHelper<T> castHelper = default(CastHelper<T>);
			this.GetComponentFastPath(typeof(T), new IntPtr((void*)(&castHelper.onePointerFurtherThanT)));
			return castHelper.t;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00028A8C File Offset: 0x00026C8C
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public bool TryGetComponent(Type type, out Component component)
		{
			return this.gameObject.TryGetComponent(type, out component);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00028AAC File Offset: 0x00026CAC
		[SecuritySafeCritical]
		public bool TryGetComponent<T>(out T component)
		{
			return this.gameObject.TryGetComponent<T>(out component);
		}

		// Token: 0x06001879 RID: 6265
		[FreeFunction(HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Component GetComponent(string type);

		// Token: 0x0600187A RID: 6266 RVA: 0x00028ACC File Offset: 0x00026CCC
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t, bool includeInactive)
		{
			return this.gameObject.GetComponentInChildren(t, includeInactive);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00028AEC File Offset: 0x00026CEC
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInChildren(Type t)
		{
			return this.GetComponentInChildren(t, false);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00028B08 File Offset: 0x00026D08
		public T GetComponentInChildren<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)((object)this.GetComponentInChildren(typeof(T), includeInactive));
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00028B30 File Offset: 0x00026D30
		[ExcludeFromDocs]
		public T GetComponentInChildren<T>()
		{
			return (T)((object)this.GetComponentInChildren(typeof(T), false));
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00028B58 File Offset: 0x00026D58
		public Component[] GetComponentsInChildren(Type t, bool includeInactive)
		{
			return this.gameObject.GetComponentsInChildren(t, includeInactive);
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00028B78 File Offset: 0x00026D78
		[ExcludeFromDocs]
		public Component[] GetComponentsInChildren(Type t)
		{
			return this.gameObject.GetComponentsInChildren(t, false);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00028B98 File Offset: 0x00026D98
		public T[] GetComponentsInChildren<T>(bool includeInactive)
		{
			return this.gameObject.GetComponentsInChildren<T>(includeInactive);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00028BB6 File Offset: 0x00026DB6
		public void GetComponentsInChildren<T>(bool includeInactive, List<T> result)
		{
			this.gameObject.GetComponentsInChildren<T>(includeInactive, result);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00028BC8 File Offset: 0x00026DC8
		public T[] GetComponentsInChildren<T>()
		{
			return this.GetComponentsInChildren<T>(false);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x00028BE1 File Offset: 0x00026DE1
		public void GetComponentsInChildren<T>(List<T> results)
		{
			this.GetComponentsInChildren<T>(false, results);
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x00028BF0 File Offset: 0x00026DF0
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type t, bool includeInactive)
		{
			return this.gameObject.GetComponentInParent(t, includeInactive);
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x00028C10 File Offset: 0x00026E10
		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public Component GetComponentInParent(Type t)
		{
			return this.gameObject.GetComponentInParent(t, false);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00028C30 File Offset: 0x00026E30
		public T GetComponentInParent<T>([DefaultValue("false")] bool includeInactive)
		{
			return (T)((object)this.GetComponentInParent(typeof(T), includeInactive));
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00028C58 File Offset: 0x00026E58
		public T GetComponentInParent<T>()
		{
			return (T)((object)this.GetComponentInParent(typeof(T), false));
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00028C80 File Offset: 0x00026E80
		public Component[] GetComponentsInParent(Type t, [DefaultValue("false")] bool includeInactive)
		{
			return this.gameObject.GetComponentsInParent(t, includeInactive);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00028CA0 File Offset: 0x00026EA0
		[ExcludeFromDocs]
		public Component[] GetComponentsInParent(Type t)
		{
			return this.GetComponentsInParent(t, false);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00028CBC File Offset: 0x00026EBC
		public T[] GetComponentsInParent<T>(bool includeInactive)
		{
			return this.gameObject.GetComponentsInParent<T>(includeInactive);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00028CDA File Offset: 0x00026EDA
		public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
		{
			this.gameObject.GetComponentsInParent<T>(includeInactive, results);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00028CEC File Offset: 0x00026EEC
		public T[] GetComponentsInParent<T>()
		{
			return this.GetComponentsInParent<T>(false);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00028D08 File Offset: 0x00026F08
		public Component[] GetComponents(Type type)
		{
			return this.gameObject.GetComponents(type);
		}

		// Token: 0x0600188E RID: 6286
		[FreeFunction(HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetComponentsForListInternal(Type searchType, object resultList);

		// Token: 0x0600188F RID: 6287 RVA: 0x00028D26 File Offset: 0x00026F26
		public void GetComponents(Type type, List<Component> results)
		{
			this.GetComponentsForListInternal(type, results);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00028D32 File Offset: 0x00026F32
		public void GetComponents<T>(List<T> results)
		{
			this.GetComponentsForListInternal(typeof(T), results);
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001891 RID: 6289 RVA: 0x00028D47 File Offset: 0x00026F47
		// (set) Token: 0x06001892 RID: 6290 RVA: 0x00028D54 File Offset: 0x00026F54
		public string tag
		{
			get
			{
				return this.gameObject.tag;
			}
			set
			{
				this.gameObject.tag = value;
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00028D64 File Offset: 0x00026F64
		public T[] GetComponents<T>()
		{
			return this.gameObject.GetComponents<T>();
		}

		// Token: 0x06001894 RID: 6292
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetComponentIndex();

		// Token: 0x06001895 RID: 6293 RVA: 0x00028D84 File Offset: 0x00026F84
		public bool CompareTag(string tag)
		{
			return this.gameObject.CompareTag(tag);
		}

		// Token: 0x06001896 RID: 6294
		[FreeFunction(HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendMessageUpwards(string methodName, [DefaultValue("null")] object value, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x06001897 RID: 6295 RVA: 0x00028DA2 File Offset: 0x00026FA2
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName, object value)
		{
			this.SendMessageUpwards(methodName, value, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00028DAF File Offset: 0x00026FAF
		[ExcludeFromDocs]
		public void SendMessageUpwards(string methodName)
		{
			this.SendMessageUpwards(methodName, null, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00028DBC File Offset: 0x00026FBC
		public void SendMessageUpwards(string methodName, SendMessageOptions options)
		{
			this.SendMessageUpwards(methodName, null, options);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00028DC9 File Offset: 0x00026FC9
		public void SendMessage(string methodName, object value)
		{
			this.SendMessage(methodName, value, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00028DD6 File Offset: 0x00026FD6
		public void SendMessage(string methodName)
		{
			this.SendMessage(methodName, null, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x0600189C RID: 6300
		[FreeFunction("SendMessage", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendMessage(string methodName, object value, SendMessageOptions options);

		// Token: 0x0600189D RID: 6301 RVA: 0x00028DE3 File Offset: 0x00026FE3
		public void SendMessage(string methodName, SendMessageOptions options)
		{
			this.SendMessage(methodName, null, options);
		}

		// Token: 0x0600189E RID: 6302
		[FreeFunction("BroadcastMessage", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BroadcastMessage(string methodName, [DefaultValue("null")] object parameter, [DefaultValue("SendMessageOptions.RequireReceiver")] SendMessageOptions options);

		// Token: 0x0600189F RID: 6303 RVA: 0x00028DF0 File Offset: 0x00026FF0
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName, object parameter)
		{
			this.BroadcastMessage(methodName, parameter, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00028DFD File Offset: 0x00026FFD
		[ExcludeFromDocs]
		public void BroadcastMessage(string methodName)
		{
			this.BroadcastMessage(methodName, null, SendMessageOptions.RequireReceiver);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00028E0A File Offset: 0x0002700A
		public void BroadcastMessage(string methodName, SendMessageOptions options)
		{
			this.BroadcastMessage(methodName, null, options);
		}
	}
}
