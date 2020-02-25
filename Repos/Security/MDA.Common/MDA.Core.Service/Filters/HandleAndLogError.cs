namespace MDA.Core.Service.Filters
{
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HandleAndLogErrorAttribute : Attribute, IErrorHandler, IServiceBehavior
    {
        /// <summary>
        /// Gets Event Log Source
        /// </summary>
        private string EventLogSource
        {
            get
            {
                return ConfigurationManager.AppSettings["EventLogSource"];
            }
        }

        /// <summary>
        /// Get Event Log Source Id
        /// </summary>
        private int EventLogSourceId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["EventLogSourceId"]);
            }
        }

        /// <summary>
        /// Default Implementation
        /// </summary>
        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Add Error Handler to Channel Dispatcher
        /// </summary>
        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                channelDispatcher.ErrorHandlers.Add(this);
            }
        }

        /// <summary>
        /// Handle the Exception and Log to Event Log
        /// </summary>
        /// <param name="error"></param>
        bool IErrorHandler.HandleError(Exception error)
        {
            var log = new EventLog { Source = EventLogSource };
            log.WriteEntry(error.ToString(), EventLogEntryType.Error, EventLogSourceId);

            return true;
        }

        /// <summary>
        /// Provide Custom Error Message to be delivered to the Client
        /// </summary>
        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var faultException = new FaultException(error.ToString());
            var messageFault = faultException.CreateMessageFault();

            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        /// <summary>
        /// Default Implementation
        /// </summary>
        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}