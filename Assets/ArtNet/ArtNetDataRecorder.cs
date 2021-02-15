/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

using UnityEditor;
using UnityEngine;

namespace ArtNet
{
    public class ArtNetDataRecorder : MonoBehaviour
    {
        #region serialize field

        [SerializeField] private ArtNetClient artNetClient;
        [SerializeField] private int startChannel;
        [SerializeField] private string path = "Assets/SampleAnimationClip.asset";

        #endregion

        #region private field

        private AnimationCurve[] curves;
        private float time;
        private const int max = 21;
        private bool isRecoding;

        #endregion

        #region private Method

        private void Update()
        {
            if (isRecoding) time += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R))
            {
                RecordStart();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RecordStop();
            }
        }

        private void RecordStart()
        {
            curves = new AnimationCurve[max];
            for (int i = startChannel; i < startChannel + max && startChannel < 512; i++)
                curves[i] = new AnimationCurve();

            time = 0f;
            isRecoding = true;
            artNetClient.onDataReceived += RecordingEventHandler;
            Debug.Log("Record Start");
        }


        private void RecordingEventHandler(ArtNetData data)
        {
            for (int i = startChannel; i < startChannel + max && i < data.Channels.Length; i++)
            {
                if (curves[i].keys.Length > 2)
                {
                    var secondLast = curves[i].keys.Length - 2;
                    var last = curves[i].keys.Length - 1;
                    var secondLastKey = curves[i].keys[secondLast];
                    var lastKey = curves[i].keys[last];

                    if (secondLastKey.value == lastKey.value && lastKey.value == data.Channels[i])
                    {
                        curves[i].RemoveKey(secondLast);
                    }
                }

                var key = new Keyframe(time, data.Channels[i]);
                curves[i].AddKey(key);
            }
        }

        public void OnApplicationQuit()
        {
            this.RecordStop();
        }

        private void RecordStop()
        {
            AnimationClip clip = new AnimationClip();
            if (!isRecoding || clip == null) return;

            for (int i = startChannel; i < startChannel + max && i < curves.Length; i++)
            {
                clip.SetCurve("", typeof(VirtualLight), $"ch{i}", curves[i]);
            }


            artNetClient.onDataReceived -= RecordingEventHandler;

            curves = null;
            isRecoding = false;

            var p = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(clip, p);
            AssetDatabase.Refresh();
            Debug.Log("Record Stop");
        }

        #endregion
    }
}