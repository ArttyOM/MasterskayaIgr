using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SpriteShapeExtras
{
    public class Sprinkler : MonoBehaviour
    {
        public GameObject m_Prefab;
        public float m_RandomFactor = 10.0f;
        public bool m_UseNormals = false;

        private float Angle(Vector3 a, Vector3 b)
        {
            var dot = Vector3.Dot(a, b);
            var det = a.x * b.y - b.x * a.y;
            return Mathf.Atan2(det, dot) * Mathf.Rad2Deg;
        }

        // Use this for initialization. Plant the Prefabs on Startup
        private void Start()
        {
            var ssc = GetComponent<SpriteShapeController>();
            var spl = ssc.spline;

            for (var i = 1; i < spl.GetPointCount() - 1; ++i)
                if (Random.Range(0, 100) > 100 - m_RandomFactor)
                {
                    var go = Instantiate(m_Prefab);
                    go.transform.position = spl.GetPosition(i);

                    if (m_UseNormals)
                    {
                        var lt = Vector3.Normalize(spl.GetPosition(i - 1) - spl.GetPosition(i));
                        var rt = Vector3.Normalize(spl.GetPosition(i + 1) - spl.GetPosition(i));
                        var a = Angle(Vector3.up, lt);
                        var b = Angle(lt, rt);
                        var c = a + b * 0.5f;
                        if (b > 0)
                            c = 180 + c;
                        go.transform.rotation = Quaternion.Euler(0, 0, c);
                    }
                }
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}