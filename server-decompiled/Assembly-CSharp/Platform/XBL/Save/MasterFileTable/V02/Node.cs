using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V02
{
	// Token: 0x02001C71 RID: 7281
	public sealed class Node : IDisposable
	{
		// Token: 0x17001AC1 RID: 6849
		// (get) Token: 0x0600D7CF RID: 55247 RVA: 0x004D9F2A File Offset: 0x004D812A
		// (set) Token: 0x0600D7D0 RID: 55248 RVA: 0x004D9F32 File Offset: 0x004D8132
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D7D1 RID: 55249 RVA: 0x004D9F3C File Offset: 0x004D813C
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.BlobIds = new List<ulong>();
			this.Children = new SortedDictionary<string, Node>();
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D7D2 RID: 55250 RVA: 0x004D9F90 File Offset: 0x004D8190
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

		// Token: 0x0600D7D3 RID: 55251 RVA: 0x004DA03C File Offset: 0x004D823C
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

		// Token: 0x0600D7D4 RID: 55252 RVA: 0x004DA148 File Offset: 0x004D8348
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

		// Token: 0x0600D7D5 RID: 55253 RVA: 0x004DA20C File Offset: 0x004D840C
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

		// Token: 0x0600D7D6 RID: 55254 RVA: 0x004DA2B4 File Offset: 0x004D84B4
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

		// Token: 0x0600D7D7 RID: 55255 RVA: 0x004DA300 File Offset: 0x004D8500
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

		// Token: 0x0600D7D8 RID: 55256 RVA: 0x004DA34C File Offset: 0x004D854C
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

		// Token: 0x0600D7D9 RID: 55257 RVA: 0x004DA3A0 File Offset: 0x004D85A0
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

		// Token: 0x0600D7DA RID: 55258 RVA: 0x004DA410 File Offset: 0x004D8610
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

		// Token: 0x0600D7DC RID: 55260 RVA: 0x004DA4BC File Offset: 0x004D86BC
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

		// Token: 0x0400A41C RID: 42012
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A41E RID: 42014
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A41F RID: 42015
		public readonly List<ulong> BlobIds;

		// Token: 0x0400A420 RID: 42016
		public readonly SortedDictionary<string, Node> Children;

		// Token: 0x0400A421 RID: 42017
		public readonly object m_lock;

		// Token: 0x0400A422 RID: 42018
		public Node Parent;
	}
}
