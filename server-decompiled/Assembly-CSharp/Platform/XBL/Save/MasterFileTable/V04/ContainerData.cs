using System;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C9C RID: 7324
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x0600D907 RID: 55559 RVA: 0x00080804 File Offset: 0x0007EA04
		public ushort Version
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x0600D908 RID: 55560 RVA: 0x004DD805 File Offset: 0x004DBA05
		// (set) Token: 0x0600D909 RID: 55561 RVA: 0x004DD80D File Offset: 0x004DBA0D
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D90A RID: 55562 RVA: 0x004DD816 File Offset: 0x004DBA16
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(4);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D90B RID: 55563 RVA: 0x004DD837 File Offset: 0x004DBA37
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D90C RID: 55564 RVA: 0x004DD858 File Offset: 0x004DBA58
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A4D3 RID: 42195
		public const ushort VERSION = 4;
	}
}
