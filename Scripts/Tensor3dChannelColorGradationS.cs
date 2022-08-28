
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace TensorViz
{
    /// <summary>
    /// Colorの配列に指定した順にチャネルの色を付けます。
    /// channelの値はColor配列の値と等しくなります。
    /// Color配列の長さは1以上、widthとheightの大きさは2以上にしてください。
    /// HSVのうち、Sを変化させてグラデーションをつけます。
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Tensor3dChannelColorGradationS : TensorSpawner
    {
        public Color[] colors;

        private float[] hArray;
        private float[] sArray;
        private float[] vArray;

        public override void OnStart()
        {
            if (width < 2)
            {
                Debug.LogError($"width value must be > 1! Current value: {width}");
            }

            if (height < 2)
            {
                Debug.LogError($"height value must be > 1! Current value: {height}");
            }

            channel = colors.Length;

            if (channel < 1)
            {
                Debug.LogError($"channel value must be > 0! Current value: {channel}");
            }

            hArray = new float[channel];
            sArray = new float[channel];
            vArray = new float[channel];

            for (int c = 0; c < channel; c++)
            {
                float h, s, v;
                Color.RGBToHSV(colors[c], out h, out s, out v);
                hArray[c] = h;
                sArray[c] = s;
                vArray[c] = v;
            }

        }
        public override void OnElementSpawned(GameObject element, int w, int h, int c)
        {
            var rd = (Renderer)element.GetComponent(typeof(Renderer));
            rd.material.color = GetElementColor(w, h, c);

        }

        /// <summary>
        /// Tensorの各要素の色を計算し、返します。
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private Color GetElementColor(int w, int h, int c)
        {
            float max = Mathf.Sqrt(2.0f);
            float wp = (1.0f - (float)w / (float)(width - 1));
            float hp = (1.0f - (float)h / (float)(height - 1));

            float s = Mathf.Sqrt(wp * wp + hp * hp) / max;

            return Color.HSVToRGB(hArray[c], sArray[c] * s, vArray[c]);
        }
    }
}
