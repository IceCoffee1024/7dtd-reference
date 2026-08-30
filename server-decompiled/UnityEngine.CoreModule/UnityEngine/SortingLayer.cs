using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020000E2 RID: 226
	[NativeHeader("Runtime/BaseClasses/TagManager.h")]
	public struct SortingLayer
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00006CF0 File Offset: 0x00004EF0
		public int id
		{
			get
			{
				return this.m_Id;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00006D08 File Offset: 0x00004F08
		public string name
		{
			get
			{
				return SortingLayer.IDToName(this.m_Id);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00006D28 File Offset: 0x00004F28
		public int value
		{
			get
			{
				return SortingLayer.GetLayerValueFromID(this.m_Id);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00006D48 File Offset: 0x00004F48
		public static SortingLayer[] layers
		{
			get
			{
				int[] sortingLayerIDsInternal = SortingLayer.GetSortingLayerIDsInternal();
				SortingLayer[] array = new SortingLayer[sortingLayerIDsInternal.Length];
				for (int i = 0; i < sortingLayerIDsInternal.Length; i++)
				{
					array[i].m_Id = sortingLayerIDsInternal[i];
				}
				return array;
			}
		}

		// Token: 0x060003FA RID: 1018
		[FreeFunction("GetTagManager().GetSortingLayerIDs")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int[] GetSortingLayerIDsInternal();

		// Token: 0x060003FB RID: 1019
		[FreeFunction("GetTagManager().GetSortingLayerValueFromUniqueID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLayerValueFromID(int id);

		// Token: 0x060003FC RID: 1020
		[FreeFunction("GetTagManager().GetSortingLayerValueFromName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetLayerValueFromName(string name);

		// Token: 0x060003FD RID: 1021
		[FreeFunction("GetTagManager().GetSortingLayerUniqueIDFromName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int NameToID(string name);

		// Token: 0x060003FE RID: 1022
		[FreeFunction("GetTagManager().GetSortingLayerNameFromUniqueID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string IDToName(int id);

		// Token: 0x060003FF RID: 1023
		[FreeFunction("GetTagManager().IsSortingLayerUniqueIDValid")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsValid(int id);

		// Token: 0x04000276 RID: 630
		private int m_Id;

		// Token: 0x04000277 RID: 631
		public static SortingLayer.LayerCallback onLayerAdded;

		// Token: 0x04000278 RID: 632
		public static SortingLayer.LayerCallback onLayerRemoved;

		// Token: 0x04000279 RID: 633
		internal static Action onLayerChanged;

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x06000401 RID: 1025
		public delegate void LayerCallback(SortingLayer layer);
	}
}
