/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ArtNet
{
    public class ArtNetDataRecorder : MonoBehaviour
    {
        #region serialize field

        [SerializeField] private ArtNetClient artNetClient;

        [SerializeField] private string DirectoryPath = "Record";
        [SerializeField] private List<MonoBehaviour> _recordables = new List<MonoBehaviour>();
        #endregion

        #region private field

        private AnimationCurve[] curves;
        private float time;
        private const int max = 512;
        private bool isRecoding;
        
        #endregion

        #region private Method

        private void Update()
        {
            if (isRecoding) time += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R)) RecordStart();
            if (Input.GetKeyDown(KeyCode.S)) RecordStop();
        }

        private void RecordStart()
        {
            
            curves = new AnimationCurve[max];
            for (int i = 0; i < max; i++)
                curves[i] = new AnimationCurve();

            time = 0f;
            isRecoding = true;
            artNetClient.onDataReceived += RecordingEventHandler;
        }


        private void RecordingEventHandler(ArtNetData data)
        {
            for (int i = 0; i < max && i < data.Channels.Length; i++)
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
            
            Debug.Log($"RecordingTime:{this.time}");
        }

        public void OnApplicationQuit()
        {
            this.RecordStop();
        }

        private void RecordStop()
        {
        
            if (!isRecoding) return;

            int index = 0;
            List<AnimationClip> clips = new List<AnimationClip>();

            var iRecordables = _recordables.Select(x => x as IRecordable);
            foreach (var r in iRecordables)
            {
                AnimationClip clip = new AnimationClip();
                var property = r.GetProperty();
                
                for (int i = 0; i < property.Length; i++)
                    clip.SetCurve("", r.GetType(), property[i], curves[index + i]);
                
                index += property.Length;
                clips.Add(clip);
            }
            artNetClient.onDataReceived -= RecordingEventHandler;

            curves = null;
            isRecoding = false;
            
            foreach (var pair in iRecordables.Select((recordable, i) => new {i,recordable}))
            {
                var path = $"{Application.dataPath}/{DirectoryPath}/{pair.recordable.GetType()}";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var p =$"Assets/{DirectoryPath}/{pair.recordable.GetType()}/{((MonoBehaviour)pair.recordable).gameObject.name}.asset";
                while (File.Exists(p)) p = p.Split('.').First() + "_1.asset";
                AssetDatabase.CreateAsset(clips[pair.i], p);
                AssetDatabase.Refresh();
                Debug.Log("Record Finish");
            }
        }

        #endregion
    }


}