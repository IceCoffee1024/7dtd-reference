using System;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C43 RID: 7235
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x0600D5FE RID: 54782 RVA: 0x0007FE9C File Offset: 0x0007E09C
		public ushort Version
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x0600D5FF RID: 54783 RVA: 0x004D28A5 File Offset: 0x004D0AA5
		// (set) Token: 0x0600D600 RID: 54784 RVA: 0x004D28AD File Offset: 0x004D0AAD
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node
		{
			Attributes = NodeAttributes.Directory
		};

		// Token: 0x0600D601 RID: 54785 RVA: 0x004D28B6 File Offset: 0x004D0AB6
		public void Write(PooledBinaryWriter writer)
		{
			this.RootNode.Attributes = NodeAttributes.Directory;
			writer.Write(4);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D602 RID: 54786 RVA: 0x004D28D7 File Offset: 0x004D0AD7
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
			this.RootNode.Attributes = NodeAttributes.Directory;
		}

		// Token: 0x0600D603 RID: 54787 RVA: 0x004D28F8 File Offset: 0x004D0AF8
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A2CD RID: 41677
		public const ushort VERSION = 4;
	}
}
