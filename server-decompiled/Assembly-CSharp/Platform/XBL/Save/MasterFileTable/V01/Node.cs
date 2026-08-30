using System;
using System.Collections.Generic;
using System.Linq;

namespace Platform.XBL.Save.MasterFileTable.V01
{
	// Token: 0x02001C73 RID: 7283
	public sealed class Node : IDisposable
	{
		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x0600D7E4 RID: 55268 RVA: 0x004DA5A4 File Offset: 0x004D87A4
		// (set) Token: 0x0600D7E5 RID: 55269 RVA: 0x004DA5AC File Offset: 0x004D87AC
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D7E6 RID: 55270 RVA: 0x004DA5B5 File Offset: 0x004D87B5
		public Node()
		{
			this.Name = string.Empty;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.Parent = null;
		}

		// Token: 0x0600D7E7 RID: 55271 RVA: 0x004DA5E8 File Offset: 0x004D87E8
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

		// Token: 0x0600D7E8 RID: 55272 RVA: 0x004DA668 File Offset: 0x004D8868
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

		// Token: 0x0600D7E9 RID: 55273 RVA: 0x004DA738 File Offset: 0x004D8938
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

		// Token: 0x0600D7EA RID: 55274 RVA: 0x004DA7BC File Offset: 0x004D89BC
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

		// Token: 0x0600D7EB RID: 55275 RVA: 0x004DA83C File Offset: 0x004D8A3C
		public bool IsDirectory()
		{
			return this.BlobIds.Count == 0;
		}

		// Token: 0x0600D7EC RID: 55276 RVA: 0x004DA84C File Offset: 0x004D8A4C
		public bool IsFile()
		{
			return this.Children.Count == 0;
		}

		// Token: 0x0600D7ED RID: 55277 RVA: 0x004DA85C File Offset: 0x004D8A5C
		public Node GetChildNode(string name)
		{
			Node node;
			if (!this.Children.TryGetValue(name, out node))
			{
				return null;
			}
			return node;
		}

		// Token: 0x0600D7EE RID: 55278 RVA: 0x004DA87C File Offset: 0x004D8A7C
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

		// Token: 0x0400A426 RID: 42022
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A427 RID: 42023
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A428 RID: 42024
		public Node Parent;
	}
}
