using System;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001CA2 RID: 7330
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x0600D941 RID: 55617 RVA: 0x0002F260 File Offset: 0x0002D460
		public ushort Version
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x0600D942 RID: 55618 RVA: 0x004DE9FE File Offset: 0x004DCBFE
		// (set) Token: 0x0600D943 RID: 55619 RVA: 0x004DEA06 File Offset: 0x004DCC06
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600D944 RID: 55620 RVA: 0x004DEA0F File Offset: 0x004DCC0F
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(2);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D945 RID: 55621 RVA: 0x004DEA24 File Offset: 0x004DCC24
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600D946 RID: 55622 RVA: 0x004DEA39 File Offset: 0x004DCC39
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A4EF RID: 42223
		public const ushort VERSION = 2;
	}
}
