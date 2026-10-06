using System;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001C9F RID: 7327
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x0600D924 RID: 55588 RVA: 0x000470B6 File Offset: 0x000452B6
		public ushort Version
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x0600D925 RID: 55589 RVA: 0x004DE0F6 File Offset: 0x004DC2F6
		// (set) Token: 0x0600D926 RID: 55590 RVA: 0x004DE0FE File Offset: 0x004DC2FE
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D927 RID: 55591 RVA: 0x004DE107 File Offset: 0x004DC307
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(3);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D928 RID: 55592 RVA: 0x004DE128 File Offset: 0x004DC328
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D929 RID: 55593 RVA: 0x004DE149 File Offset: 0x004DC349
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A4E1 RID: 42209
		public const ushort VERSION = 3;
	}
}
