using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MCSLock.Tests
{
    [TestClass]
    public class UnitTest1
    {
        System.Threading.MCSLock _mCSLock = new System.Threading.MCSLock();
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var node1 = _mCSLock.Lock();
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                _mCSLock.Unlock(node1);
            });
            Assert.IsNotNull(node1);
            Assert.AreEqual(node1.Index, 1);
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 100);

            stopwatch.Start();
            var node2 = _mCSLock.Lock();
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                _mCSLock.Unlock(node2);
            });
            stopwatch.Stop();
            Assert.IsNotNull(node2);
            Assert.AreEqual(node2.Index, 2);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 2000);

            stopwatch.Start();
            var node3 = _mCSLock.Lock();
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
                _mCSLock.Unlock(node3);
            });
            stopwatch.Stop();
            Assert.IsNotNull(node3);
            Assert.AreEqual(node3.Index, 3);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > 5000);

        }
    }
}
