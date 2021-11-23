using UnityEngine;

namespace LEGOAquazone.Scripts.Helpers
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log(typeof(T).ToString() + " is NULL.");
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = (T)this;
        }

        protected virtual void Init()
        {

        }
    }
}

