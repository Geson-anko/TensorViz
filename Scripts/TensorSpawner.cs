
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace TensorViz
{
    /// <summary>
    /// Tensorをスポーンさせる基底クラスです。GameObjectにアタッチして使用します。
    /// OnTensorSpawnedというオーバーライドメソッドを派生クラスで使用することによって
    /// Tensorに色付けをしたり、形状、位置を変更することができます。
    /// </summary>
    /// 
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class TensorSpawner : UdonSharpBehaviour
    {
        public int width = 1;
        public int height = 1;
        public int channel = 1;
        public float arrangeInterval = 1.0f;

        public GameObject elementPrefab;

        void Start()
        {
            if (elementPrefab == null)
            {
                Debug.LogError("`ElementPrefab` is not set in the Inspector.");
                return;
            }
            OnStart();
            SpawnTensor();
            OnTensorSpawned();
        }

        /// <summary>
        /// 基底クラスの`void Start`ではじめに呼ばれるメソッドです。
        /// </summary>
        public virtual void OnStart()
        {
            
        }

        /// <summary>
        /// `elementPrefab`を配置してTensorをスポーンします。
        /// width * height * channel 個のelementPrefabを arrangeIntervalを開けて配置します。
        /// </summary>
        public void SpawnTensor()
        {
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    for (int c = 0; c < channel; c++)
                    {
                        var instance = Instantiate(elementPrefab, transform);
                        var x = arrangeInterval * w;
                        var y = arrangeInterval * h;
                        var z = arrangeInterval * c;
                        instance.transform.localPosition = new Vector3(x, y, z);

                        OnElementSpawned(instance, w, h, c);

                    }
                }
            }
        }
        
        /// <summary>
        /// Tensorの要素`elementPrefab`を一つ配置したときに呼ばれるメソッドです。
        /// w, h, c はその要素の位置です。ゼロオリジンです。
        /// </summary>
        /// <param name="element"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        public virtual void OnElementSpawned(GameObject element, int w, int h, int c)
        {
            
        }

        /// <summary>
        /// Elementをすべて配置し終わったあとに呼ばれるメソッドです。
        /// </summary>
        public virtual void OnTensorSpawned()
        {

        }


    }
}
