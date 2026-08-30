using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D
{
	// Token: 0x020002B3 RID: 691
	[NativeType(CodegenOptions.Custom, "ScriptingSpriteBone")]
	[RequiredByNativeCode]
	[MovedFrom("UnityEngine.Experimental.U2D")]
	[NativeHeader("Runtime/2D/Common/SpriteDataAccess.h")]
	[NativeHeader("Runtime/2D/Common/SpriteDataMarshalling.h")]
	[Serializable]
	public struct SpriteBone
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x000307EC File Offset: 0x0002E9EC
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x00030804 File Offset: 0x0002EA04
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x00030810 File Offset: 0x0002EA10
		// (set) Token: 0x06001D6C RID: 7532 RVA: 0x00030828 File Offset: 0x0002EA28
		public string guid
		{
			get
			{
				return this.m_Guid;
			}
			set
			{
				this.m_Guid = value;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x00030834 File Offset: 0x0002EA34
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x0003084C File Offset: 0x0002EA4C
		public Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x00030858 File Offset: 0x0002EA58
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x00030870 File Offset: 0x0002EA70
		public Quaternion rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				this.m_Rotation = value;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x0003087C File Offset: 0x0002EA7C
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x00030894 File Offset: 0x0002EA94
		public float length
		{
			get
			{
				return this.m_Length;
			}
			set
			{
				this.m_Length = value;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x000308A0 File Offset: 0x0002EAA0
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x000308B8 File Offset: 0x0002EAB8
		public int parentId
		{
			get
			{
				return this.m_ParentId;
			}
			set
			{
				this.m_ParentId = value;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x000308C4 File Offset: 0x0002EAC4
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x000308DC File Offset: 0x0002EADC
		public Color32 color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
			}
		}

		// Token: 0x040009D2 RID: 2514
		[SerializeField]
		[NativeName("name")]
		private string m_Name;

		// Token: 0x040009D3 RID: 2515
		[NativeName("guid")]
		[SerializeField]
		private string m_Guid;

		// Token: 0x040009D4 RID: 2516
		[SerializeField]
		[NativeName("position")]
		private Vector3 m_Position;

		// Token: 0x040009D5 RID: 2517
		[SerializeField]
		[NativeName("rotation")]
		private Quaternion m_Rotation;

		// Token: 0x040009D6 RID: 2518
		[NativeName("length")]
		[SerializeField]
		private float m_Length;

		// Token: 0x040009D7 RID: 2519
		[NativeName("parentId")]
		[SerializeField]
		private int m_ParentId;

		// Token: 0x040009D8 RID: 2520
		[NativeName("color")]
		[SerializeField]
		private Color32 m_Color;
	}
}
