using System;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C6A RID: 7274
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600D78E RID: 55182 RVA: 0x00080550 File Offset: 0x0007E750
		public ushort Version
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x0600D78F RID: 55183 RVA: 0x004D8CC9 File Offset: 0x004D6EC9
		// (set) Token: 0x0600D790 RID: 55184 RVA: 0x004D8CD1 File Offset: 0x004D6ED1
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D791 RID: 55185 RVA: 0x004D8CDA File Offset: 0x004D6EDA
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(4);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D792 RID: 55186 RVA: 0x004D8CFB File Offset: 0x004D6EFB
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D793 RID: 55187 RVA: 0x004D8D1C File Offset: 0x004D6F1C
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A3FE RID: 41982
		public const ushort VERSION = 4;
	}
}
