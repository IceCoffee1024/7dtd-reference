using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001FB RID: 507
	[RequiredByNativeCode]
	[StaticAccessor("GetObjectDispatcher()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Misc/ObjectDispatcher.h")]
	internal sealed class ObjectDispatcher : IDisposable
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x00026664 File Offset: 0x00024864
		public bool valid
		{
			get
			{
				return this.m_Ptr != IntPtr.Zero;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00026688 File Offset: 0x00024888
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x000266AC File Offset: 0x000248AC
		public int maxDispatchHistoryFramesCount
		{
			get
			{
				this.ValidateSystemHandleAndThrow();
				return ObjectDispatcher.GetMaxDispatchHistoryFramesCount(this.m_Ptr);
			}
			set
			{
				this.ValidateSystemHandleAndThrow();
				ObjectDispatcher.SetMaxDispatchHistoryFramesCount(this.m_Ptr, value);
			}
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x000266C3 File Offset: 0x000248C3
		public ObjectDispatcher()
		{
			this.m_Ptr = ObjectDispatcher.CreateDispatchSystemHandle();
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x000266E4 File Offset: 0x000248E4
		~ObjectDispatcher()
		{
			this.Dispose(false);
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x00026718 File Offset: 0x00024918
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0002672C File Offset: 0x0002492C
		private void Dispose(bool disposing)
		{
			bool flag = this.m_Ptr != IntPtr.Zero;
			if (flag)
			{
				ObjectDispatcher.DestroyDispatchSystemHandle(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00026768 File Offset: 0x00024968
		private void ValidateSystemHandleAndThrow()
		{
			bool flag = !this.valid;
			if (flag)
			{
				throw new Exception("The ObjectDispatcher is invalid or has been disposed.");
			}
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x00026790 File Offset: 0x00024990
		private void ValidateTypeAndThrow(Type type)
		{
			bool flag = !type.IsSubclassOf(typeof(Object));
			if (flag)
			{
				throw new Exception("Only types inherited from UnityEngine.Object are supported.");
			}
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000267C0 File Offset: 0x000249C0
		private void ValidateComponentTypeAndThrow(Type type)
		{
			bool flag = !type.IsSubclassOf(typeof(Component));
			if (flag)
			{
				throw new Exception("Only types inherited from UnityEngine.Component are supported.");
			}
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x000267F0 File Offset: 0x000249F0
		public void DispatchTypeChangesAndClear(Type type, Action<TypeDispatchData> callback, bool sortByInstanceID = false, bool noScriptingArray = false)
		{
			this.ValidateSystemHandleAndThrow();
			this.ValidateTypeAndThrow(type);
			ObjectDispatcher.DispatchTypeChangesAndClear(this.m_Ptr, type, ObjectDispatcher.s_TypeDispatch, sortByInstanceID, noScriptingArray, callback);
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00026818 File Offset: 0x00024A18
		public void DispatchTransformChangesAndClear(Type type, ObjectDispatcher.TransformTrackingType trackingType, Action<Component[]> callback, bool sortByInstanceID = false)
		{
			this.ValidateSystemHandleAndThrow();
			this.ValidateComponentTypeAndThrow(type);
			ObjectDispatcher.DispatchTransformChangesAndClear(this.m_Ptr, type, trackingType, callback, sortByInstanceID);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0002683B File Offset: 0x00024A3B
		public void DispatchTransformChangesAndClear(Type type, ObjectDispatcher.TransformTrackingType trackingType, Action<TransformDispatchData> callback)
		{
			this.ValidateSystemHandleAndThrow();
			this.ValidateComponentTypeAndThrow(type);
			ObjectDispatcher.DispatchTransformDataChangesAndClear(this.m_Ptr, type, trackingType, ObjectDispatcher.s_TransformDispatch, callback);
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00026861 File Offset: 0x00024A61
		public void ClearTypeChanges(Type type)
		{
			this.ValidateSystemHandleAndThrow();
			this.ValidateTypeAndThrow(type);
			ObjectDispatcher.DispatchTypeChangesAndClear(this.m_Ptr, type, null, false, false, null);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00026884 File Offset: 0x00024A84
		public TypeDispatchData GetTypeChangesAndClear(Type type, Allocator allocator, bool sortByInstanceID = false, bool noScriptingArray = false)
		{
			TypeDispatchData dispatchData = default(TypeDispatchData);
			this.DispatchTypeChangesAndClear(type, delegate(TypeDispatchData data)
			{
				dispatchData.changed = data.changed;
				dispatchData.changedID = new NativeArray<int>(data.changedID, allocator);
				dispatchData.destroyedID = new NativeArray<int>(data.destroyedID, allocator);
			}, sortByInstanceID, noScriptingArray);
			return dispatchData;
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x000268CC File Offset: 0x00024ACC
		public void GetTypeChangesAndClear(Type type, List<Object> changed, out NativeArray<int> changedID, out NativeArray<int> destroyedID, Allocator allocator, bool sortByInstanceID = false)
		{
			TypeDispatchData dispatchData = default(TypeDispatchData);
			this.DispatchTypeChangesAndClear(type, delegate(TypeDispatchData data)
			{
				dispatchData.changedID = new NativeArray<int>(data.changedID, allocator);
				dispatchData.destroyedID = new NativeArray<int>(data.destroyedID, allocator);
			}, sortByInstanceID, true);
			changedID = dispatchData.changedID;
			destroyedID = dispatchData.destroyedID;
			Resources.InstanceIDToObjectList(dispatchData.changedID, changed);
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x00026940 File Offset: 0x00024B40
		public Component[] GetTransformChangesAndClear(Type type, ObjectDispatcher.TransformTrackingType trackingType, bool sortByInstanceID = false)
		{
			Component[] dispatchData = null;
			this.DispatchTransformChangesAndClear(type, trackingType, delegate(Component[] instances)
			{
				dispatchData = instances;
			}, sortByInstanceID);
			return dispatchData;
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0002697C File Offset: 0x00024B7C
		public TransformDispatchData GetTransformChangesAndClear(Type type, ObjectDispatcher.TransformTrackingType trackingType, Allocator allocator)
		{
			TransformDispatchData dispatchData = default(TransformDispatchData);
			this.DispatchTransformChangesAndClear(type, trackingType, delegate(TransformDispatchData data)
			{
				dispatchData.transformedID = new NativeArray<int>(data.transformedID, allocator);
				dispatchData.parentID = new NativeArray<int>(data.parentID, allocator);
				dispatchData.localToWorldMatrices = new NativeArray<Matrix4x4>(data.localToWorldMatrices, allocator);
				dispatchData.positions = new NativeArray<Vector3>(data.positions, allocator);
				dispatchData.rotations = new NativeArray<Quaternion>(data.rotations, allocator);
				dispatchData.scales = new NativeArray<Vector3>(data.scales, allocator);
			});
			return dispatchData;
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x000269C4 File Offset: 0x00024BC4
		public void EnableTypeTracking(ObjectDispatcher.TypeTrackingFlags typeTrackingMask, params Type[] types)
		{
			this.ValidateSystemHandleAndThrow();
			foreach (Type type in types)
			{
				this.ValidateTypeAndThrow(type);
				ObjectDispatcher.EnableTypeTracking(this.m_Ptr, type, typeTrackingMask);
			}
		}

		// Token: 0x0600172D RID: 5933 RVA: 0x00026A06 File Offset: 0x00024C06
		public void EnableTypeTracking(params Type[] types)
		{
			this.EnableTypeTracking(ObjectDispatcher.TypeTrackingFlags.Default, types);
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x00026A06 File Offset: 0x00024C06
		[Obsolete("EnableTypeTrackingIncludingAssets is deprecated, please use EnableTypeTracking and provide the flag that specifies whether you need assets or not.", false)]
		public void EnableTypeTrackingIncludingAssets(params Type[] types)
		{
			this.EnableTypeTracking(ObjectDispatcher.TypeTrackingFlags.Default, types);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x00026A14 File Offset: 0x00024C14
		public void DisableTypeTracking(params Type[] types)
		{
			this.ValidateSystemHandleAndThrow();
			foreach (Type type in types)
			{
				this.ValidateTypeAndThrow(type);
				ObjectDispatcher.DisableTypeTracking(this.m_Ptr, type);
			}
		}

		// Token: 0x06001730 RID: 5936 RVA: 0x00026A58 File Offset: 0x00024C58
		public void EnableTransformTracking(ObjectDispatcher.TransformTrackingType trackingType, params Type[] types)
		{
			this.ValidateSystemHandleAndThrow();
			foreach (Type type in types)
			{
				this.ValidateComponentTypeAndThrow(type);
				ObjectDispatcher.EnableTransformTracking(this.m_Ptr, type, trackingType);
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x00026A9C File Offset: 0x00024C9C
		public void DisableTransformTracking(ObjectDispatcher.TransformTrackingType trackingType, params Type[] types)
		{
			this.ValidateSystemHandleAndThrow();
			foreach (Type type in types)
			{
				this.ValidateComponentTypeAndThrow(type);
				ObjectDispatcher.DisableTransformTracking(this.m_Ptr, type, trackingType);
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00026ADE File Offset: 0x00024CDE
		public void DispatchTypeChangesAndClear<T>(Action<TypeDispatchData> callback, bool sortByInstanceID = false, bool noScriptingArray = false) where T : Object
		{
			this.DispatchTypeChangesAndClear(typeof(T), callback, sortByInstanceID, noScriptingArray);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00026AF5 File Offset: 0x00024CF5
		public void DispatchTransformChangesAndClear<T>(ObjectDispatcher.TransformTrackingType trackingType, Action<Component[]> callback, bool sortByInstanceID = false) where T : Object
		{
			this.DispatchTransformChangesAndClear(typeof(T), trackingType, callback, sortByInstanceID);
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00026B0C File Offset: 0x00024D0C
		public void DispatchTransformChangesAndClear<T>(ObjectDispatcher.TransformTrackingType trackingType, Action<TransformDispatchData> callback) where T : Object
		{
			this.DispatchTransformChangesAndClear(typeof(T), trackingType, callback);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00026B22 File Offset: 0x00024D22
		public void ClearTypeChanges<T>() where T : Object
		{
			this.ClearTypeChanges(typeof(T));
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00026B38 File Offset: 0x00024D38
		public TypeDispatchData GetTypeChangesAndClear<T>(Allocator allocator, bool sortByInstanceID = false, bool noScriptingArray = false) where T : Object
		{
			return this.GetTypeChangesAndClear(typeof(T), allocator, sortByInstanceID, noScriptingArray);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00026B5D File Offset: 0x00024D5D
		public void GetTypeChangesAndClear<T>(List<Object> changed, out NativeArray<int> changedID, out NativeArray<int> destroyedID, Allocator allocator, bool sortByInstanceID = false) where T : Object
		{
			this.GetTypeChangesAndClear(typeof(T), changed, out changedID, out destroyedID, allocator, sortByInstanceID);
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x00026B78 File Offset: 0x00024D78
		public Component[] GetTransformChangesAndClear<T>(ObjectDispatcher.TransformTrackingType trackingType, bool sortByInstanceID = false) where T : Object
		{
			return this.GetTransformChangesAndClear(typeof(T), trackingType, sortByInstanceID);
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x00026B9C File Offset: 0x00024D9C
		public TransformDispatchData GetTransformChangesAndClear<T>(ObjectDispatcher.TransformTrackingType trackingType, Allocator allocator) where T : Object
		{
			return this.GetTransformChangesAndClear(typeof(T), trackingType, allocator);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x00026BC0 File Offset: 0x00024DC0
		public void EnableTypeTracking<T>(ObjectDispatcher.TypeTrackingFlags typeTrackingMask = ObjectDispatcher.TypeTrackingFlags.Default) where T : Object
		{
			this.EnableTypeTracking(typeTrackingMask, new Type[] { typeof(T) });
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x00026BDE File Offset: 0x00024DDE
		public void DisableTypeTracking<T>() where T : Object
		{
			this.DisableTypeTracking(new Type[] { typeof(T) });
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00026BFB File Offset: 0x00024DFB
		public void EnableTransformTracking<T>(ObjectDispatcher.TransformTrackingType trackingType) where T : Object
		{
			this.EnableTransformTracking(trackingType, new Type[] { typeof(T) });
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00026C19 File Offset: 0x00024E19
		public void DisableTransformTracking<T>(ObjectDispatcher.TransformTrackingType trackingType) where T : Object
		{
			this.DisableTransformTracking(trackingType, new Type[] { typeof(T) });
		}

		// Token: 0x0600173E RID: 5950
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateDispatchSystemHandle();

		// Token: 0x0600173F RID: 5951
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyDispatchSystemHandle(IntPtr ptr);

		// Token: 0x06001740 RID: 5952
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetMaxDispatchHistoryFramesCount(IntPtr ptr);

		// Token: 0x06001741 RID: 5953
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaxDispatchHistoryFramesCount(IntPtr ptr, int count);

		// Token: 0x06001742 RID: 5954
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableTypeTracking(IntPtr ptr, Type type, ObjectDispatcher.TypeTrackingFlags typeTrackingMask);

		// Token: 0x06001743 RID: 5955
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableTypeTracking(IntPtr ptr, Type type);

		// Token: 0x06001744 RID: 5956
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableTransformTracking(IntPtr ptr, Type type, ObjectDispatcher.TransformTrackingType trackingType);

		// Token: 0x06001745 RID: 5957
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableTransformTracking(IntPtr ptr, Type type, ObjectDispatcher.TransformTrackingType trackingType);

		// Token: 0x06001746 RID: 5958
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DispatchTypeChangesAndClear(IntPtr ptr, Type type, Action<Object[], IntPtr, IntPtr, int, int, Action<TypeDispatchData>> callback, bool sortByInstanceID, bool noScriptingArray, Action<TypeDispatchData> param);

		// Token: 0x06001747 RID: 5959
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DispatchTransformDataChangesAndClear(IntPtr ptr, Type type, ObjectDispatcher.TransformTrackingType trackingType, Action<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, int, Action<TransformDispatchData>> callback, Action<TransformDispatchData> param);

		// Token: 0x06001748 RID: 5960
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DispatchTransformChangesAndClear(IntPtr ptr, Type type, ObjectDispatcher.TransformTrackingType trackingType, Action<Component[]> callback, bool sortByInstanceID);

		// Token: 0x04000853 RID: 2131
		private IntPtr m_Ptr = IntPtr.Zero;

		// Token: 0x04000854 RID: 2132
		private static Action<Object[], IntPtr, IntPtr, int, int, Action<TypeDispatchData>> s_TypeDispatch = delegate(Object[] changed, IntPtr changedID, IntPtr destroyedID, int changedCount, int destroyedCount, Action<TypeDispatchData> callback)
		{
			NativeArray<int> nativeArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(changedID.ToPointer(), changedCount, Allocator.Invalid);
			NativeArray<int> nativeArray2 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(destroyedID.ToPointer(), destroyedCount, Allocator.Invalid);
			TypeDispatchData typeDispatchData = new TypeDispatchData
			{
				changed = changed,
				changedID = nativeArray,
				destroyedID = nativeArray2
			};
			callback(typeDispatchData);
		};

		// Token: 0x04000855 RID: 2133
		private static Action<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, int, Action<TransformDispatchData>> s_TransformDispatch = delegate(IntPtr transformed, IntPtr parents, IntPtr localToWorldMatrices, IntPtr positions, IntPtr rotations, IntPtr scales, int count, Action<TransformDispatchData> callback)
		{
			NativeArray<int> nativeArray3 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(transformed.ToPointer(), count, Allocator.Invalid);
			NativeArray<int> nativeArray4 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>(parents.ToPointer(), (parents != IntPtr.Zero) ? count : 0, Allocator.Invalid);
			NativeArray<Matrix4x4> nativeArray5 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Matrix4x4>(localToWorldMatrices.ToPointer(), (localToWorldMatrices != IntPtr.Zero) ? count : 0, Allocator.Invalid);
			NativeArray<Vector3> nativeArray6 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector3>(positions.ToPointer(), (positions != IntPtr.Zero) ? count : 0, Allocator.Invalid);
			NativeArray<Quaternion> nativeArray7 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Quaternion>(rotations.ToPointer(), (rotations != IntPtr.Zero) ? count : 0, Allocator.Invalid);
			NativeArray<Vector3> nativeArray8 = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector3>(scales.ToPointer(), (scales != IntPtr.Zero) ? count : 0, Allocator.Invalid);
			TransformDispatchData transformDispatchData = new TransformDispatchData
			{
				transformedID = nativeArray3,
				parentID = nativeArray4,
				localToWorldMatrices = nativeArray5,
				positions = nativeArray6,
				rotations = nativeArray7,
				scales = nativeArray8
			};
			callback(transformDispatchData);
		};

		// Token: 0x020001FC RID: 508
		public enum TransformTrackingType
		{
			// Token: 0x04000857 RID: 2135
			GlobalTRS,
			// Token: 0x04000858 RID: 2136
			LocalTRS,
			// Token: 0x04000859 RID: 2137
			Hierarchy
		}

		// Token: 0x020001FD RID: 509
		[Flags]
		public enum TypeTrackingFlags
		{
			// Token: 0x0400085B RID: 2139
			SceneObjects = 1,
			// Token: 0x0400085C RID: 2140
			Assets = 2,
			// Token: 0x0400085D RID: 2141
			EditorOnlyObjects = 4,
			// Token: 0x0400085E RID: 2142
			Default = 3,
			// Token: 0x0400085F RID: 2143
			All = 7
		}
	}
}
