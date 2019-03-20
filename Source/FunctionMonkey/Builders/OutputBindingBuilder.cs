using System.IO;
using FunctionMonkey.Abstractions.Builders;
using FunctionMonkey.Abstractions.Builders.Model;
using FunctionMonkey.Model.OutputBindings;

namespace FunctionMonkey.Builders
{
    internal class OutputBindingBuilder<TParentBuilder> : IOutputBindingBuilder<TParentBuilder>
    {
        private readonly TParentBuilder _parentBuilder;
        private readonly AbstractFunctionDefinition _functionDefinition;

        public OutputBindingBuilder(TParentBuilder parentBuilder, AbstractFunctionDefinition functionDefinition)
        {
            _parentBuilder = parentBuilder;
            _functionDefinition = functionDefinition;
        }
        
        public TParentBuilder ServiceBusQueue(string connectionString, string queueName)
        {
            VerifyOutputBinding();
            _functionDefinition.OutputBinding = new ServiceBusQueueOutputBinding
            {
                ConnectionStringName = connectionString,
                QueueName = queueName
            };

            return _parentBuilder;
        }

        public TParentBuilder ServiceBusTopic(string connectionString, string topicName)
        {
            VerifyOutputBinding();
            _functionDefinition.OutputBinding = new ServiceBusTopicOutputBinding
            {
                ConnectionStringName = connectionString,
                TopicName = topicName
            };

            return _parentBuilder;
        }

        public TParentBuilder SignalRMessage(string hubName)
        {
            VerifyOutputBinding();
            throw new System.NotImplementedException();
        }

        public TParentBuilder SignalRGroup(string hubName)
        {
            VerifyOutputBinding();
            throw new System.NotImplementedException();
        }

        public TParentBuilder StorageBlob(string connectionStringSettingName, string name, FileAccess fileAccess = FileAccess.Write)
        {
            if (_functionDefinition.OutputBinding is null)
            {
                _functionDefinition.OutputBinding = new StorageBlobOutputBinding();
            }

            if (_functionDefinition.OutputBinding is StorageBlobOutputBinding blobBinding)
            {
                blobBinding.Outputs.Add(new StorageBlobOutput
                {
                    ConnectionStringSettingName = connectionStringSettingName,
                    FileAccess = fileAccess,
                    Name = name
                });
            }
            else
            {
                throw new ConfigurationException($"An output binding is already set for command {_functionDefinition.CommandType.Name}");
            }

            return _parentBuilder;
        }

        public TParentBuilder StorageQueue(string connectionStringSettingName, string queueName)
        {
            VerifyOutputBinding();
            throw new System.NotImplementedException();
        }

        public TParentBuilder StorageTable(string connectionStringSettingName, string tableName)
        {
            VerifyOutputBinding();
            throw new System.NotImplementedException();
        }

        public TParentBuilder Cosmos(string connectionStringSettingName, string databaseName, string leaseName)
        {
            VerifyOutputBinding();
            throw new System.NotImplementedException();
            
            // we can use the command output type to determine whether or not to use a IAsyncCollector or an out parameter
            // if its based on IEnumerable we do the former, otherwise the latter
        }

        private void VerifyOutputBinding()
        {
            if (_functionDefinition.OutputBinding != null)
            {
                throw new ConfigurationException($"An output binding is already set for command {_functionDefinition.CommandType.Name}");
            }
            
        }
    }
}