using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000473 RID: 1139
	public struct ShaderTagId : IEquatable<ShaderTagId>
	{
		// Token: 0x060026AD RID: 9901 RVA: 0x0004245A File Offset: 0x0004065A
		public ShaderTagId(string name)
		{
			this.m_Id = Shader.TagToID(name);
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x0004246C File Offset: 0x0004066C
		// (set) Token: 0x060026AF RID: 9903 RVA: 0x00042484 File Offset: 0x00040684
		internal int id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x00042490 File Offset: 0x00040690
		public string name
		{
			get
			{
				return Shader.IDToTag(this.id);
			}
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x000424B0 File Offset: 0x000406B0
		public override bool Equals(object obj)
		{
			return obj is ShaderTagId && this.Equals((ShaderTagId)obj);
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x000424DC File Offset: 0x000406DC
		public bool Equals(ShaderTagId other)
		{
			return this.m_Id == other.m_Id;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x000424FC File Offset: 0x000406FC
		public override int GetHashCode()
		{
			int num = 2079669542;
			return num * -1521134295 + this.m_Id.GetHashCode();
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0004252C File Offset: 0x0004072C
		public static bool operator ==(ShaderTagId tag1, ShaderTagId tag2)
		{
			return tag1.Equals(tag2);
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x00042548 File Offset: 0x00040748
		public static bool operator !=(ShaderTagId tag1, ShaderTagId tag2)
		{
			return !(tag1 == tag2);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x00042564 File Offset: 0x00040764
		public static explicit operator ShaderTagId(string name)
		{
			return new ShaderTagId(name);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0004257C File Offset: 0x0004077C
		public static explicit operator string(ShaderTagId tagId)
		{
			return tagId.name;
		}

		// Token: 0x04000E9B RID: 3739
		public static readonly ShaderTagId none;

		// Token: 0x04000E9C RID: 3740
		private int m_Id;
	}
}
