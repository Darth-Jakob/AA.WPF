using AA.WPF.MVVM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.WPF.MVVM
{
    /// <summary>
    /// ServiceLocator Pattern 구현 class
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        #region Singleton
        
        private static readonly Lazy<ServiceLocator> lazy = new Lazy<ServiceLocator>(() => new ServiceLocator());
        public static ServiceLocator Instance 
        {
            get 
            {
                return lazy.Value;
            }
        }

        #endregion
        #region public Func
        
        public void RegisterService<T>(object service) where T : IService
        {
            if (service is IService)
            {
                if (RegistServices == null)
                {
                    RegistServices = new List<IService>();
                }

                var find = RegistServices.FirstOrDefault(o => o.GetType().Equals(typeof(T)));
                if (find != null)
                {
                    //기존에 등록된 service가 있음!
                    throw new Exception("Service is already resgist");
                }
                else
                {
                    RegistServices.Add((IService)service);
                }
            }
            else
            {
                throw new Exception("parameter is must be ISerive");
            }
        }

        public T GetService<T>() where T : IService
        {
            if (RegistServices == null)
            {
                //기존에 등록된 service가 없음
                throw new Exception("Service is regist first");
            }

            var find = RegistServices.FirstOrDefault(o => o.GetType().Equals(typeof(T)));
            if (find == null)
            {
                //기존에 등록된 service가 없음
                throw new Exception("Service is regist first");
            }
            else
            {
                return (T)find;
            }
        }

        #endregion
        #region variable & property
        internal IList<IService> RegistServices { get; private set; }
        #endregion
    }
}
