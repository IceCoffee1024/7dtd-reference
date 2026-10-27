using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001CD4 RID: 7380
	public sealed class Node : IDisposable
	{
		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x0600DA5C RID: 55900 RVA: 0x004E25B6 File Offset: 0x004E07B6
		// (set) Token: 0x0600DA5D RID: 55901 RVA: 0x004E25BE File Offset: 0x004E07BE
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DA5E RID: 55902 RVA: 0x004E25C8 File Offset: 0x004E07C8
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600DA5F RID: 55903 RVA: 0x004E261C File Offset: 0x004E081C
		public override string ToString()
		{
			object @lock = this.m_lock;
			string text;
			lock (@lock)
			{
				text = string.Format("{0}[{1}=\"{2}\", {3}=[{4}], #{5}={6}]", new object[]
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
			return text;
		}

		// Token: 0x0600DA60 RID: 55904 RVA: 0x004E26C8 File Offset: 0x004E08C8
		public void Write(PooledBinaryWriter writer)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				writer.Write(this.Name);
				writer.Write(this.LastWriteTimeUtc.Ticks);
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
		}

		// Token: 0x0600DA61 RID: 55905 RVA: 0x004E27D4 File Offset: 0x004E09D4
		public void Read(PooledBinaryReader reader)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.Name = reader.ReadString();
				this.LastWriteTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
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
		}

		// Token: 0x0600DA62 RID: 55906 RVA: 0x004E2898 File Offset: 0x004E0A98
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
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
		}

		// Token: 0x0600DA63 RID: 55907 RVA: 0x004E2940 File Offset: 0x004E0B40
		public bool IsDirectory()
		{
			object @lock = this.m_lock;
			bool flag2;
			lock (@lock)
			{
				flag2 = this.BlobIds.Count == 0;
			}
			return flag2;
		}

		// Token: 0x0600DA64 RID: 55908 RVA: 0x004E298C File Offset: 0x004E0B8C
		public bool IsFile()
		{
			object @lock = this.m_lock;
			bool flag2;
			lock (@lock)
			{
				flag2 = this.Children.Count == 0;
			}
			return flag2;
		}

		// Token: 0x0600DA65 RID: 55909 RVA: 0x004E29D8 File Offset: 0x004E0BD8
		public Node GetChildNode(string name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.Children.TryGetValue(name, out node))
				{
					node2 = null;
				}
				else
				{
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600DA66 RID: 55910 RVA: 0x004E2A2C File Offset: 0x004E0C2C
		public Node GetOrCreateChildNode(string name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
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
				node2 = node;
			}
			return node2;
		}

		// Token: 0x0600DA67 RID: 55911 RVA: 0x004E2A9C File Offset: 0x004E0C9C
		public IEnumerable<Node> Enumerate(bool includeSelf, bool recursive)
		{
			List<Node> list = new List<Node>();
			if (!recursive)
			{
				if (includeSelf)
				{
					list.Add(this);
				}
				object @lock = this.m_lock;
				lock (@lock)
				{
					list.AddRange(this.Children.Values);
					return list;
				}
			}
			Stack<Node> stack = new Stack<Node>();
			if (includeSelf)
			{
				stack.Push(this);
			}
			else
			{
				Node.<Enumerate>g__AddNodeChildrenToStack|19_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|19_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600DA69 RID: 55913 RVA: 0x004E2B48 File Offset: 0x004E0D48
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|19_0(Stack<Node> stack, Node currentNode)
		{
			object @lock = currentNode.m_lock;
			lock (@lock)
			{
				foreach (Node node in currentNode.Children.Values.Reverse<Node>())
				{
					stack.Push(node);
				}
			}
		}

		// Token: 0x0400A5DA RID: 42458
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A5DC RID: 42460
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A5DD RID: 42461
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A5DE RID: 42462
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A5DF RID: 42463
		public readonly object m_lock;

		// Token: 0x0400A5E0 RID: 42464
		public Node Parent;
	}
}
