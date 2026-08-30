using System;
using System.Runtime.InteropServices;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Scripting;

namespace Unity.Profiling
{
	// Token: 0x02000058 RID: 88
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Explicit, Size = 2)]
	public readonly struct ProfilerCategory
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00003042 File Offset: 0x00001242
		public ProfilerCategory(string categoryName)
		{
			this.m_CategoryId = ProfilerUnsafeUtility.CreateCategory(categoryName, ProfilerCategoryColor.Scripts);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003052 File Offset: 0x00001252
		public ProfilerCategory(string categoryName, ProfilerCategoryColor color)
		{
			this.m_CategoryId = ProfilerUnsafeUtility.CreateCategory(categoryName, color);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003062 File Offset: 0x00001262
		internal ProfilerCategory(ushort category)
		{
			this.m_CategoryId = category;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000306C File Offset: 0x0000126C
		public string Name
		{
			get
			{
				ProfilerCategoryDescription categoryDescription = ProfilerUnsafeUtility.GetCategoryDescription(this.m_CategoryId);
				return ProfilerUnsafeUtility.Utf8ToString(categoryDescription.NameUtf8, categoryDescription.NameUtf8Len);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000309B File Offset: 0x0000129B
		public Color32 Color
		{
			get
			{
				return ProfilerUnsafeUtility.GetCategoryDescription(this.m_CategoryId).Color;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000030B0 File Offset: 0x000012B0
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000030C8 File Offset: 0x000012C8
		public static ProfilerCategory Render
		{
			get
			{
				return new ProfilerCategory(0);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000030D0 File Offset: 0x000012D0
		public static ProfilerCategory Scripts
		{
			get
			{
				return new ProfilerCategory(1);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000030D8 File Offset: 0x000012D8
		public static ProfilerCategory Gui
		{
			get
			{
				return new ProfilerCategory(4);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000030E0 File Offset: 0x000012E0
		public static ProfilerCategory Physics
		{
			get
			{
				return new ProfilerCategory(5);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000030E8 File Offset: 0x000012E8
		public static ProfilerCategory Physics2D
		{
			get
			{
				return new ProfilerCategory(33);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000030F1 File Offset: 0x000012F1
		public static ProfilerCategory Animation
		{
			get
			{
				return new ProfilerCategory(6);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000030F9 File Offset: 0x000012F9
		public static ProfilerCategory Ai
		{
			get
			{
				return new ProfilerCategory(7);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00003101 File Offset: 0x00001301
		public static ProfilerCategory Audio
		{
			get
			{
				return new ProfilerCategory(8);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00003109 File Offset: 0x00001309
		public static ProfilerCategory Video
		{
			get
			{
				return new ProfilerCategory(11);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00003112 File Offset: 0x00001312
		public static ProfilerCategory Particles
		{
			get
			{
				return new ProfilerCategory(12);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000311B File Offset: 0x0000131B
		public static ProfilerCategory Lighting
		{
			get
			{
				return new ProfilerCategory(13);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00003124 File Offset: 0x00001324
		public static ProfilerCategory Network
		{
			get
			{
				return new ProfilerCategory(14);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000312D File Offset: 0x0000132D
		public static ProfilerCategory Loading
		{
			get
			{
				return new ProfilerCategory(15);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00003136 File Offset: 0x00001336
		public static ProfilerCategory Vr
		{
			get
			{
				return new ProfilerCategory(22);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000313F File Offset: 0x0000133F
		public static ProfilerCategory Input
		{
			get
			{
				return new ProfilerCategory(30);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00003148 File Offset: 0x00001348
		public static ProfilerCategory Memory
		{
			get
			{
				return new ProfilerCategory(23);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00003151 File Offset: 0x00001351
		public static ProfilerCategory VirtualTexturing
		{
			get
			{
				return new ProfilerCategory(31);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000315A File Offset: 0x0000135A
		public static ProfilerCategory FileIO
		{
			get
			{
				return new ProfilerCategory(25);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00003163 File Offset: 0x00001363
		public static ProfilerCategory Internal
		{
			get
			{
				return new ProfilerCategory(24);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000316C File Offset: 0x0000136C
		internal static ProfilerCategory Any
		{
			get
			{
				return new ProfilerCategory(ushort.MaxValue);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00003178 File Offset: 0x00001378
		internal static ProfilerCategory GPU
		{
			get
			{
				return new ProfilerCategory(32);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003184 File Offset: 0x00001384
		public static implicit operator ushort(ProfilerCategory category)
		{
			return category.m_CategoryId;
		}

		// Token: 0x04000114 RID: 276
		[FieldOffset(0)]
		private readonly ushort m_CategoryId;
	}
}
