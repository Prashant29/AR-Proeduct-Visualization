using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        bool PingPong = false;
        void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }
            SetValue(1);
           
            
        }

        private void OnEnable()
        {
            SetValue(1);
            Appear();
        }

        public void Appear()
        {
            StartCoroutine(AppearObj());
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        public void Disappear()
        {
            StartCoroutine(DisappearObj());
        }

        // Update is called once per frame
        void Update()
        {

           /* var value = Mathf.PingPong(Time.time * 0.5f, 1f);
            SetValue(value);*/
        }

        IEnumerator AppearObj()
        {
            Debug.Log("Appear");
            float value = 1;      
            while (value > 0)
            {
                Mathf.PingPong(value, 1f);
                value -= Time.deltaTime;
                SetValue(value);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator DisappearObj()
        {
            Debug.Log("Disappear");

            float value = 0;
            while (value < 1)
            {
                Mathf.PingPong(value, 1f);
                value += Time.deltaTime;
                SetValue(value);
                yield return new WaitForEndOfFrame();
            }
        }

        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}