using System;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001C49 RID: 7241
	public sealed class ContainerData : IDisposable, IMigratable
	{
		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x0600D638 RID: 54840 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public ushort Version
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x0600D639 RID: 54841 RVA: 0x004D3A9E File Offset: 0x004D1C9E
		// (set) Token: 0x0600D63A RID: 54842 RVA: 0x004D3AA6 File Offset: 0x004D1CA6
		public Node RootNode
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Node();

		// Token: 0x0600D63B RID: 54843 RVA: 0x004D3AAF File Offset: 0x004D1CAF
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(2);
			this.RootNode.Write(writer);
		}

		// Token: 0x0600D63C RID: 54844 RVA: 0x004D3AC4 File Offset: 0x004D1CC4
		public void Read(PooledBinaryReader reader)
		{
			reader.ReadUInt16();
			this.RootNode.Read(reader);
		}

		// Token: 0x0600D63D RID: 54845 RVA: 0x004D3AD9 File Offset: 0x004D1CD9
		public void Dispose()
		{
			Node rootNode = this.RootNode;
			if (rootNode != null)
			{
				rootNode.Dispose();
			}
			this.RootNode = null;
		}

		// Token: 0x0400A2E9 RID: 41705
		public const ushort VERSION = 2;
	}
}
