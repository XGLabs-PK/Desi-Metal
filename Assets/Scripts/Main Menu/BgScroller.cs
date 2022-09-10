using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace XGStudios
{
    public class BgScroller : MonoBehaviour
    {
        [SerializeField]
        float xSpeed;
        [SerializeField]
        float ySpeed;
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
