using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200024E RID: 590
	[NativeHeader("Runtime/BaseClasses/TagManager.h")]
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeHeader("Runtime/BaseClasses/BitField.h")]
	[NativeClass("BitField", "struct BitField;")]
	public struct LayerMask
	{
		// Token: 0x06001926 RID: 6438 RVA: 0x00029FEC File Offset: 0x000281EC
		public static implicit operator int(LayerMask mask)
		{
			return mask.m_Mask;
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0002A004 File Offset: 0x00028204
		public static implicit operator LayerMask(int intVal)
		{
			LayerMask layerMask;
			layerMask.m_Mask = intVal;
			return layerMask;
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x0002A020 File Offset: 0x00028220
		// (set) Token: 0x06001929 RID: 6441 RVA: 0x0002A038 File Offset: 0x00028238
		public int value
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}

		// Token: 0x0600192A RID: 6442
		[StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
		[NativeMethod("LayerToString")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string LayerToName(int layer);

		// Token: 0x0600192B RID: 6443
		[StaticAccessor("GetTagManager()", StaticAccessorType.Dot)]
		[NativeMethod("StringToLayer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int NameToLayer(string layerName);

		// Token: 0x0600192C RID: 6444 RVA: 0x0002A044 File Offset: 0x00028244
		public static int GetMask(params string[] layerNames)
		{
			bool flag = layerNames == null;
			if (flag)
			{
				throw new ArgumentNullException("layerNames");
			}
			int num = 0;
			foreach (string text in layerNames)
			{
				int num2 = LayerMask.NameToLayer(text);
				bool flag2 = num2 != -1;
				if (flag2)
				{
					num |= 1 << num2;
				}
			}
			return num;
		}

		// Token: 0x040008D3 RID: 2259
		[NativeName("m_Bits")]
		private int m_Mask;
	}
}
