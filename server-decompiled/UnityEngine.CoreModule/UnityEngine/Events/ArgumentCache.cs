using System;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	// Token: 0x020002F4 RID: 756
	[Serializable]
	internal class ArgumentCache : ISerializationCallbackReceiver
	{
		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x00033644 File Offset: 0x00031844
		// (set) Token: 0x06001F55 RID: 8021 RVA: 0x0003365C File Offset: 0x0003185C
		public Object unityObjectArgument
		{
			get
			{
				return this.m_ObjectArgument;
			}
			set
			{
				this.m_ObjectArgument = value;
				this.m_ObjectArgumentAssemblyTypeName = ((value != null) ? value.GetType().AssemblyQualifiedName : string.Empty);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x00033688 File Offset: 0x00031888
		public string unityObjectArgumentAssemblyTypeName
		{
			get
			{
				return this.m_ObjectArgumentAssemblyTypeName;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001F57 RID: 8023 RVA: 0x000336A0 File Offset: 0x000318A0
		// (set) Token: 0x06001F58 RID: 8024 RVA: 0x000336B8 File Offset: 0x000318B8
		public int intArgument
		{
			get
			{
				return this.m_IntArgument;
			}
			set
			{
				this.m_IntArgument = value;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001F59 RID: 8025 RVA: 0x000336C4 File Offset: 0x000318C4
		// (set) Token: 0x06001F5A RID: 8026 RVA: 0x000336DC File Offset: 0x000318DC
		public float floatArgument
		{
			get
			{
				return this.m_FloatArgument;
			}
			set
			{
				this.m_FloatArgument = value;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x000336E8 File Offset: 0x000318E8
		// (set) Token: 0x06001F5C RID: 8028 RVA: 0x00033700 File Offset: 0x00031900
		public string stringArgument
		{
			get
			{
				return this.m_StringArgument;
			}
			set
			{
				this.m_StringArgument = value;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001F5D RID: 8029 RVA: 0x0003370C File Offset: 0x0003190C
		// (set) Token: 0x06001F5E RID: 8030 RVA: 0x00033724 File Offset: 0x00031924
		public bool boolArgument
		{
			get
			{
				return this.m_BoolArgument;
			}
			set
			{
				this.m_BoolArgument = value;
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0003372E File Offset: 0x0003192E
		public void OnBeforeSerialize()
		{
			this.m_ObjectArgumentAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_ObjectArgumentAssemblyTypeName);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0003372E File Offset: 0x0003192E
		public void OnAfterDeserialize()
		{
			this.m_ObjectArgumentAssemblyTypeName = UnityEventTools.TidyAssemblyTypeName(this.m_ObjectArgumentAssemblyTypeName);
		}

		// Token: 0x04000A65 RID: 2661
		[FormerlySerializedAs("objectArgument")]
		[SerializeField]
		private Object m_ObjectArgument;

		// Token: 0x04000A66 RID: 2662
		[FormerlySerializedAs("objectArgumentAssemblyTypeName")]
		[SerializeField]
		private string m_ObjectArgumentAssemblyTypeName;

		// Token: 0x04000A67 RID: 2663
		[FormerlySerializedAs("intArgument")]
		[SerializeField]
		private int m_IntArgument;

		// Token: 0x04000A68 RID: 2664
		[SerializeField]
		[FormerlySerializedAs("floatArgument")]
		private float m_FloatArgument;

		// Token: 0x04000A69 RID: 2665
		[SerializeField]
		[FormerlySerializedAs("stringArgument")]
		private string m_StringArgument;

		// Token: 0x04000A6A RID: 2666
		[SerializeField]
		private bool m_BoolArgument;
	}
}
