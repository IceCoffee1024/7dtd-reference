using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200023A RID: 570
	[RequiredByNativeCode(GenerateProxy = true)]
	[NativeHeader("Runtime/Mono/AssemblyFullName.h")]
	internal struct AssemblyVersion
	{
		// Token: 0x06001867 RID: 6247 RVA: 0x00028658 File Offset: 0x00026858
		public AssemblyVersion(ushort major, ushort minor, ushort build, ushort revision)
		{
			this.major = major;
			this.minor = minor;
			this.build = build;
			this.revision = revision;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x00028678 File Offset: 0x00026878
		public static bool operator ==(AssemblyVersion lhs, AssemblyVersion rhs)
		{
			return lhs.major == rhs.major && lhs.minor == rhs.minor && lhs.build == rhs.build && lhs.revision == rhs.revision;
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x000286C8 File Offset: 0x000268C8
		public static bool operator !=(AssemblyVersion lhs, AssemblyVersion rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x000286E4 File Offset: 0x000268E4
		public static bool operator <(AssemblyVersion lhs, AssemblyVersion rhs)
		{
			bool flag = lhs.major != rhs.major;
			bool flag2;
			if (flag)
			{
				flag2 = lhs.major < rhs.major;
			}
			else
			{
				bool flag3 = lhs.minor != rhs.minor;
				if (flag3)
				{
					flag2 = lhs.minor < rhs.minor;
				}
				else
				{
					bool flag4 = lhs.build != rhs.build;
					if (flag4)
					{
						flag2 = lhs.build < rhs.build;
					}
					else
					{
						bool flag5 = lhs.revision != rhs.revision;
						flag2 = flag5 && lhs.revision < rhs.revision;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x00028794 File Offset: 0x00026994
		public static bool operator >(AssemblyVersion lhs, AssemblyVersion rhs)
		{
			bool flag = lhs.major != rhs.major;
			bool flag2;
			if (flag)
			{
				flag2 = lhs.major > rhs.major;
			}
			else
			{
				bool flag3 = lhs.minor != rhs.minor;
				if (flag3)
				{
					flag2 = lhs.minor > rhs.minor;
				}
				else
				{
					bool flag4 = lhs.build != rhs.build;
					if (flag4)
					{
						flag2 = lhs.build > rhs.build;
					}
					else
					{
						bool flag5 = lhs.revision != rhs.revision;
						flag2 = flag5 && lhs.revision > rhs.revision;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00028844 File Offset: 0x00026A44
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}.{3}", new object[] { this.major, this.minor, this.build, this.revision });
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000288A0 File Offset: 0x00026AA0
		public override bool Equals(object other)
		{
			if (other is AssemblyVersion)
			{
				AssemblyVersion assemblyVersion = (AssemblyVersion)other;
				if (this.major == assemblyVersion.major && this.minor == assemblyVersion.minor && this.build == assemblyVersion.build)
				{
					return this.revision == assemblyVersion.revision;
				}
			}
			return false;
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000288FC File Offset: 0x00026AFC
		public override int GetHashCode()
		{
			return HashCode.Combine<ushort, ushort, ushort, ushort>(this.major, this.minor, this.build, this.revision);
		}

		// Token: 0x040008A7 RID: 2215
		public ushort major;

		// Token: 0x040008A8 RID: 2216
		public ushort minor;

		// Token: 0x040008A9 RID: 2217
		public ushort build;

		// Token: 0x040008AA RID: 2218
		public ushort revision;
	}
}
