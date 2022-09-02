using System;
using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class AllIn1VfxScrollShaderProperty : MonoBehaviour
    {
        [SerializeField]
        string numericPropertyName = "_HsvShift";
        [SerializeField]
        float scrollSpeed;

        [Header("There are 3 modifiers, just pick 1")]
        [Space]
        [SerializeField]
        bool backAndForth;
        [SerializeField]
        float maxValue = 1f;
        float iniValue;
        bool goingUp;

        [Space]
        [SerializeField]
        bool applyModulo;
        [SerializeField]
        float modulo = 360f;

        [Space]
        [SerializeField]
        bool stopAtValue;
        [SerializeField]
        float stopValue;

        [Space]
        [SerializeField]
        [Header("If missing uses an instance of the currently used Material")]
        Material mat;
        Material originalMat;
        bool restoreMaterialOnDisable = false;

        int propertyShaderID;
        float currValue;

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
                                            " has no valid Material, deleting AllIn1VfxScrollShaderProperty component");
            else
            {
                if (mat.HasProperty(numericPropertyName)) propertyShaderID = Shader.PropertyToID(numericPropertyName);
                else
                    DestroyComponentAndLogError(gameObject.name + "'s Material doesn't have a " + numericPropertyName +
                                                " property");

                currValue = mat.GetFloat(propertyShaderID);

                if (backAndForth || stopAtValue)
                {
                    iniValue = currValue;
                    goingUp = iniValue < maxValue;
                    if (!goingUp && scrollSpeed > 0) scrollSpeed *= -1f;
                    if (goingUp && scrollSpeed < 0) scrollSpeed *= -1f;
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
                if (goingUp && currValue >= maxValue) FlipGoingUp();
                else if (!goingUp && currValue <= iniValue) FlipGoingUp();
            }

            if (applyModulo) currValue %= modulo;

            if (stopAtValue)
            {
                if (goingUp && currValue >= stopValue) scrollSpeed = 0f;
                else if (!goingUp && currValue <= stopValue) scrollSpeed = 0f;
            }

            mat.SetFloat(propertyShaderID, currValue);
        }

        void FlipGoingUp()
        {
            goingUp = !goingUp;
            scrollSpeed *= -1f;
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
