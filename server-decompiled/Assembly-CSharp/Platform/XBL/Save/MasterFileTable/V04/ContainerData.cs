using System;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001CCD RID: 7373
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x0600DA1B RID: 55835 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public ushort Version
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600DA1C RID: 55836 RVA: 0x004E1355 File Offset: 0x004DF555
		// (set) Token: 0x0600DA1D RID: 55837 RVA: 0x004E135D File Offset: 0x004DF55D
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600DA1E RID: 55838 RVA: 0x004E1366 File Offset: 0x004DF566
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(4);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600DA1F RID: 55839 RVA: 0x004E1387 File Offset: 0x004DF587
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600DA20 RID: 55840 RVA: 0x004E13A8 File Offset: 0x004DF5A8
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A5BC RID: 42428
		public const ushort VERSION = 4;
	}
}
