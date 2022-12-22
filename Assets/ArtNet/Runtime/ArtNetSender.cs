using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace ArtNet.Runtime
{
    public class ArtNetSender : MonoBehaviour
    {
        [SerializeField] private string host = "127.0.0.1";
        [SerializeField] public int port = 6454;
        private Queue<ArtNetData> bufferStream;
        private UdpClient _client;
        public bool IsActive => this._client != null;
        private int _port;
        public void Open()
        {
            if(_client != null) return;
            _port = port;
            _client = new UdpClient();
            _client.Connect(host, port);
        }

        private void OnEnable() => Open();
        private void OnDisable() => Close();

        public void Close()
        {
            if(_client == null) return;
            _client.Close();
            _client = null;
        }

        private void Update()
        {
            if (port != _port)
            {
                Close();
                Open();
            }

            _port = port;
        }

        public void Send(ArtNetData artNet)
        {
            var sendBuf = artNet.ToBytes();
            _client.Send(sendBuf, sendBuf.Length);
        }
    }
}