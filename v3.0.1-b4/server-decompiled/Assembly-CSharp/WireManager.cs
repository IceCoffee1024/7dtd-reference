using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001361 RID: 4961
public class WireManager
{
	// Token: 0x17001260 RID: 4704
	// (get) Token: 0x06009C59 RID: 40025 RVA: 0x003B18D1 File Offset: 0x003AFAD1
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

	// Token: 0x17001261 RID: 4705
	// (get) Token: 0x06009C5A RID: 40026 RVA: 0x003B18E9 File Offset: 0x003AFAE9
	public static bool HasInstance
	{
		get
		{
			return WireManager.instance != null;
		}
	}

	// Token: 0x17001262 RID: 4706
	// (get) Token: 0x06009C5B RID: 40027 RVA: 0x003B18F3 File Offset: 0x003AFAF3
	public Transform WireManagerRoot
	{
		get
		{
			return this.wireManagerRoot;
		}
	}

	// Token: 0x06009C5C RID: 40028 RVA: 0x003B18FC File Offset: 0x003AFAFC
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

	// Token: 0x06009C5D RID: 40029 RVA: 0x003B19DC File Offset: 0x003AFBDC
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

	// Token: 0x06009C5E RID: 40030 RVA: 0x003B1A7C File Offset: 0x003AFC7C
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

	// Token: 0x06009C5F RID: 40031 RVA: 0x003B1ADC File Offset: 0x003AFCDC
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

	// Token: 0x06009C60 RID: 40032 RVA: 0x003B1B5F File Offset: 0x003AFD5F
	public bool AddActiveWire(IWireNode wire)
	{
		return this.activeWires.Add(wire);
	}

	// Token: 0x06009C61 RID: 40033 RVA: 0x003B1B6D File Offset: 0x003AFD6D
	public bool RemoveActiveWire(IWireNode wire)
	{
		return this.activeWires.Remove(wire);
	}

	// Token: 0x06009C62 RID: 40034 RVA: 0x003B1B7B File Offset: 0x003AFD7B
	public bool AddPulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Add(pulseObject);
	}

	// Token: 0x06009C63 RID: 40035 RVA: 0x003B1B89 File Offset: 0x003AFD89
	public bool RemovePulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Remove(pulseObject);
	}

	// Token: 0x06009C64 RID: 40036 RVA: 0x003B1B98 File Offset: 0x003AFD98
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

	// Token: 0x06009C65 RID: 40037 RVA: 0x003B1D94 File Offset: 0x003AFF94
	public void SetWirePulse(IWireNode node)
	{
		node.TogglePulse(this.ShowPulse);
	}

	// Token: 0x06009C66 RID: 40038 RVA: 0x003B1DA4 File Offset: 0x003AFFA4
	public void RefreshPulseObjects()
	{
		foreach (GameObject gameObject in this.activePulseObjects)
		{
			gameObject.SetActive(this.ShowPulse);
			gameObject.layer = (this.ShowPulse ? 0 : 11);
		}
	}

	// Token: 0x06009C67 RID: 40039 RVA: 0x003B1E10 File Offset: 0x003B0010
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

	// Token: 0x0400766C RID: 30316
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool USE_FAST_WIRE_NODES = true;

	// Token: 0x0400766D RID: 30317
	[PublicizedFrom(EAccessModifier.Private)]
	public static WireManager instance = null;

	// Token: 0x0400766E RID: 30318
	[PublicizedFrom(EAccessModifier.Private)]
	public int wireIndex;

	// Token: 0x0400766F RID: 30319
	public bool ShowPulse;

	// Token: 0x04007670 RID: 30320
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardPulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x04007671 RID: 30321
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tripWirePulseColor = Color.magenta;

	// Token: 0x04007672 RID: 30322
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<IWireNode> activeWires;

	// Token: 0x04007673 RID: 30323
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<GameObject> activePulseObjects;

	// Token: 0x04007674 RID: 30324
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wireManagerRoot;

	// Token: 0x04007675 RID: 30325
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wirePool;

	// Token: 0x04007676 RID: 30326
	public bool WiresShowing;
}
