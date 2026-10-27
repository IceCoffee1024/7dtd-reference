using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011BE RID: 4542
public class DebugLines : MonoBehaviour
{
	// Token: 0x060090D2 RID: 37074 RVA: 0x003681B6 File Offset: 0x003663B6
	public static DebugLines Create(string _name, Transform _parentT, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		return debugLines;
	}

	// Token: 0x060090D3 RID: 37075 RVA: 0x003681EC File Offset: 0x003663EC
	public static DebugLines Create(string _name, Transform _parentT, Vector3 _pos1, Vector3 _pos2, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, _pos1 - Origin.position);
		lineRenderer.SetPosition(1, _pos2 - Origin.position);
		return debugLines;
	}

	// Token: 0x060090D4 RID: 37076 RVA: 0x00368258 File Offset: 0x00366458
	public static DebugLines CreateAttached(string _name, Transform _parentT, Vector3 _pos1, Vector3 _pos2, Color _color1, Color _color2, float _width1, float _width2, float _duration)
	{
		DebugLines debugLines = DebugLines.Create(_name, _parentT);
		debugLines.duration = _duration;
		LineRenderer lineRenderer = debugLines.line;
		lineRenderer.useWorldSpace = false;
		lineRenderer.startColor = _color1;
		lineRenderer.startWidth = _width1;
		lineRenderer.endColor = _color2;
		lineRenderer.endWidth = _width2;
		lineRenderer.positionCount = 2;
		Vector3 vector = _parentT.InverseTransformPoint(_pos1 - Origin.position);
		lineRenderer.SetPosition(0, vector);
		Vector3 vector2 = _parentT.InverseTransformPoint(_pos2 - Origin.position);
		lineRenderer.SetPosition(1, vector2);
		return debugLines;
	}

	// Token: 0x060090D5 RID: 37077 RVA: 0x003682DC File Offset: 0x003664DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static DebugLines Create(string _name, Transform _parentT)
	{
		DebugLines debugLines = null;
		string text = "DebugLines";
		if (_name != null)
		{
			text += _name;
			DebugLines.lines.TryGetValue(_name, out debugLines);
		}
		if (!debugLines)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load("Prefabs/Debug/DebugLines"), _parentT);
			gameObject.name = text;
			debugLines = gameObject.transform.GetComponent<DebugLines>();
			if (_name != null)
			{
				debugLines.keyName = _name;
				DebugLines.lines[_name] = debugLines;
			}
		}
		else
		{
			debugLines.transform.SetParent(_parentT, false);
		}
		debugLines.line = debugLines.GetComponent<LineRenderer>();
		debugLines.line.positionCount = 0;
		return debugLines;
	}

	// Token: 0x060090D6 RID: 37078 RVA: 0x00368378 File Offset: 0x00366578
	public void AddPoint(Vector3 _pos)
	{
		int positionCount = this.line.positionCount;
		this.line.positionCount = positionCount + 1;
		Vector3 vector = _pos - Origin.position;
		if (!this.line.useWorldSpace)
		{
			vector = base.transform.InverseTransformPoint(vector);
		}
		this.line.SetPosition(positionCount, vector);
	}

	// Token: 0x060090D7 RID: 37079 RVA: 0x003683D4 File Offset: 0x003665D4
	public void AddCube(Vector3 _cornerPos1, Vector3 _cornerPos2)
	{
		Vector3 vector = _cornerPos1;
		Vector3 vector2 = _cornerPos2 - _cornerPos1;
		this.AddPoint(vector);
		vector.x += vector2.x;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.z += vector2.z;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.x -= vector2.x;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.y -= vector2.y;
		this.AddPoint(vector);
		vector.z -= vector2.z;
		this.AddPoint(vector);
		vector.y += vector2.y;
		this.AddPoint(vector);
		vector.x += vector2.x;
		this.AddPoint(vector);
		vector.z += vector2.z;
		this.AddPoint(vector);
		vector.x -= vector2.x;
		this.AddPoint(vector);
		vector.z -= vector2.z;
		this.AddPoint(vector);
	}

	// Token: 0x060090D8 RID: 37080 RVA: 0x0036855A File Offset: 0x0036675A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		this.duration -= Time.deltaTime;
		if (this.duration <= 0f)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060090D9 RID: 37081 RVA: 0x00368586 File Offset: 0x00366786
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		if (this.keyName != null)
		{
			DebugLines.lines.Remove(this.keyName);
		}
	}

	// Token: 0x04006B0E RID: 27406
	public static Vector3 InsideOffsetV = new Vector3(0.05f, 0.05f, 0.05f);

	// Token: 0x04006B0F RID: 27407
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string cName = "DebugLines";

	// Token: 0x04006B10 RID: 27408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Dictionary<string, DebugLines> lines = new Dictionary<string, DebugLines>();

	// Token: 0x04006B11 RID: 27409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string keyName;

	// Token: 0x04006B12 RID: 27410
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float duration;

	// Token: 0x04006B13 RID: 27411
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LineRenderer line;
}
