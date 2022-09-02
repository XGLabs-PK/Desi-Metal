using System;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class AllIn1VfxScrollShaderTexture : MonoBehaviour
    {
        [SerializeField]
        string texturePropertyName = "_MainTex";
        [SerializeField]
        Vector2 scrollSpeed = Vector2.zero;

        [Space]
        [Header("True changes tex offset, false to change tex scale")]
        [SerializeField]
        bool textureOffset = true;

        [Header("There are 3 modifiers, just pick 1")]
        [Space]
        [SerializeField]
        bool backAndForth;
        [SerializeField]
        Vector2 maxValue = Vector2.one;
        Vector2 iniValue = Vector2.zero;
        bool goingUpX, goingUpY;

        [Space]
        [SerializeField]
        bool applyModulo;
        [SerializeField]
        Vector2 modulo = Vector2.one;

        [Space]
        [SerializeField]
        bool stopAtValue;
        [SerializeField]
        Vector2 stopValue = Vector2.zero;

        [Space]
        [SerializeField]
        [Header("If missing uses an instance of the currently used Material")]
        Material mat;
        Material originalMat;
        bool restoreMaterialOnDisable = false;

        int propertyShaderID;
        Vector2 currValue = Vector2.zero;

        void Start()
        {
            //Get material if missing
            if (mat == null) mat = GetComponent<Renderer>().material;
            else
            {
                originalMat = new Material(mat);
                restoreMaterialOnDisable = true;
            }

            //Show error message if material or numericPropertyName property error
            //Otherwise cache shader property ID
            if (mat == null)
                DestroyComponentAndLogError(gameObject.name +
                                            " has no valid Material, deleting AllIn1VfxScrollShaderTexture component");
            else
            {
                if (mat.HasProperty(texturePropertyName)) propertyShaderID = Shader.PropertyToID(texturePropertyName);
                else
                    DestroyComponentAndLogError(gameObject.name + "'s Material doesn't have a " + texturePropertyName +
                                                " texture property");

                if (textureOffset) currValue = mat.GetTextureOffset(texturePropertyName);
                else currValue = mat.GetTextureScale(texturePropertyName);

                if (backAndForth || stopAtValue)
                {
                    iniValue = currValue;
                    goingUpX = iniValue.x < maxValue.x;
                    if (!goingUpX && scrollSpeed.x > 0) scrollSpeed *= -1f;
                    if (goingUpX && scrollSpeed.x < 0) scrollSpeed *= -1f;

                    goingUpY = iniValue.y < maxValue.y;
                    if (!goingUpY && scrollSpeed.y > 0) scrollSpeed *= -1f;
                    if (goingUpY && scrollSpeed.y < 0) scrollSpeed *= -1f;
                }
            }
        }

        bool isValid = true;

        void Update()
        {
            if (mat == null)
            {
                if (isValid)
                {
                    Debug.LogError("The object " + gameObject.name +
                                   " has no Material and you are trying to access it. Please take a look");

                    isValid = false;
                }

                return;
            }

            currValue += scrollSpeed * Time.deltaTime;

            if (backAndForth)
            {
                if (goingUpX && currValue.x >= maxValue.x) FlipGoingUp(true);
                else if (!goingUpX && currValue.x <= iniValue.x) FlipGoingUp(true);

                if (goingUpY && currValue.y >= maxValue.y) FlipGoingUp(false);
                else if (!goingUpY && currValue.y <= iniValue.y) FlipGoingUp(false);
            }

            if (applyModulo)
            {
                currValue.x %= modulo.x;
                currValue.y %= modulo.y;
            }

            if (stopAtValue)
            {
                if (goingUpX && currValue.x >= stopValue.x) scrollSpeed.x = 0f;
                else if (!goingUpX && currValue.x <= stopValue.x) scrollSpeed.x = 0f;

                if (goingUpY && currValue.y >= stopValue.y) scrollSpeed.y = 0f;
                else if (!goingUpY && currValue.y <= stopValue.y) scrollSpeed.y = 0f;
            }

            if (textureOffset) mat.SetTextureOffset(propertyShaderID, currValue);
            else mat.SetTextureScale(propertyShaderID, currValue);
        }

        void FlipGoingUp(bool isXComponent)
        {
            if (isXComponent)
            {
                goingUpX = !goingUpX;
                scrollSpeed.x *= -1f;
            }
            else
            {
                goingUpY = !goingUpY;
                scrollSpeed.y *= -1f;
            }
        }

        void DestroyComponentAndLogError(string logError)
        {
            Debug.LogError(logError);
            Destroy(this);
        }

        void OnDisable()
        {
            if (restoreMaterialOnDisable) mat.CopyPropertiesFromMaterial(originalMat);
        }
    }
}
