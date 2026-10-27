using System;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001CD3 RID: 7379
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x0600DA55 RID: 55893 RVA: 0x0002F260 File Offset: 0x0002D460
		public ushort Version
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x0600DA56 RID: 55894 RVA: 0x004E254E File Offset: 0x004E074E
		// (set) Token: 0x0600DA57 RID: 55895 RVA: 0x004E2556 File Offset: 0x004E0756
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600DA58 RID: 55896 RVA: 0x004E255F File Offset: 0x004E075F
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(2);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600DA59 RID: 55897 RVA: 0x004E2574 File Offset: 0x004E0774
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600DA5A RID: 55898 RVA: 0x004E2589 File Offset: 0x004E0789
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A5D8 RID: 42456
		public const ushort VERSION = 2;
	}
}
