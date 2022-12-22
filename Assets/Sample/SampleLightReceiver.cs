using ArtNet.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

public class SampleLightReceiver : MonoBehaviour
{
    [FormerlySerializedAs("client")] [SerializeField] private ArtNetReceiver receiver;
    [SerializeField] private new Light light;

    private void Start()
    {
        this.receiver.OnDataReceived += data =>
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