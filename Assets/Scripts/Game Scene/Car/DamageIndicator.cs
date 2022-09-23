using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace XG.Studios
{
    public class DamageIndicator : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public float lifeTime = 0.6f;
        public float minDis = 2f;
        public float maxDis = 3f;

        Vector3 intPos;
        Vector3 targetPos;
        float timer;

        void Start()
        {
            transform.LookAt(2 * transform.position - Camera.main.transform.position);
            float direction = Random.rotation.eulerAngles.x;
            intPos = transform.position;
            float dis = Random.Range(minDis, maxDis);
            targetPos = intPos + Quaternion.Euler(0, 0, direction) * new Vector3(dis, dis, 0f);
            transform.localScale = Vector3.zero;
        }

        void Update()
        {
            timer += Time.deltaTime;

            float fraction = lifeTime / 2;

            if (timer > lifeTime) Destroy(gameObject);
            else if (timer > fraction)
                text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifeTime - fraction));

            transform.localPosition = Vector3.Lerp(intPos, targetPos, Mathf.Sin(timer / lifeTime));
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifeTime));
        }

        public void SetDamageInt(float damage)
        {
            text.text = damage.ToString();
        }
    }
}
