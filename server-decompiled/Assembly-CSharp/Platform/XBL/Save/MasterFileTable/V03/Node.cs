using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V03
{
	// Token: 0x02001CD1 RID: 7377
	public sealed class Node : IDisposable
	{
		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x0600DA3F RID: 55871 RVA: 0x004E1CCD File Offset: 0x004DFECD
		// (set) Token: 0x0600DA40 RID: 55872 RVA: 0x004E1CD5 File Offset: 0x004DFED5
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x0600DA41 RID: 55873 RVA: 0x004E1CDE File Offset: 0x004DFEDE
		public IReadOnlyList<ulong> BlobIds
		{
			get
			{
				return this.m_blobIds;
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x0600DA42 RID: 55874 RVA: 0x004E1CE6 File Offset: 0x004DFEE6
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600DA43 RID: 55875 RVA: 0x004E1CF0 File Offset: 0x004DFEF0
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

		// Token: 0x0600DA44 RID: 55876 RVA: 0x004E1D58 File Offset: 0x004DFF58
		public void SetBlobIds(ulong[] blobIds)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobIds = blobIds;
			}
		}

		// Token: 0x0600DA45 RID: 55877 RVA: 0x004E1D9C File Offset: 0x004DFF9C
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

		// Token: 0x0600DA46 RID: 55878 RVA: 0x004E1E78 File Offset: 0x004E0078
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

		// Token: 0x0600DA47 RID: 55879 RVA: 0x004E1F70 File Offset: 0x004E0170
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

		// Token: 0x0600DA48 RID: 55880 RVA: 0x004E204C File Offset: 0x004E024C
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

		// Token: 0x0600DA49 RID: 55881 RVA: 0x004E20EC File Offset: 0x004E02EC
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600DA4A RID: 55882 RVA: 0x004E2104 File Offset: 0x004E0304
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600DA4B RID: 55883 RVA: 0x004E2110 File Offset: 0x004E0310
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

		// Token: 0x0600DA4C RID: 55884 RVA: 0x004E2164 File Offset: 0x004E0364
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600DA4D RID: 55885 RVA: 0x004E217C File Offset: 0x004E037C
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

		// Token: 0x0600DA4E RID: 55886 RVA: 0x004E223C File Offset: 0x004E043C
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

		// Token: 0x0600DA4F RID: 55887 RVA: 0x004E22A8 File Offset: 0x004E04A8
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

		// Token: 0x0600DA50 RID: 55888 RVA: 0x004E2344 File Offset: 0x004E0544
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600DA51 RID: 55889 RVA: 0x004E2358 File Offset: 0x004E0558
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

		// Token: 0x0600DA53 RID: 55891 RVA: 0x004E2420 File Offset: 0x004E0620
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

		// Token: 0x0600DA54 RID: 55892 RVA: 0x004E24A0 File Offset: 0x004E06A0
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

		// Token: 0x0400A5CC RID: 42444
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A5CE RID: 42446
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A5CF RID: 42447
		public NodeAttributes Attributes;

		// Token: 0x0400A5D0 RID: 42448
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong[] m_blobIds;

		// Token: 0x0400A5D1 RID: 42449
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A5D2 RID: 42450
		public readonly object m_blobLock;

		// Token: 0x0400A5D3 RID: 42451
		public readonly object m_lock;

		// Token: 0x0400A5D4 RID: 42452
		public Node Parent;
	}
}
