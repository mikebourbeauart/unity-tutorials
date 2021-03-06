﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathSurface : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)] 
    public int resolution = 10;
    
    public GraphFunctionName function;

    Transform[] points;


    void Awake () {
		float step = 2f / resolution;
		Vector3 scale = Vector3.one * step;
		Vector3 position;
		position.y = 0f;
        position.z = 0f;

        points = new Transform[resolution];
		for (int i = 0; i < points.Length; i++) {
			Transform point = Instantiate(pointPrefab);
			position.x = (i + 0.5f) * step - 1f;
			point.localPosition = position;
			point.localScale = scale;
            point.SetParent(transform, false);
            position.x = (i + 0.5f) * step - 1f;
            points[i] = point;
		}
	}

    void Update() {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        for(int i=0; i < points.Length; i++){
            Transform point = points[i];
            float step = 2f / resolution;
            Vector3 position = point.localPosition;
            point.localPosition = position;
        }
    }

    static GraphFunction[] functions = {
            SineFunction, MultiSineFunction
        };
    static float SineFunction (float x, float t) {
        return Mathf.Sin(Mathf.PI * (x + t));
    }
    static float MultiSineFunction  (float x, float t) {
        float y = Mathf.Sin(Mathf.PI * (x + t));
        y += Mathf.Sin(2f * Mathf.PI * (x + 2f * t)) / 2f;
        y *= 2f/3f;
        return y;
    }
}
