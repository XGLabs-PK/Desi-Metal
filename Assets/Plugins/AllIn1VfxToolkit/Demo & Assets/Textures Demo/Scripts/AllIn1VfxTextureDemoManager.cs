using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.DemoAssets.TexturesDemo.Scripts
{
    public class AllIn1VfxTextureDemoManager : MonoBehaviour
    {
        [SerializeField]
        int startingCollectionIndex, startingPageIndex;

        [Space]
        [Header("Demo Textures")]
        [SerializeField]
        All1VfxDemoTextureCollection[] textureCollections;

        [Space]
        [Header("Demo Controller Input")]
        [SerializeField]
        KeyCode nextPageKey = KeyCode.RightArrow;
        [SerializeField]
        KeyCode nextPageKeyAlt = KeyCode.D;
        [SerializeField]
        KeyCode previousPageKey = KeyCode.LeftArrow;
        [SerializeField]
        KeyCode previousPageKeyAlt = KeyCode.A;
        [SerializeField]
        KeyCode nextCollectionKey = KeyCode.UpArrow;
        [SerializeField]
        KeyCode nextCollectionKeyAlt = KeyCode.W;
        [SerializeField]
        KeyCode previousCollectionKey = KeyCode.DownArrow;
        [SerializeField]
        KeyCode previousCollectionKeyAlt = KeyCode.S;

        [Space]
        [Header("References")]
        [SerializeField]
        RawImage[] images;
        [SerializeField]
        Text collectionText, pageText;
        [SerializeField]
        AllIn1DemoScaleTween expositorTween,
            nextPageButtTween,
            prevPageButtTween,
            nextCollectionButtTween,
            prevCollectionButtTween;

        int currTextureCollectionIndex, currTextureIndex, numberOfImagesPerPage;

        void Start()
        {
            currTextureCollectionIndex = startingCollectionIndex;
            currTextureIndex = startingPageIndex;
            numberOfImagesPerPage = images.Length;

            RefreshCollectionAndPageText();
            AssignCurrentImages();
        }

        void Update()
        {
            if (Input.GetKeyDown(nextPageKey) || Input.GetKeyDown(nextPageKeyAlt)) ChangeTextureIndex(1);
            if (Input.GetKeyDown(previousPageKey) || Input.GetKeyDown(previousPageKeyAlt)) ChangeTextureIndex(-1);

            if (Input.GetKeyDown(nextCollectionKey) || Input.GetKeyDown(nextCollectionKeyAlt)) ChangeCollectionIndex(1);

            if (Input.GetKeyDown(previousCollectionKey) || Input.GetKeyDown(previousCollectionKeyAlt))
                ChangeCollectionIndex(-1);
        }

        public void ChangeTextureIndex(int pagesAmount)
        {
            currTextureIndex += pagesAmount * numberOfImagesPerPage;

            if (pagesAmount > 0) nextPageButtTween.ScaleDownTween();
            else prevPageButtTween.ScaleDownTween();

            expositorTween.ScaleDownTween();

            bool hasOverflowed = false;

            if (currTextureIndex < 0)
            {
                hasOverflowed = true;
                ChangeCollectionIndex(-1);
            }
            else if (currTextureIndex >= textureCollections[currTextureCollectionIndex].demoTextureCollection.Length)
            {
                hasOverflowed = true;
                ChangeCollectionIndex(1);
            }

            if (!hasOverflowed)
            {
                AssignCurrentImages();
                RefreshCollectionAndPageText();
            }
        }

        public void ChangeCollectionIndex(int collectionChangeAmount)
        {
            currTextureCollectionIndex += collectionChangeAmount;

            if (collectionChangeAmount > 0) nextCollectionButtTween.ScaleDownTween();
            else prevCollectionButtTween.ScaleDownTween();

            expositorTween.ScaleDownTween();

            if (currTextureCollectionIndex < 0) currTextureCollectionIndex = textureCollections.Length - 1;
            else if (currTextureCollectionIndex >= textureCollections.Length) currTextureCollectionIndex = 0;

            if (collectionChangeAmount > 0) currTextureIndex = 0;
            else
            {
                int lastPageTextureAmount =
                    textureCollections[currTextureCollectionIndex].demoTextureCollection.Length % numberOfImagesPerPage;

                if (lastPageTextureAmount == 0) lastPageTextureAmount = numberOfImagesPerPage;

                currTextureIndex = textureCollections[currTextureCollectionIndex].demoTextureCollection.Length -
                                   lastPageTextureAmount;
            }

            AssignCurrentImages();
            RefreshCollectionAndPageText();
        }

        void RefreshCollectionAndPageText()
        {
            collectionText.text = textureCollections[currTextureCollectionIndex].collectionName + " Collection";
            int currentPage = 0;

            int maxPages =
                (int)Mathf.Ceil((float)textureCollections[currTextureCollectionIndex].demoTextureCollection.Length /
                                (float)numberOfImagesPerPage);

            if (currTextureIndex > 1) currentPage = currTextureIndex / numberOfImagesPerPage;
            currentPage++;
            pageText.text = currentPage + "/" + maxPages;
        }

        void AssignCurrentImages()
        {
            int currPageIndex = 0;

            foreach (RawImage currImage in images)
            {
                if (currTextureIndex + currPageIndex >=
                    textureCollections[currTextureCollectionIndex].demoTextureCollection.Length)
                {
                    currImage.enabled = false;
                    continue;
                }

                currImage.enabled = true;

                currImage.texture = textureCollections[currTextureCollectionIndex]
                    .demoTextureCollection[currTextureIndex + currPageIndex];

                currPageIndex++;
            }
        }
    }
}
