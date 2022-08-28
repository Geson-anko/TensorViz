
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace TensorViz 
{
    /// <summary>
    /// 対角線にそって2つの色のグラデーションをつけます。
    /// 2dTensorをスポーンさせるため`channel`項目は指定しても意味はありません。
    /// また、widthとheightは2以上に指定してください。
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Tensor2d2ColorDiagonalGradation : TensorSpawner
    {
        public Color color1 = Color.blue;
        public Color color2 = Color.yellow;

        private float h1;
        private float s1;
        private float v1;

        private float h2;
        private float s2;
        private float v2;

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

            channel = 1;

            Color.RGBToHSV(color1, out h1, out s1, out v1);
            Color.RGBToHSV(color2, out h2, out s2, out v2);
        }

        /// <summary>
        /// スポーンした`elementPrefab`のマテリアルカラーを変更します。
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        public override void OnElementSpawned(GameObject element, int w, int h, int c)
        {
            var rd = (Renderer)element.GetComponent(typeof(Renderer));
            rd.material.color = GetElementColor(w, h);

        }

        /// <summary>
        /// 二つのColorをHSVのうちSを連続的に変化させることによって
        /// 二色のグラデーションを実装します。       
        /// マンハッタン距離を使用して対角線のグラデーションを作成しています。
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private Color GetElementColor(int w, int h)
        {
            float max = 2.0f;
            float wp = (1.0f - (float)w / (float)(width - 1));
            float hp = (1.0f - (float)h / (float)(height - 1));

            float l = (wp + hp) / max;

            float s = 2.0f * l - 1.0f;


            if (s > 0.0f)
            {
                return Color.HSVToRGB(h1, s * s1, v1);
            }
            else
            {
                s = Mathf.Abs(s);
                return Color.HSVToRGB(h2, s * s2, v2);
            }
        }
    }
}
