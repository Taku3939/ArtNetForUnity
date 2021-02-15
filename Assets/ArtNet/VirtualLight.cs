/*!
 * Copyright (c) 2021 Takuya Isaki
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */
using System;
using UnityEngine;

namespace ArtNet
{
    /// <summary>
    /// Unityで使う仮想灯台につける
    /// Animationで配列を使うことができないので21チャンネルで決め打ち
    /// </summary>
    [Serializable]
    public class VirtualLight : MonoBehaviour
    {
        public int ch0, 
            ch1,
            ch2,
            ch3,
            ch4,
            ch5,
            ch6,
            ch7,
            ch8,
            ch9,
            ch10,
            ch11,
            ch12,
            ch13,
            ch14,
            ch15,
            ch16,
            ch17,
            ch18,
            ch19,
            ch20;
    }
}