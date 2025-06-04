using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StatsMenu: MonoBehaviour
    {
        public GameHttpProviderBase httpProvider;
        private SynchronizationContext unityContext;
        public TMP_Text errorText;
        public TMP_Text totalGamesText;
        public TMP_Text totalWinsText;
        public GameObject mainMenu;
        public void Awake()
        {
            unityContext = SynchronizationContext.Current;
        }
        public async void OnEnable()
        {
            var result = await httpProvider.GetMyStats();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                errorText.text = result.ErrorMessage;
                return;
            }

            totalGamesText.text = $"Total games: {result.Data.totalGames}";
            totalWinsText.text = $"Won games: {result.Data.wonGames}";
        }

        public void OnBack()
        {
            ResetText();
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        private void ResetText()
        {
            totalGamesText.text = "Total games: ";
            totalWinsText.text = "Won games: ";
        }

        private void OnDestroy()
        {
            ResetText();
        }
    }
}