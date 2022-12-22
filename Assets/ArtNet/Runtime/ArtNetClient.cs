/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ArtNet.Runtime
{
    public class ArtNetClient : MonoBehaviour, IDisposable
    {
        #region public field
        public string host;
        public int port = 6454;
        public event Action<ArtNetData> onDataReceived;

        private Queue<byte[]> _bufferStream;
        public bool IsActive => this._client != null;
        #endregion
        
        #region private field
        
        private UdpClient _client;
        private CancellationTokenSource _cts;

        #endregion
        
        #region public method
        public void Open()
        {
            if(_client != null) return;
            _bufferStream = new Queue<byte[]>();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(host),port);
            _client = new UdpClient(ipEndPoint);
            _cts = new CancellationTokenSource();
            Task.Run(() => Loop(_cts.Token), _cts.Token);
        }
        public void Close() => Dispose();
        
        public void Dispose()
        {

            if(_client == null) return;
            _cts.Cancel();
            _client.Close();
            _client = null;
        }
        
        #endregion
        
        #region private method

        private void Update()
        {
            while (_bufferStream.Count > 0)
            {
                var buffer = _bufferStream.Dequeue();
                if(ArtNetData.IsArtNet(buffer)) 
                    onDataReceived?.Invoke(new ArtNetData(buffer));
            }
        }

        private void OnEnable() => Open();
        private void OnDisable() => Close();

        private void OnApplicationQuit() => Close();

        private async Task Loop(CancellationToken token)
        {
            if (_client == null) return;
            while (true)
            {
                if (token.IsCancellationRequested) return;
                try
                {
                    while (_client.Available != 0)
                    {
                        var buf = await _client.ReceiveAsync();
                        _bufferStream.Enqueue(buf.Buffer);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        #endregion
    }
}