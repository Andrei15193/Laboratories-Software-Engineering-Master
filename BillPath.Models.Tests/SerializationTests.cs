using System;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BillPath.Models.Tests
{
    [TestClass]
    public abstract class SerializationTests<T>
    {
        [TestInitialize]
        public virtual void TestInitialize()
        {
            Instance = GetNewInstance();
            SetValidTestDataToInstance();
        }

        protected virtual T GetNewInstance()
        {
            return Activator.CreateInstance<T>();
        }
        protected abstract void SetValidTestDataToInstance();
        protected T Instance
        {
            get;
            private set;
        }
        protected abstract void AssertInstanceIsEqualTo(T other);

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Instance = default(T);
        }

        [TestMethod]
        public void TestSerialization()
        {
            T deserializedInstance;

            using (var serializationStream = new MemoryStream())
            {
                var instanceSerializer = new DataContractSerializer(typeof(T));

                instanceSerializer.WriteObject(serializationStream, Instance);
                serializationStream.Seek(0, SeekOrigin.Begin);

                deserializedInstance = (T)instanceSerializer.ReadObject(serializationStream);
            }

            AssertInstanceIsEqualTo(deserializedInstance);
        }
    }
}