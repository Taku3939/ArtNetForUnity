/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ArtNet
{
    public class ArtNetClient : MonoBehaviour, IDisposable
    {
        #region public field
        
        public int port = 6454;
        public event Action<ArtNetData> onDataReceived;
        public bool IsActive => this.client != null;
        
        #endregion
        
        #region private field
        
        private UdpClient client;
        private CancellationTokenSource cts;
        private SynchronizationContext context;
        
        #endregion
        
        #region public method
        public void Open()
        {
            if(client != null) return;
            context = SynchronizationContext.Current;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            client = new UdpClient(ipEndPoint);
            cts = new CancellationTokenSource();
            Task.Run(() => Loop(cts.Token), cts.Token);
        }
        public void Close() => Dispose();
        
        public void Dispose()
        {
            if(client == null) return;
            cts.Cancel();
            client.Close();
            client = null;
        }


        #endregion
        
        #region private method
        private void OnEnable() => Open();
        private void OnDisable() => Close();

        private void OnApplicationQuit() => Close();

        private async Task Loop(CancellationToken token)
        {
            if (client == null) return;
            
            while (true)
            {
                if (token.IsCancellationRequested) return;
                try
                {
                    while (client.Available != 0)
                    {
                        var buf = await client.ReceiveAsync();
                        context.Post(_ => onDataReceived?.Invoke(new ArtNetData(buf.Buffer)), null);
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