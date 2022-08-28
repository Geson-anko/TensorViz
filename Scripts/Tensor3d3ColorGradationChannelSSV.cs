
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace TensorViz 
{
    /// <summary>
    /// このクラスは3dTensorをスポーンさせる特殊クラスです。
    /// channelは3で固定です。3dTensorのためwidth, heightは
    /// 2以上である必要があります。
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Tensor3d3ColorGradationChannelSSV : TensorSpawner
    {
        public Color color1_S = Color.blue;
        public Color color2_S = Color.yellow;
        public Color color3_V = Color.black;

        private float h1;
        private float s1;
        private float v1;

        private float h2;
        private float s2;
        private float v2;

        private float h3;
        private float s3;
        private float v3;

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

            channel = 3;
            Color.RGBToHSV(color1_S, out h1, out s1, out v1);
            Color.RGBToHSV(color2_S, out h2, out s2, out v2);
            Color.RGBToHSV(color3_V, out h3, out s3, out v3);
        }

        public override void OnElementSpawned(GameObject element, int w, int h, int c)
        {
            var rd = (Renderer)element.GetComponent(typeof(Renderer));
            rd.material.color = GetElementColor(w, h, c);

        }

        /// <summary>
        /// 1,2番目の色に対してはHSVのうちSを徐々に変化させます。
        /// 3番目の色に対してはVを徐々に変化させます。
        /// 距離はユークリッド距離を使用しています。
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

            float l = Mathf.Sqrt(wp * wp + hp * hp) / max;

            float s = 1.0f - l;
            
            switch (c)
            {
                case 0:
                    return Color.HSVToRGB(h1, s1 * s, v1);
                case 1:
                    return Color.HSVToRGB(h2, s2 * s, v2);
                case 2:
                    float v = l * (1.0f - v3) + v3;
                    return Color.HSVToRGB(h3, s3, v);
            }

            return Color.black;
        }

    }
}
