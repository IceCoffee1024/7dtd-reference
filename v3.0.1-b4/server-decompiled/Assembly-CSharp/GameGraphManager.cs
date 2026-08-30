using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001322 RID: 4898
public class GameGraphManager
{
	// Token: 0x06009A8B RID: 39563 RVA: 0x003A5B7A File Offset: 0x003A3D7A
	public static GameGraphManager Create(EntityPlayerLocal player)
	{
		GameGraphManager gameGraphManager = new GameGraphManager();
		gameGraphManager.player = player;
		gameGraphManager.Init();
		return gameGraphManager;
	}

	// Token: 0x06009A8C RID: 39564 RVA: 0x003A5B8E File Offset: 0x003A3D8E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		if (!GameGraphManager.whiteTex)
		{
			GameGraphManager.whiteTex = new Texture2D(1, 1);
			GameGraphManager.whiteTex.FillTexture(Color.white, true, false);
		}
	}

	// Token: 0x06009A8D RID: 39565 RVA: 0x003A5BB9 File Offset: 0x003A3DB9
	public void Destroy()
	{
		if (GameGraphManager.whiteTex)
		{
			global::UnityEngine.Object.Destroy(GameGraphManager.whiteTex);
		}
	}

	// Token: 0x06009A8E RID: 39566 RVA: 0x003A5BD4 File Offset: 0x003A3DD4
	public void Add(string name, GameGraphManager.Graph.Callback callback, int sampleCount, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (sampleCount > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, sampleCount, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.callback = callback;
		}
	}

	// Token: 0x06009A8F RID: 39567 RVA: 0x003A5C20 File Offset: 0x003A3E20
	public void AddCVar(string name, int count, string cvarName, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.cvarName = cvarName;
		}
	}

	// Token: 0x06009A90 RID: 39568 RVA: 0x003A5C6C File Offset: 0x003A3E6C
	public void AddPassiveEffect(string name, int count, PassiveEffects passiveEffect, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.passiveEffect = passiveEffect;
		}
	}

	// Token: 0x06009A91 RID: 39569 RVA: 0x003A5CB8 File Offset: 0x003A3EB8
	public void AddStat(string name, int count, string statName, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.statName = statName.ToLower();
		}
	}

	// Token: 0x06009A92 RID: 39570 RVA: 0x003A5D06 File Offset: 0x003A3F06
	public void RemoveAll()
	{
		this.graphs.Clear();
	}

	// Token: 0x06009A93 RID: 39571 RVA: 0x003A5D14 File Offset: 0x003A3F14
	public GameGraphManager.Graph FindGraph(string name)
	{
		for (int i = 0; i < this.graphs.Count; i++)
		{
			GameGraphManager.Graph graph = this.graphs[i];
			if (graph.name == name)
			{
				return graph;
			}
		}
		return null;
	}

	// Token: 0x06009A94 RID: 39572 RVA: 0x003A5D58 File Offset: 0x003A3F58
	public void Draw()
	{
		bool flag = Event.current.type == EventType.Repaint;
		float num = 1f;
		for (int i = 0; i < this.graphs.Count; i++)
		{
			GameGraphManager.Graph graph = this.graphs[i];
			if (flag)
			{
				graph.UpdateValues();
			}
			graph.pos.x = 2f;
			graph.pos.y = num;
			graph.Draw();
			num += (float)(this.graphHeight + 2);
		}
	}

	// Token: 0x06009A95 RID: 39573 RVA: 0x003A5DD3 File Offset: 0x003A3FD3
	public void SetHeight(int _height)
	{
		this.graphHeight = _height;
		this.graphHeight = Mathf.Clamp(this.graphHeight, 1, 2100);
	}

	// Token: 0x04007468 RID: 29800
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D whiteTex;

	// Token: 0x04007469 RID: 29801
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400746A RID: 29802
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameGraphManager.Graph> graphs = new List<GameGraphManager.Graph>();

	// Token: 0x0400746B RID: 29803
	[PublicizedFrom(EAccessModifier.Private)]
	public int graphHeight = 100;

	// Token: 0x02001323 RID: 4899
	public class Graph
	{
		// Token: 0x06009A97 RID: 39575 RVA: 0x003A5E10 File Offset: 0x003A4010
		public Graph(GameGraphManager _manager, string _name, int _count, float _maxValue, float _markerValue)
		{
			this.manager = _manager;
			this.name = _name;
			this.count = _count;
			this.count = Mathf.Clamp(this.count, 1, 4096);
			this.maxValue = _maxValue;
			this.markerValue = _markerValue;
			this.values = new float[this.count];
		}

		// Token: 0x06009A98 RID: 39576 RVA: 0x003A5E70 File Offset: 0x003A4070
		public void AddValue(float value)
		{
			this.index = (this.index + 1) % this.count;
			this.values[this.index] = value;
		}

		// Token: 0x06009A99 RID: 39577 RVA: 0x003A5E98 File Offset: 0x003A4098
		public void Draw()
		{
			Texture whiteTex = GameGraphManager.whiteTex;
			float num = (float)this.count * 2f + 2f;
			int graphHeight = this.manager.graphHeight;
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(this.pos.x, this.pos.y, num, (float)(graphHeight + 2)), whiteTex, ScaleMode.StretchToFill, false, 0f, new Color(0f, 0f, 0f, 0.9f), 0f, 0f);
			int num2 = this.index + 1;
			for (int i = 0; i < this.count; i++)
			{
				float num3 = this.values[num2 % this.count];
				num3 /= this.maxValue;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				float num4 = (float)graphHeight * num3;
				num4 = (float)((int)(num4 + 0.5f));
				Color color = new Color(1f, num3, num3 * 0.6f + 0.4f);
				GUI.DrawTexture(new Rect(this.pos.x + 1f + (float)i * 2f, this.pos.y + 1f + (float)graphHeight - num4, 2f, num4), whiteTex, ScaleMode.StretchToFill, false, 0f, color, 0f, 0f);
				num2++;
			}
			if (this.markerValue > 0f)
			{
				GUI.DrawTexture(new Rect(this.pos.x, this.pos.y + (float)graphHeight - this.markerValue / this.maxValue * (float)graphHeight, num, 1f), whiteTex, ScaleMode.StretchToFill, true, 0f, new Color(1f, 1f, 0f, 0.6f), 0f, 0f);
			}
			GUI.color = new Color(0.6f, 0.6f, 1f);
			GUI.Label(new Rect(this.pos.x + 1f, this.pos.y + 1f, 256f, 256f), string.Format("{0} {1}", this.name, this.values[this.index]));
		}

		// Token: 0x06009A9A RID: 39578 RVA: 0x003A60E4 File Offset: 0x003A42E4
		public void UpdateValues()
		{
			if (GameManager.Instance.World == null)
			{
				return;
			}
			EntityPlayerLocal player = this.manager.player;
			if (this.callback != null)
			{
				float num = this.values[this.index];
				if (this.callback(ref num))
				{
					this.AddValue(num);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.cvarName))
			{
				float cvar = player.GetCVar(this.cvarName);
				if (cvar != this.values[this.index])
				{
					this.AddValue(cvar);
					return;
				}
			}
			else if (this.passiveEffect != PassiveEffects.None)
			{
				float value = EffectManager.GetValue(this.passiveEffect, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				if (value != this.values[this.index])
				{
					this.AddValue(value);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.statName))
			{
				float num2 = 0f;
				string text = this.statName;
				if (!(text == "health"))
				{
					if (!(text == "stamina"))
					{
						if (!(text == "coretemp"))
						{
							if (text == "water")
							{
								num2 = player.PlayerStats.Water.Value;
							}
						}
						else
						{
							num2 = player.PlayerStats.CoreTemp;
						}
					}
					else
					{
						num2 = player.PlayerStats.Stamina.Value;
					}
				}
				else
				{
					num2 = player.PlayerStats.Health.Value;
				}
				if (num2 != this.values[this.index])
				{
					this.AddValue(num2);
				}
			}
		}

		// Token: 0x0400746C RID: 29804
		public string name;

		// Token: 0x0400746D RID: 29805
		public GameGraphManager.Graph.Callback callback;

		// Token: 0x0400746E RID: 29806
		public string cvarName;

		// Token: 0x0400746F RID: 29807
		public PassiveEffects passiveEffect;

		// Token: 0x04007470 RID: 29808
		public string statName;

		// Token: 0x04007471 RID: 29809
		public Vector2 pos;

		// Token: 0x04007472 RID: 29810
		[PublicizedFrom(EAccessModifier.Private)]
		public GameGraphManager manager;

		// Token: 0x04007473 RID: 29811
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04007474 RID: 29812
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxValue;

		// Token: 0x04007475 RID: 29813
		[PublicizedFrom(EAccessModifier.Private)]
		public float markerValue;

		// Token: 0x04007476 RID: 29814
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] values;

		// Token: 0x04007477 RID: 29815
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x02001324 RID: 4900
		// (Invoke) Token: 0x06009A9C RID: 39580
		public delegate bool Callback(ref float value);
	}
}
