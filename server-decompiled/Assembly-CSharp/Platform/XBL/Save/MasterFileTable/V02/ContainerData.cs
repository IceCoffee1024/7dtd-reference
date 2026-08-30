using System;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001C70 RID: 7280
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001ABF RID: 6847
		// (get) Token: 0x0600D7C8 RID: 55240 RVA: 0x0002F168 File Offset: 0x0002D368
		public ushort Version
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001AC0 RID: 6848
		// (get) Token: 0x0600D7C9 RID: 55241 RVA: 0x004D9EC2 File Offset: 0x004D80C2
		// (set) Token: 0x0600D7CA RID: 55242 RVA: 0x004D9ECA File Offset: 0x004D80CA
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600D7CB RID: 55243 RVA: 0x004D9ED3 File Offset: 0x004D80D3
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(2);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D7CC RID: 55244 RVA: 0x004D9EE8 File Offset: 0x004D80E8
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600D7CD RID: 55245 RVA: 0x004D9EFD File Offset: 0x004D80FD
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A41A RID: 42010
		public const ushort VERSION = 2;
	}
}
