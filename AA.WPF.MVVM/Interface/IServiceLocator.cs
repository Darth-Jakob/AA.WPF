using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.WPF.MVVM
{
    /// <summary>
    /// ServiceLocator interface
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// 서비스 등록
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RegisterService<T>(object serivce) where T : IService;

        /// <summary>
        /// 서비스 호출
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetService<T>() where T : IService;
    }
}
