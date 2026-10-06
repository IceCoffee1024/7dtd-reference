using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200139B RID: 5019
public class WireManager
{
	// Token: 0x170012DE RID: 4830
	// (get) Token: 0x06009DFF RID: 40447 RVA: 0x003BB701 File Offset: 0x003B9901
	public static WireManager Instance
	{
		get
		{
			if (WireManager.instance == null)
			{
				WireManager.instance = new WireManager();
			}
			return WireManager.instance;
		}
	}

	// Token: 0x170012DF RID: 4831
	// (get) Token: 0x06009E00 RID: 40448 RVA: 0x003BB719 File Offset: 0x003B9919
	public static bool HasInstance
	{
		get
		{
			return WireManager.instance != null;
		}
	}

	// Token: 0x170012E0 RID: 4832
	// (get) Token: 0x06009E01 RID: 40449 RVA: 0x003BB723 File Offset: 0x003B9923
	public Transform WireManagerRoot
	{
		get
		{
			return this.wireManagerRoot;
		}
	}

	// Token: 0x06009E02 RID: 40450 RVA: 0x003BB72C File Offset: 0x003B992C
	public void Init()
	{
		this.activeWires = new HashSet<IWireNode>();
		this.activePulseObjects = new HashSet<GameObject>();
		GameObject gameObject = GameObject.Find("WireManager");
		if (gameObject == null)
		{
			this.wireManagerRoot = new GameObject("WireManager").transform;
		}
		else
		{
			this.wireManagerRoot = gameObject.transform;
		}
		this.wirePool = this.wireManagerRoot.Find("Pool");
		if (this.wirePool == null)
		{
			this.wirePool = new GameObject("Pool").transform;
			this.wirePool.parent = this.wireManagerRoot;
		}
		Origin.Add(this.wireManagerRoot.transform, 0);
		if (this.wirePool.transform.childCount == 0)
		{
			for (int i = 0; i < 200; i++)
			{
				this.addNewNode();
			}
		}
	}

	// Token: 0x06009E03 RID: 40451 RVA: 0x003BB80C File Offset: 0x003B9A0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addNewNode()
	{
		GameObject gameObject;
		if (WireManager.USE_FAST_WIRE_NODES)
		{
			gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/WireNode2"));
		}
		else
		{
			gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/WireNode"));
		}
		global::UnityEngine.Object @object = gameObject;
		string text = "WireNode_{0}";
		int num = this.wireIndex;
		this.wireIndex = num + 1;
		@object.name = string.Format(text, num.ToString());
		gameObject.SetActive(false);
		gameObject.transform.parent = this.wirePool;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06009E04 RID: 40452 RVA: 0x003BB8AC File Offset: 0x003B9AAC
	public void ReturnToPool(IWireNode wireNode)
	{
		this.activeWires.Remove(wireNode);
		GameObject gameObject = wireNode.GetGameObject();
		gameObject.transform.parent = this.wirePool;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.SetActive(false);
		wireNode.Reset();
	}

	// Token: 0x06009E05 RID: 40453 RVA: 0x003BB90C File Offset: 0x003B9B0C
	public IWireNode GetWireNodeFromPool()
	{
		if (this.wirePool.childCount < 1)
		{
			this.addNewNode();
		}
		Transform child = this.wirePool.GetChild(this.wirePool.childCount - 1);
		child.gameObject.SetActive(true);
		child.parent = this.wireManagerRoot;
		IWireNode wireNode;
		if (WireManager.USE_FAST_WIRE_NODES)
		{
			wireNode = child.gameObject.GetComponent<FastWireNode>();
		}
		else
		{
			wireNode = child.gameObject.GetComponent<WireNode>();
		}
		this.activeWires.Add(wireNode);
		return wireNode;
	}

	// Token: 0x06009E06 RID: 40454 RVA: 0x003BB98F File Offset: 0x003B9B8F
	public bool AddActiveWire(IWireNode wire)
	{
		return this.activeWires.Add(wire);
	}

	// Token: 0x06009E07 RID: 40455 RVA: 0x003BB99D File Offset: 0x003B9B9D
	public bool RemoveActiveWire(IWireNode wire)
	{
		return this.activeWires.Remove(wire);
	}

	// Token: 0x06009E08 RID: 40456 RVA: 0x003BB9AB File Offset: 0x003B9BAB
	public bool AddPulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Add(pulseObject);
	}

	// Token: 0x06009E09 RID: 40457 RVA: 0x003BB9B9 File Offset: 0x003B9BB9
	public bool RemovePulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Remove(pulseObject);
	}

	// Token: 0x06009E0A RID: 40458 RVA: 0x003BB9C8 File Offset: 0x003B9BC8
	public void ToggleAllWirePulse(bool isPulseOn)
	{
		World world = GameManager.Instance.World;
		this.ShowPulse = isPulseOn;
		this.WiresShowing = isPulseOn;
		if (this.activeWires == null)
		{
			return;
		}
		if (this.ShowPulse)
		{
			Dictionary<Vector3, bool> dictionary = new Dictionary<Vector3, bool>(Vector3EqualityComparer.Instance);
			foreach (IWireNode wireNode in this.activeWires)
			{
				Vector3 startPosition = wireNode.GetStartPosition();
				bool flag;
				if (dictionary.ContainsKey(startPosition))
				{
					flag = dictionary[startPosition];
				}
				else
				{
					flag = world.CanPlaceBlockAt(new Vector3i(startPosition), world.gameManager.GetPersistentLocalPlayer(), false);
					dictionary[startPosition] = flag;
				}
				if (flag)
				{
					wireNode.TogglePulse(isPulseOn);
					wireNode.SetVisible(this.WiresShowing);
				}
				else
				{
					wireNode.SetVisible(false);
				}
			}
			dictionary.Clear();
			using (HashSet<GameObject>.Enumerator enumerator2 = this.activePulseObjects.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject gameObject = enumerator2.Current;
					Vector3i vector3i = new Vector3i(gameObject.transform.position);
					if (world.CanPlaceBlockAt(vector3i, world.gameManager.GetPersistentLocalPlayer(), false))
					{
						gameObject.SetActive(isPulseOn);
					}
					gameObject.layer = 0;
				}
				return;
			}
		}
		foreach (IWireNode wireNode2 in this.activeWires)
		{
			wireNode2.TogglePulse(false);
			wireNode2.SetVisible(false);
		}
		foreach (GameObject gameObject2 in this.activePulseObjects)
		{
			gameObject2.SetActive(isPulseOn);
			gameObject2.layer = 11;
		}
	}

	// Token: 0x06009E0B RID: 40459 RVA: 0x003BBBC4 File Offset: 0x003B9DC4
	public void SetWirePulse(IWireNode node)
	{
		node.TogglePulse(this.ShowPulse);
	}

	// Token: 0x06009E0C RID: 40460 RVA: 0x003BBBD4 File Offset: 0x003B9DD4
	public void RefreshPulseObjects()
	{
		foreach (GameObject gameObject in this.activePulseObjects)
		{
			gameObject.SetActive(this.ShowPulse);
			gameObject.layer = (this.ShowPulse ? 0 : 11);
		}
	}

	// Token: 0x06009E0D RID: 40461 RVA: 0x003BBC40 File Offset: 0x003B9E40
	public void Cleanup()
	{
		foreach (IWireNode wireNode in this.activeWires)
		{
			global::UnityEngine.Object.Destroy(wireNode.GetGameObject());
		}
		this.activeWires.Clear();
		for (int i = this.wirePool.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(this.wirePool.GetChild(i).gameObject);
		}
		global::UnityEngine.Object.Destroy(this.wirePool.gameObject);
		Origin.Remove(this.wireManagerRoot);
		global::UnityEngine.Object.Destroy(this.wireManagerRoot.gameObject);
		WireManager.instance = null;
	}

	// Token: 0x040077BF RID: 30655
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool USE_FAST_WIRE_NODES = true;

	// Token: 0x040077C0 RID: 30656
	[PublicizedFrom(EAccessModifier.Private)]
	public static WireManager instance = null;

	// Token: 0x040077C1 RID: 30657
	[PublicizedFrom(EAccessModifier.Private)]
	public int wireIndex;

	// Token: 0x040077C2 RID: 30658
	public bool ShowPulse;

	// Token: 0x040077C3 RID: 30659
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardPulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x040077C4 RID: 30660
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tripWirePulseColor = Color.magenta;

	// Token: 0x040077C5 RID: 30661
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<IWireNode> activeWires;

	// Token: 0x040077C6 RID: 30662
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<GameObject> activePulseObjects;

	// Token: 0x040077C7 RID: 30663
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wireManagerRoot;

	// Token: 0x040077C8 RID: 30664
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wirePool;

	// Token: 0x040077C9 RID: 30665
	public bool WiresShowing;
}
