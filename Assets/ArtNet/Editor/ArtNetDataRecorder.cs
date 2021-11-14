/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

using System.IO;
using System.Linq;
using ArtNet.Runtime;
using UnityEditor;
using UnityEngine;

namespace ArtNet.Editor
{
    public class ArtNetDataRecorder : MonoBehaviour
    {
        #region serialize field

        [SerializeField] private ArtNetClient artNetClient;

        [SerializeField] private string directoryPath = "Record";

        [SerializeField] private string clipName = "NewArtNetClip";
        #endregion

        #region private field

        private AnimationCurve[] _curves;
        private float _time;
        private const int ChannelCount = 512;
        private bool _isRecoding;

        #endregion

        #region private Method

        private void Update()
        {
            if (_isRecoding) _time += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R)) RecordStart();
            if (Input.GetKeyDown(KeyCode.S)) RecordStop();
        }

        private void RecordStart()
        {
            _curves = new AnimationCurve[ChannelCount];
            for (int i = 0; i < ChannelCount; i++)
                _curves[i] = new AnimationCurve();

            _time = 0f;
            _isRecoding = true;
            artNetClient.onDataReceived += RecordingEventHandler;

            Debug.Log("RecordStart");
        }


        private void RecordingEventHandler(ArtNetData data)
        {
            if (data.OpCode != ArtNetOpCode.OpDmx)
            {
                return;
            }

            for (int i = 0; i < ChannelCount && i < data.Channels.Length; i++)
            {
                if (_curves[i].keys.Length > 2)
                {
                    var secondLast = _curves[i].keys.Length - 2;
                    var last = _curves[i].keys.Length - 1;
                    var secondLastKey = _curves[i].keys[secondLast];
                    var lastKey = _curves[i].keys[last];

                    if (secondLastKey.value == lastKey.value && lastKey.value == data.Channels[i])
                    {
                        _curves[i].RemoveKey(secondLast);
                    }
                }

                var key = new Keyframe(_time, data.Channels[i]);
                _curves[i].AddKey(key);
            }

            Debug.Log($"RecordingTime:{this._time}");
        }

        public void OnApplicationQuit()
        {
            this.RecordStop();
        }

        private void RecordStop()
        {
            if (!_isRecoding) return;

            AnimationClip clip = new AnimationClip();
            for (int i = 0; i < _curves.Length; i++) clip.SetCurve("", typeof(ArtNetChannels), $"Ch{i + 1}", _curves[i]);
            artNetClient.onDataReceived -= RecordingEventHandler;
            _curves = null;
            _isRecoding = false;
            
            var path = $"{Application.dataPath}/{directoryPath}";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var p = $"Assets/{directoryPath}/{clipName}.asset";
            while (File.Exists(p)) p = p.Split('.').First() + "_1.asset";
            AssetDatabase.CreateAsset(clip, p);
            AssetDatabase.Refresh();
            Debug.Log("Record Finish");
        }

        #endregion
    }
}