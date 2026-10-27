using System;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001CD0 RID: 7376
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x0600DA38 RID: 55864 RVA: 0x00047076 File Offset: 0x00045276
		public ushort Version
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x0600DA39 RID: 55865 RVA: 0x004E1C46 File Offset: 0x004DFE46
		// (set) Token: 0x0600DA3A RID: 55866 RVA: 0x004E1C4E File Offset: 0x004DFE4E
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600DA3B RID: 55867 RVA: 0x004E1C57 File Offset: 0x004DFE57
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(3);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600DA3C RID: 55868 RVA: 0x004E1C78 File Offset: 0x004DFE78
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600DA3D RID: 55869 RVA: 0x004E1C99 File Offset: 0x004DFE99
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A5CA RID: 42442
		public const ushort VERSION = 3;
	}
}
