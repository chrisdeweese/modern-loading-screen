using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ModernProgramming.LoadingScreen
{
    public sealed class LoadingScreen : MonoBehaviour
    {
        [Header("Scene Information")] 
        public int sceneIndexToLoad;
        
        [Header("Loading Screen")]
        public GameObject loadingScreen;
        public Slider slider;
        public Text progressLabel;
        
        private static readonly string[] PercentStrings = {
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

        private bool _isloadingScreenNotNull;
        private bool _issliderNull;
        private bool _isprogressLabelNotNull;

        private void Awake()
        {
            _isprogressLabelNotNull = progressLabel != null;
            _issliderNull = slider == null;
            _isloadingScreenNotNull = loadingScreen != null;
        }

        public void Start()
        {
            StartCoroutine(LoadAsynchronously(sceneIndexToLoad));
        }

        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            if (_isloadingScreenNotNull) loadingScreen.SetActive(true);
            
            if (_issliderNull)
            {
                Debug.Log("Modern Loading Screen - ERROR Please assign a loading slider in the inspector!");
                yield break;
            }
            
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                slider.value = progress;
                if (_isprogressLabelNotNull) progressLabel.text = PercentStrings[(int) (progress * 100f)];
                
                yield return null;
            }
        }
    }
}