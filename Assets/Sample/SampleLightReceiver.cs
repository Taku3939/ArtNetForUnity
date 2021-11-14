using ArtNet.Runtime;
using UnityEngine;

public class SampleLightReceiver : MonoBehaviour
{
    [SerializeField] private ArtNetClient client;
    [SerializeField] private new Light light;

    private void Start()
    {
        this.client.onDataReceived += data =>
        {
            if (data.OpCode == ArtNetOpCode.OpDmx)
            {
                light.color = new Color(
                    data.Channels[0],
                    data.Channels[1],
                    data.Channels[2]);
            }
        };
    }
}