using ArtNet;
using UnityEngine;

public class SampleLightReceiver : MonoBehaviour
{
    [SerializeField] private ArtNetClient client;
    [SerializeField] private Light light;

    private void Start()
    {
        this.client.onDataReceived += data =>
        {
            if (data.OpCode != 20480)
            {
                light.color = new Color(
                    data.Channels[0],
                    data.Channels[1],
                    data.Channels[2]);
            }
        };
    }
}