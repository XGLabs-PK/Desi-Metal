using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class BgScroller : MonoBehaviour
    {
        [SerializeField]
        float xSpeed = 0.08f;
        [SerializeField]
        float ySpeed = 0.08f;
        RawImage _img;

        void Start()
        {
            _img = GetComponent<RawImage>();
        }

        void Update()
        {
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(xSpeed, ySpeed)
                * Time.deltaTime, _img.uvRect.size);
        }
    }
}
