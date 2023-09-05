using System;
using System.Collections.Generic;
using Code.Model;
using Code.Shared;
using Cysharp.Threading.Tasks;
using Game.Model.Popups;
using UnityEngine;

namespace Code.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private ObjectPoolsController objectPoolsController;
        
        private readonly Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();
        private readonly List<IPopup> popups = new List<IPopup>();
        
        public static Game Instance { get; private set; }
        public LeaderboardModel LeaderboardModel { get; private set; }
        
        public string PlayerName;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            RegisterManager(new PopupManager());
            RegisterManager(objectPoolsController);

            GetComponentsInChildren(true, popups);
            
            foreach (var popup in popups)
            {
                Get<PopupManager>().RegisterPopup(popup);
            }
            
            LeaderboardModel = new LeaderboardModel();
        }

        private void Start()
        {
            Get<PopupManager>().Open<SplashPopup>().Forget();
        }

        private void OnDestroy()
        {
            Get<PopupManager>().Dispose();
        }

        public static T Get<T>() where T : IManager
        {
            return (T) Instance.Get(typeof(T));
        }

        private IManager Get(Type type)
        {
            return Instance.managers[type];
        }

        private void RegisterManager(IManager manager)
        {
            var managerType = manager.GetType();

            void AddToDictionary(Dictionary<Type, IManager> dict)
            {
                dict[managerType] = manager;
            }

            AddToDictionary(managers);
        }
    }
}
