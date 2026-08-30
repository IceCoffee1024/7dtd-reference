using System;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001C6D RID: 7277
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x0600D7AB RID: 55211 RVA: 0x00046EDA File Offset: 0x000450DA
		public ushort Version
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x0600D7AC RID: 55212 RVA: 0x004D95BA File Offset: 0x004D77BA
		// (set) Token: 0x0600D7AD RID: 55213 RVA: 0x004D95C2 File Offset: 0x004D77C2
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D7AE RID: 55214 RVA: 0x004D95CB File Offset: 0x004D77CB
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(3);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D7AF RID: 55215 RVA: 0x004D95EC File Offset: 0x004D77EC
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D7B0 RID: 55216 RVA: 0x004D960D File Offset: 0x004D780D
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A40C RID: 41996
		public const ushort VERSION = 3;
	}
}
