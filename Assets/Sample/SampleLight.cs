using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ArtNet;
using UnityEngine;

public class SampleLight : MonoBehaviour, IRecordable
{
    private bool ArtNetOrVirtualLight;

    [SerializeField] private Light light;
    public int ch1, ch2, ch3;
    public string[] GetProperty() => Enumerable.Range(1, 3).Select(x => "ch" + x).ToArray();
    

    public void Update()
    {
        var r = ch1 / 255f;
        var g = ch2 / 255f;
        var b = ch3 / 255f;
        light.color = new UnityEngine.Color(r, g, b, 255);
    }
}
