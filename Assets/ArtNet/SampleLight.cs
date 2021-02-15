using ArtNet;
using UnityEngine;

public class SampleLight : MonoBehaviour
{
    [Header("ArtNetの場合はtrue、VirutualLightのデータを使う場合はfalse")] [SerializeField]
    private bool ArtNetOrVirtualLight;

    [SerializeField] private Light light;
    [SerializeField] private ArtNetClient client;
    [SerializeField] private VirtualLight virtualLight;

    private void Start()
    {
        repository = new VirtualLightRepository(virtualLight);
        if (ArtNetOrVirtualLight)
            client.onDataReceived += data => Lighting(new ArtNetLightRepository(data));
    }

    private ILight repository;
    private void Update()
    {
        if (!ArtNetOrVirtualLight) Lighting(repository);
    }

    private void Lighting(ILight iLight)
    {
        var r = iLight.GetChannel(0);
        var g = iLight.GetChannel(1);
        var b = iLight.GetChannel(2);
        light.color = new Color(r, g, b);
    }

    class VirtualLightRepository : ILight
    {
        private VirtualLight virtualLight;

        public VirtualLightRepository(VirtualLight virtualLight)
        {
            this.virtualLight = virtualLight;
        }
        
        public int GetChannel(int index)
        {
            switch (index)
            {
                case 0: return virtualLight.ch0;
                case 1: return virtualLight.ch1;
                case 2: return virtualLight.ch2;
                case 3: return virtualLight.ch3;
                case 4: return virtualLight.ch4;
                case 5: return virtualLight.ch5;
                case 6: return virtualLight.ch6;
                case 7: return virtualLight.ch7;
                case 8: return virtualLight.ch8;
                case 9: return virtualLight.ch9;
                case 10: return virtualLight.ch10;
                case 11: return virtualLight.ch11;
                case 12: return virtualLight.ch12;
                case 13: return virtualLight.ch13;
                case 14: return virtualLight.ch14;
                case 15: return virtualLight.ch15;
                case 16: return virtualLight.ch16;
                case 17: return virtualLight.ch17;
                case 18: return virtualLight.ch18;
                case 19: return virtualLight.ch19;
                case 20: return virtualLight.ch20;
                default: return 0;
            }
        }
    }

    class ArtNetLightRepository : ILight
    {
        private ArtNetData data;
        public ArtNetLightRepository(ArtNetData data) => this.data = data;
        public int GetChannel(int index) => data.Channels[index];
    }

    interface ILight
    {
        int GetChannel(int index);
    }
}