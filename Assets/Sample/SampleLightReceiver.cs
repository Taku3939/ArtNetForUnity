using System;
using System.Collections;
using System.Collections.Generic;
using ArtNet;
using UnityEngine;

public class SampleLightReceiver : MonoBehaviour
{
   [SerializeField] private ArtNetClient client;
   [SerializeField] private SampleLight sampleLight;
   private void Start()
   {
      this.client.onDataReceived += data =>
      {
         sampleLight.ch1 = data.Channels[0];
         sampleLight.ch2 = data.Channels[1];
         sampleLight.ch3 = data.Channels[2];
      };
   }
}
