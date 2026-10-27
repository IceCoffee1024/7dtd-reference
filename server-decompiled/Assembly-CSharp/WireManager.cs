using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013AD RID: 5037
public class WireManager
{
	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x06009E57 RID: 40535 RVA: 0x003BDB05 File Offset: 0x003BBD05
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

	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x06009E58 RID: 40536 RVA: 0x003BDB1D File Offset: 0x003BBD1D
	public static bool HasInstance
	{
		get
		{
			return WireManager.instance != null;
		}
	}

	// Token: 0x170012EC RID: 4844
	// (get) Token: 0x06009E59 RID: 40537 RVA: 0x003BDB27 File Offset: 0x003BBD27
	public Transform WireManagerRoot
	{
		get
		{
			return this.wireManagerRoot;
		}
	}

	// Token: 0x06009E5A RID: 40538 RVA: 0x003BDB30 File Offset: 0x003BBD30
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

	// Token: 0x06009E5B RID: 40539 RVA: 0x003BDC10 File Offset: 0x003BBE10
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

	// Token: 0x06009E5C RID: 40540 RVA: 0x003BDCB0 File Offset: 0x003BBEB0
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

	// Token: 0x06009E5D RID: 40541 RVA: 0x003BDD10 File Offset: 0x003BBF10
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

	// Token: 0x06009E5E RID: 40542 RVA: 0x003BDD93 File Offset: 0x003BBF93
	public bool AddActiveWire(IWireNode wire)
	{
		return this.activeWires.Add(wire);
	}

	// Token: 0x06009E5F RID: 40543 RVA: 0x003BDDA1 File Offset: 0x003BBFA1
	public bool RemoveActiveWire(IWireNode wire)
	{
		return this.activeWires.Remove(wire);
	}

	// Token: 0x06009E60 RID: 40544 RVA: 0x003BDDAF File Offset: 0x003BBFAF
	public bool AddPulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Add(pulseObject);
	}

	// Token: 0x06009E61 RID: 40545 RVA: 0x003BDDBD File Offset: 0x003BBFBD
	public bool RemovePulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Remove(pulseObject);
	}

	// Token: 0x06009E62 RID: 40546 RVA: 0x003BDDCC File Offset: 0x003BBFCC
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

	// Token: 0x06009E63 RID: 40547 RVA: 0x003BDFC8 File Offset: 0x003BC1C8
	public void SetWirePulse(IWireNode node)
	{
		node.TogglePulse(this.ShowPulse);
	}

	// Token: 0x06009E64 RID: 40548 RVA: 0x003BDFD8 File Offset: 0x003BC1D8
	public void RefreshPulseObjects()
	{
		foreach (GameObject gameObject in this.activePulseObjects)
		{
			gameObject.SetActive(this.ShowPulse);
			gameObject.layer = (this.ShowPulse ? 0 : 11);
		}
	}

	// Token: 0x06009E65 RID: 40549 RVA: 0x003BE044 File Offset: 0x003BC244
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

	// Token: 0x04007800 RID: 30720
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool USE_FAST_WIRE_NODES = true;

	// Token: 0x04007801 RID: 30721
	[PublicizedFrom(EAccessModifier.Private)]
	public static WireManager instance = null;

	// Token: 0x04007802 RID: 30722
	[PublicizedFrom(EAccessModifier.Private)]
	public int wireIndex;

	// Token: 0x04007803 RID: 30723
	public bool ShowPulse;

	// Token: 0x04007804 RID: 30724
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardPulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x04007805 RID: 30725
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tripWirePulseColor = Color.magenta;

	// Token: 0x04007806 RID: 30726
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<IWireNode> activeWires;

	// Token: 0x04007807 RID: 30727
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<GameObject> activePulseObjects;

	// Token: 0x04007808 RID: 30728
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wireManagerRoot;

	// Token: 0x04007809 RID: 30729
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wirePool;

	// Token: 0x0400780A RID: 30730
	public bool WiresShowing;
}
