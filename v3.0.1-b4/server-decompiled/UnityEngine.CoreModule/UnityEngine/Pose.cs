using System;

namespace UnityEngine
{
	// Token: 0x02000298 RID: 664
	[Serializable]
	public struct Pose : IEquatable<Pose>
	{
		// Token: 0x06001C1E RID: 7198 RVA: 0x0002EA5C File Offset: 0x0002CC5C
		public Pose(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0002EA70 File Offset: 0x0002CC70
		public override string ToString()
		{
			return UnityString.Format("({0}, {1})", new object[]
			{
				this.position.ToString(),
				this.rotation.ToString()
			});
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0002EABC File Offset: 0x0002CCBC
		public string ToString(string format)
		{
			return UnityString.Format("({0}, {1})", new object[]
			{
				this.position.ToString(format),
				this.rotation.ToString(format)
			});
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0002EAFC File Offset: 0x0002CCFC
		public Pose GetTransformedBy(Pose lhs)
		{
			return new Pose
			{
				position = lhs.position + lhs.rotation * this.position,
				rotation = lhs.rotation * this.rotation
			};
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0002EB54 File Offset: 0x0002CD54
		public Pose GetTransformedBy(Transform lhs)
		{
			return new Pose
			{
				position = lhs.TransformPoint(this.position),
				rotation = lhs.rotation * this.rotation
			};
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x0002EB9C File Offset: 0x0002CD9C
		public Vector3 forward
		{
			get
			{
				return this.rotation * Vector3.forward;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001C24 RID: 7204 RVA: 0x0002EBC0 File Offset: 0x0002CDC0
		public Vector3 right
		{
			get
			{
				return this.rotation * Vector3.right;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
		public Vector3 up
		{
			get
			{
				return this.rotation * Vector3.up;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x0002EC08 File Offset: 0x0002CE08
		public static Pose identity
		{
			get
			{
				return Pose.k_Identity;
			}
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0002EC20 File Offset: 0x0002CE20
		public override bool Equals(object obj)
		{
			bool flag = !(obj is Pose);
			return !flag && this.Equals((Pose)obj);
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0002EC54 File Offset: 0x0002CE54
		public bool Equals(Pose other)
		{
			return this.position.Equals(other.position) && this.rotation.Equals(other.rotation);
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0002EC90 File Offset: 0x0002CE90
		public override int GetHashCode()
		{
			return this.position.GetHashCode() ^ (this.rotation.GetHashCode() << 1);
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x0002ECC8 File Offset: 0x0002CEC8
		public static bool operator ==(Pose a, Pose b)
		{
			return a.position == b.position && a.rotation.Equals(b.rotation);
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0002ED04 File Offset: 0x0002CF04
		public static bool operator !=(Pose a, Pose b)
		{
			return !(a == b);
		}

		// Token: 0x04000975 RID: 2421
		public Vector3 position;

		// Token: 0x04000976 RID: 2422
		public Quaternion rotation;

		// Token: 0x04000977 RID: 2423
		private static readonly Pose k_Identity = new Pose(Vector3.zero, Quaternion.identity);
	}
}
