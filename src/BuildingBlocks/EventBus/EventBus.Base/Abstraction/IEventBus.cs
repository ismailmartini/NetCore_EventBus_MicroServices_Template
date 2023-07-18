using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Abstraction
{
    //microservislerinin subscription işlemlerini hangi event'i subscript  edeceğini söylidikleri eventbus olacak
    public interface IEventBus:IDisposable
    {
        void Publish(IntegrationEvent @event);
        
        void Subscribe<T,TH>() where T:IntegrationEvent where TH:IIntegrationEventHandler<T>;
        void UnSubscribe<T,TH>() where T:IntegrationEvent where TH:IIntegrationEventHandler<T>;
    }
}
