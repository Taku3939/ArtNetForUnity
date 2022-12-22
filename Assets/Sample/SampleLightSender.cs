using System;
using System.Collections;
using System.Collections.Generic;
using ArtNet.Runtime;
using UnityEngine;

public class SampleLightSender : MonoBehaviour
{
    [SerializeField] private ArtNetSender sender;
    [SerializeField] [Range(0, 255)] private int[] channels = new int[512];

    private void Update()
    {
        sender.Send(channels);
    }
}