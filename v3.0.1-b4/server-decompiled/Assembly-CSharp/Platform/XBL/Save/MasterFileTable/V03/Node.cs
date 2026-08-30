using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001C47 RID: 7239
	public sealed class Node : IDisposable
	{
		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x0600D622 RID: 54818 RVA: 0x004D321D File Offset: 0x004D141D
		// (set) Token: 0x0600D623 RID: 54819 RVA: 0x004D3225 File Offset: 0x004D1425
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x0600D624 RID: 54820 RVA: 0x004D322E File Offset: 0x004D142E
		public IReadOnlyList<ulong> BlobIds
		{
			get
			{
				return this.m_blobIds;
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x0600D625 RID: 54821 RVA: 0x004D3236 File Offset: 0x004D1436
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D626 RID: 54822 RVA: 0x004D3240 File Offset: 0x004D1440
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.Attributes = NodeAttributes.None;
			this.m_blobIds = Array.Empty<ulong>();
			this.m_blobLock = new object();
			this.m_children = new StringSpanDictionary<Node>(new SortedDictionary<string, Node>());
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D627 RID: 54823 RVA: 0x004D32A8 File Offset: 0x004D14A8
		public void SetBlobIds(ulong[] blobIds)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobIds = blobIds;
			}
		}

		// Token: 0x0600D628 RID: 54824 RVA: 0x004D32EC File Offset: 0x004D14EC
		public override string ToString()
		{
			object @lock = this.m_lock;
			string text;
			lock (@lock)
			{
				text = string.Format("{0}[{1}=\"{2}\", {3}=\"{4}\", {5}=\"{6}\", {7}=[{8}], #{9}={10}]", new object[]
				{
					"Node",
					"Name",
					this.Name,
					"LastWriteTimeUtc",
					this.LastWriteTimeUtc,
					"Attributes",
					this.Attributes,
					"BlobIds",
					string.Join(", ", this.BlobIds.Select<ulong, string>(new Func<ulong, string>(SaveContainer.IdToString))),
					"Children",
					this.Children.Count
				});
			}
			return text;
		}

		// Token: 0x0600D629 RID: 54825 RVA: 0x004D33C8 File Offset: 0x004D15C8
		public void Write(PooledBinaryWriter writer)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				writer.Write(this.Name);
				writer.Write(this.LastWriteTimeUtc.Ticks);
				writer.Write(Convert.ToUInt32(this.Attributes));
				ulong[] blobIds = this.m_blobIds;
				ushort num = (ushort)blobIds.Length;
				writer.Write(num);
				foreach (ulong num2 in blobIds)
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

		// Token: 0x0600D62A RID: 54826 RVA: 0x004D34C0 File Offset: 0x004D16C0
		public void Read(PooledBinaryReader reader)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.Name = reader.ReadString();
				this.LastWriteTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				this.Attributes = (NodeAttributes)reader.ReadUInt32();
				ushort num = reader.ReadUInt16();
				ulong[] array = new ulong[(int)num];
				for (int i = 0; i < (int)num; i++)
				{
					ulong num2 = reader.ReadUInt64();
					array[i] = num2;
				}
				this.m_blobIds = array;
				ushort num3 = reader.ReadUInt16();
				for (int j = 0; j < (int)num3; j++)
				{
					Node node = new Node();
					node.Read(reader);
					this.m_children.Add(node.Name, node);
					node.Parent = this;
				}
			}
		}

		// Token: 0x0600D62B RID: 54827 RVA: 0x004D359C File Offset: 0x004D179C
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				foreach (Node node in this.Children.Values)
				{
					node.Dispose();
				}
				this.m_children.Clear();
				this.Parent = null;
				this.m_blobIds = Array.Empty<ulong>();
				this.Name = string.Empty;
			}
		}

		// Token: 0x0600D62C RID: 54828 RVA: 0x004D363C File Offset: 0x004D183C
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D62D RID: 54829 RVA: 0x004D3654 File Offset: 0x004D1854
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D62E RID: 54830 RVA: 0x004D3660 File Offset: 0x004D1860
		public Node GetChildNode(StringSpan name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.m_children.TryGetValue(name, out node))
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

		// Token: 0x0600D62F RID: 54831 RVA: 0x004D36B4 File Offset: 0x004D18B4
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D630 RID: 54832 RVA: 0x004D36CC File Offset: 0x004D18CC
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory, out bool wasCreated)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (this.m_children.TryGetValue(name, out node))
				{
					wasCreated = false;
					node2 = node;
				}
				else
				{
					string text = name.ToString();
					node = new Node
					{
						Name = text
					};
					node.Parent = this;
					this.m_children.Add(text, node);
					this.LastWriteTimeUtc = (node.LastWriteTimeUtc = DateTime.UtcNow);
					this.Attributes |= NodeAttributes.Directory;
					if (createDirectory)
					{
						node.Attributes |= NodeAttributes.Directory;
					}
					wasCreated = true;
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D631 RID: 54833 RVA: 0x004D378C File Offset: 0x004D198C
		public Node DeleteChildNode(StringSpan name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.m_children.TryGetValue(name, out node) || !this.m_children.Remove(name))
				{
					node2 = null;
				}
				else
				{
					this.LastWriteTimeUtc = DateTime.UtcNow;
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D632 RID: 54834 RVA: 0x004D37F8 File Offset: 0x004D19F8
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
				Node.<Enumerate>g__AddNodeChildrenToStack|28_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|28_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600D633 RID: 54835 RVA: 0x004D3894 File Offset: 0x004D1A94
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D634 RID: 54836 RVA: 0x004D38A8 File Offset: 0x004D1AA8
		public void MoveChild(Node child, StringSpan newName)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				object lock2 = child.m_lock;
				lock (lock2)
				{
					Node parent = child.Parent;
					if (parent == null || parent == this)
					{
						Node.<MoveChild>g__MoveChildInternal|30_0(parent, this, child, newName);
					}
					else
					{
						object lock3 = parent.m_lock;
						lock (lock3)
						{
							Node.<MoveChild>g__MoveChildInternal|30_0(parent, this, child, newName);
						}
					}
				}
			}
		}

		// Token: 0x0600D636 RID: 54838 RVA: 0x004D3970 File Offset: 0x004D1B70
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|28_0(Stack<Node> stack, Node currentNode)
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

		// Token: 0x0600D637 RID: 54839 RVA: 0x004D39F0 File Offset: 0x004D1BF0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <MoveChild>g__MoveChildInternal|30_0(Node oldParent, Node newParent, Node child, StringSpan newName)
		{
			if (oldParent == newParent && child.Name == newName)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (oldParent != null)
			{
				oldParent.m_children.Remove(child.Name);
				oldParent.LastWriteTimeUtc = utcNow;
			}
			if (child.Name != newName)
			{
				child.Name = newName.ToString();
			}
			child.Parent = newParent;
			Node node;
			newParent.m_children.Remove(child.Name, out node);
			if (node != null && node != child)
			{
				node.Dispose();
			}
			newParent.m_children.Add(child.Name, child);
			newParent.LastWriteTimeUtc = utcNow;
		}

		// Token: 0x0400A2DD RID: 41693
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A2DF RID: 41695
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A2E0 RID: 41696
		public NodeAttributes Attributes;

		// Token: 0x0400A2E1 RID: 41697
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong[] m_blobIds;

		// Token: 0x0400A2E2 RID: 41698
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A2E3 RID: 41699
		public readonly object m_blobLock;

		// Token: 0x0400A2E4 RID: 41700
		public readonly object m_lock;

		// Token: 0x0400A2E5 RID: 41701
		public Node Parent;
	}
}
