using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001CD6 RID: 7382
	public sealed class Node : IDisposable
	{
		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x0600DA71 RID: 55921 RVA: 0x004E2C30 File Offset: 0x004E0E30
		// (set) Token: 0x0600DA72 RID: 55922 RVA: 0x004E2C38 File Offset: 0x004E0E38
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DA73 RID: 55923 RVA: 0x004E2C41 File Offset: 0x004E0E41
		public Node()
		{
			this.Name = string.Empty;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.Parent = null;
		}

		// Token: 0x0600DA74 RID: 55924 RVA: 0x004E2C74 File Offset: 0x004E0E74
		public override string ToString()
		{
			return string.Format("{0}[{1}=\"{2}\", {3}=[{4}], #{5}={6}]", new object[]
			{
				"Node",
				"Name",
				this.Name,
				"BlobIds",
				string.Join(", ", this.BlobIds.Select<ulong, string>(new Func<ulong, string>(SaveContainer.IdToString))),
				"Children",
				this.Children.Count
			});
		}

		// Token: 0x0600DA75 RID: 55925 RVA: 0x004E2CF4 File Offset: 0x004E0EF4
		public void Write(PooledBinaryWriter writer)
		{
			writer.Write(this.Name);
			ushort num = (ushort)this.BlobIds.Count;
			writer.Write(num);
			foreach (ulong num2 in this.BlobIds)
			{
				writer.Write(num2);
			}
			ushort num3 = (ushort)this.Children.Count;
			writer.Write(num3);
			foreach (Node node in this.Children.Values)
			{
				node.Write(writer);
			}
		}

		// Token: 0x0600DA76 RID: 55926 RVA: 0x004E2DC4 File Offset: 0x004E0FC4
		public void Read(PooledBinaryReader reader)
		{
			this.Name = reader.ReadString();
			ushort num = reader.ReadUInt16();
			for (int i = 0; i < (int)num; i++)
			{
				ulong num2 = reader.ReadUInt64();
				this.BlobIds.Add(num2);
			}
			ushort num3 = reader.ReadUInt16();
			for (int j = 0; j < (int)num3; j++)
			{
				Node node = new Node();
				node.Read(reader);
				this.Children.Add(node.Name, node);
				node.Parent = this;
			}
		}

		// Token: 0x0600DA77 RID: 55927 RVA: 0x004E2E48 File Offset: 0x004E1048
		public void Dispose()
		{
			foreach (Node node in this.Children.Values)
			{
				node.Dispose();
			}
			this.Children.Clear();
			this.Parent = null;
			this.BlobIds.Clear();
			this.Name = string.Empty;
		}

		// Token: 0x0600DA78 RID: 55928 RVA: 0x004E2EC8 File Offset: 0x004E10C8
		public bool IsDirectory()
		{
			return this.BlobIds.Count == 0;
		}

		// Token: 0x0600DA79 RID: 55929 RVA: 0x004E2ED8 File Offset: 0x004E10D8
		public bool IsFile()
		{
			return this.Children.Count == 0;
		}

		// Token: 0x0600DA7A RID: 55930 RVA: 0x004E2EE8 File Offset: 0x004E10E8
		public Node GetChildNode(string name)
		{
			Node node;
			if (!this.Children.TryGetValue(name, out node))
			{
				return null;
			}
			return node;
		}

		// Token: 0x0600DA7B RID: 55931 RVA: 0x004E2F08 File Offset: 0x004E1108
		public Node GetOrCreateChildNode(string name)
		{
			Node node;
			if (!this.Children.TryGetValue(name, out node))
			{
				node = new Node
				{
					Name = name
				};
				node.Parent = this;
				this.Children.Add(name, node);
			}
			return node;
		}

		// Token: 0x0400A5E4 RID: 42468
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A5E5 RID: 42469
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A5E6 RID: 42470
		public Node Parent;
	}
}
