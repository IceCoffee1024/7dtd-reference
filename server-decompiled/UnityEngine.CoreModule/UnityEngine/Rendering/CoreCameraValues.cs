using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x0200044C RID: 1100
	[UsedByNativeCode]
	internal struct CoreCameraValues : IEquatable<CoreCameraValues>
	{
		// Token: 0x060024DB RID: 9435 RVA: 0x0003DF10 File Offset: 0x0003C110
		public bool Equals(CoreCameraValues other)
		{
			return this.filterMode == other.filterMode && this.cullingMask == other.cullingMask && this.instanceID == other.instanceID;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x0003DF50 File Offset: 0x0003C150
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is CoreCameraValues && this.Equals((CoreCameraValues)obj);
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x0003DF88 File Offset: 0x0003C188
		public override int GetHashCode()
		{
			int num = this.filterMode;
			num = (num * 397) ^ (int)this.cullingMask;
			return (num * 397) ^ this.instanceID;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x0003DFC4 File Offset: 0x0003C1C4
		public static bool operator ==(CoreCameraValues left, CoreCameraValues right)
		{
			return left.Equals(right);
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x0003DFE0 File Offset: 0x0003C1E0
		public static bool operator !=(CoreCameraValues left, CoreCameraValues right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000DB8 RID: 3512
		private int filterMode;

		// Token: 0x04000DB9 RID: 3513
		private uint cullingMask;

		// Token: 0x04000DBA RID: 3514
		private int instanceID;
	}
}
