namespace _Game.Scripts.Gameplay.UI
{
    using BlockSmash.Extensions;
    using BlockSmash.Managers;
    using UnityEngine;
    using UnityEngine.UI;
    using VContainer;

    [RequireComponent(typeof(Button))]
    public class ButtonPlayAgain : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        
        private Button button;

        private GameManager gameManager;
        private GameManager GameManager => this.gameManager ??= this.GetCurrentContainer().Resolve<GameManager>();

        private void Awake()
        {
            this.button = this.GetComponent<Button>();

            this.button.onClick.AddListener(this.OnPlayAgainClicked);
        }

        private void OnDestroy()
        {
            this.button.onClick.RemoveListener(this.OnPlayAgainClicked);
        }

        private void OnPlayAgainClicked()
        {
            this.popup.SetActive(false);
            this.GameManager.Replay();
        }
    }
}