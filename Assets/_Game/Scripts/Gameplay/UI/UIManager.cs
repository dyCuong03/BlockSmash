namespace _Game.Scripts.Gameplay.UI
{
    using System;
    using BlockSmash.Extensions;
    using BlockSmash.Signals.BlockSmash.Signals;
    using MessagePipe;
    using UnityEngine;
    using VContainer;

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject lose;
        
        private ISubscriber<GameLoseSignal> loseSubscriber;
        private IDisposable                 loseSubscription;

        private void OnEnable()
        {
            this.loseSubscriber   = this.GetCurrentContainer().Resolve<ISubscriber<GameLoseSignal>>();
            this.loseSubscription = this.loseSubscriber.Subscribe(this.OnGameLose);
        }

        private void OnDisable()
        {
            this.loseSubscription.Dispose();
        }

        private void OnGameLose(GameLoseSignal signal)
        {
            this.lose.gameObject.SetActive(true);
        }
    }
}