using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Serialization
{
	// Token: 0x0200030C RID: 780
	[NativeHeader("Runtime/Serialize/ManagedReferenceUtility.h")]
	public sealed class ManagedReferenceUtility
	{
		// Token: 0x0600200A RID: 8202
		[NativeMethod("SetManagedReferenceIdForObject")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetManagedReferenceIdForObjectInternal(Object obj, object scriptObj, long refId);

		// Token: 0x0600200B RID: 8203 RVA: 0x00035480 File Offset: 0x00033680
		public static bool SetManagedReferenceIdForObject(Object obj, object scriptObj, long refId)
		{
			bool flag = scriptObj == null;
			bool flag2;
			if (flag)
			{
				flag2 = refId == -2L;
			}
			else
			{
				Type type = scriptObj.GetType();
				bool flag3 = type == typeof(Object) || type.IsSubclassOf(typeof(Object));
				if (flag3)
				{
					throw new InvalidOperationException("Cannot assign an object deriving from UnityEngine.Object to a managed reference. This is not supported.");
				}
				flag2 = ManagedReferenceUtility.SetManagedReferenceIdForObjectInternal(obj, scriptObj, refId);
			}
			return flag2;
		}

		// Token: 0x0600200C RID: 8204
		[NativeMethod("GetManagedReferenceIdForObject")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetManagedReferenceIdForObjectInternal(Object obj, object scriptObj);

		// Token: 0x0600200D RID: 8205 RVA: 0x000354E8 File Offset: 0x000336E8
		public static long GetManagedReferenceIdForObject(Object obj, object scriptObj)
		{
			return ManagedReferenceUtility.GetManagedReferenceIdForObjectInternal(obj, scriptObj);
		}

		// Token: 0x0600200E RID: 8206
		[NativeMethod("GetManagedReference")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object GetManagedReferenceInternal(Object obj, long id);

		// Token: 0x0600200F RID: 8207 RVA: 0x00035504 File Offset: 0x00033704
		public static object GetManagedReference(Object obj, long id)
		{
			return ManagedReferenceUtility.GetManagedReferenceInternal(obj, id);
		}

		// Token: 0x06002010 RID: 8208
		[NativeMethod("GetManagedReferenceIds")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long[] GetManagedReferenceIdsForObjectInternal(Object obj);

		// Token: 0x06002011 RID: 8209 RVA: 0x00035520 File Offset: 0x00033720
		public static long[] GetManagedReferenceIds(Object obj)
		{
			return ManagedReferenceUtility.GetManagedReferenceIdsForObjectInternal(obj);
		}

		// Token: 0x04000A89 RID: 2697
		public const long RefIdUnknown = -1L;

		// Token: 0x04000A8A RID: 2698
		public const long RefIdNull = -2L;
	}
}
