using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ModernProgramming.LoadingScreen
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        // This is the index of the next scene you want to load.
        [Header("Scene Information")] 
        public int sceneIndexToLoad;
        
        // UI elements inside the loading scene.
        [Header("Loading Screen")]
        public GameObject loadingScreen;
        public Slider slider;
        public Text progressLabel;
        
        // String array reduces memory usage, no need to create/concatenate new strings every frame.
        private static readonly string[] PercentStrings =
        {
            "0%", "1%", "2%", "3%", "4%", "5%", "6%", "7%", "8%", "9%",
            "10%", "11%", "12%", "13%", "14%", "15%", "16%", "17%", "18%", "19%",
            "20%", "21%", "22%", "23%", "24%", "25%", "26%", "27%", "28%", "29%",
            "30%", "31%", "32%", "33%", "34%", "35%", "36%", "37%", "38%", "39%",
            "40%", "41%", "42%", "43%", "44%", "45%", "46%", "47%", "48%", "49%",
            "50%", "51%", "52%", "53%", "54%", "55%", "56%", "57%", "58%", "59%",
            "60%", "61%", "62%", "63%", "64%", "65%", "66%", "67%", "68%", "69%",
            "70%", "71%", "72%", "73%", "74%", "75%", "76%", "77%", "78%", "79%",
            "80%", "81%", "82%", "83%", "84%", "85%", "86%", "87%", "88%", "89%",
            "90%", "91%", "92%", "93%", "94%", "95%", "96%", "97%", "98%", "99%",
            "100%"
        };

        private bool _isLoadingScreenNotNull;
        private bool _isSliderNull;
        private bool _isProgressLabelNotNull;

        private void Awake()
        {
            // Check if all components are added in inspector.
            _isProgressLabelNotNull = progressLabel != null;
            _isSliderNull = slider == null;
            _isLoadingScreenNotNull = loadingScreen != null;
        }

        public void Start()
        {
            // Stop running the script if we can't find a slider to manipulate.
            if (_isSliderNull)
            {
                Debug.Log("Modern Loading Screen - ERROR Please assign a loading slider in the inspector!");
                return;
            }
            
            // Start the loading process.
            StartCoroutine(LoadAsynchronously(sceneIndexToLoad));
        }

        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            // Show the loading screen.
            if (_isLoadingScreenNotNull) loadingScreen.SetActive(true);
            
            // Start loading the next scene in a new thread.
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            // Update the slider while the scene is still loading.
            while (!operation.isDone)
            {
                // Unity scenes loads from 0.0f to 0.9f, so we need to clamp our float to this range.
                // The final 0.9f to 1.0f is for unloading the current scene, we don't need to show that progress.
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                // Update our UI elements.
                slider.value = progress;
                if (_isProgressLabelNotNull) progressLabel.text = PercentStrings[(int) (progress * 100f)];
                
                // Wait for next frame.
                yield return null;
            }
        }
    }
}