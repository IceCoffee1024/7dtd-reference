using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x02000409 RID: 1033
	[UsedByNativeCode]
	public struct VertexAttributeDescriptor : IEquatable<VertexAttributeDescriptor>
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x00038134 File Offset: 0x00036334
		// (set) Token: 0x060021BF RID: 8639 RVA: 0x0003813C File Offset: 0x0003633C
		public VertexAttribute attribute { readonly get; set; }

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x00038145 File Offset: 0x00036345
		// (set) Token: 0x060021C1 RID: 8641 RVA: 0x0003814D File Offset: 0x0003634D
		public VertexAttributeFormat format { readonly get; set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x00038156 File Offset: 0x00036356
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x0003815E File Offset: 0x0003635E
		public int dimension { readonly get; set; }

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00038167 File Offset: 0x00036367
		// (set) Token: 0x060021C5 RID: 8645 RVA: 0x0003816F File Offset: 0x0003636F
		public int stream { readonly get; set; }

		// Token: 0x060021C6 RID: 8646 RVA: 0x00038178 File Offset: 0x00036378
		public VertexAttributeDescriptor(VertexAttribute attribute = VertexAttribute.Position, VertexAttributeFormat format = VertexAttributeFormat.Float32, int dimension = 3, int stream = 0)
		{
			this.attribute = attribute;
			this.format = format;
			this.dimension = dimension;
			this.stream = stream;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0003819C File Offset: 0x0003639C
		public override string ToString()
		{
			return string.Format("(attr={0} fmt={1} dim={2} stream={3})", new object[] { this.attribute, this.format, this.dimension, this.stream });
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000381F8 File Offset: 0x000363F8
		public override int GetHashCode()
		{
			int num = 17;
			num = (int)(num * 23 + this.attribute);
			num = (int)(num * 23 + this.format);
			num = num * 23 + this.dimension;
			return num * 23 + this.stream;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00038240 File Offset: 0x00036440
		public override bool Equals(object other)
		{
			bool flag = !(other is VertexAttributeDescriptor);
			return !flag && this.Equals((VertexAttributeDescriptor)other);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00038274 File Offset: 0x00036474
		public bool Equals(VertexAttributeDescriptor other)
		{
			return this.attribute == other.attribute && this.format == other.format && this.dimension == other.dimension && this.stream == other.stream;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000382C8 File Offset: 0x000364C8
		public static bool operator ==(VertexAttributeDescriptor lhs, VertexAttributeDescriptor rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000382E4 File Offset: 0x000364E4
		public static bool operator !=(VertexAttributeDescriptor lhs, VertexAttributeDescriptor rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
